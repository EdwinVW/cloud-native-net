using MassTransit;

namespace Microsoft.Extensions.DependencyInjection;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        string sqlConnectionString,
        int sqlMaxRetryCount,
        MassTransitOptions massTransitOptions)
    {
        // TODO Inject IConfiguration

        return services
            .AddDbContext(sqlConnectionString, sqlMaxRetryCount)
            .AddMassTransit(massTransitOptions)
            .AddRepositories()
            .AddUnitOfWork();
    }

    private static IServiceCollection AddDbContext(this IServiceCollection services, string connectionString, int maxRetryCount)
    {
        return services
            .AddDbContext<ServiceDbContext>(options => options
                .UseSqlServer(connectionString, sqlOptions => sqlOptions.EnableRetryOnFailure(maxRetryCount)))
                .AddScoped<DbContext>(sp => sp.GetRequiredService<ServiceDbContext>());
    }

    private static IServiceCollection AddMassTransit(this IServiceCollection services, MassTransitOptions massTransitOptions)
    {
        services.AddMassTransit(x =>
        {
            x.SetKebabCaseEndpointNameFormatter();

            var grpcOptions = massTransitOptions.GrpcOptions;
            var rabbitMQOptions = massTransitOptions.RabbitMqOptions;

            if (massTransitOptions.IsConsumer)
            {
                // TODO auto register EventHandlers

                x.RegisterEventHandler<ContractRegisteredV2, ContractRegisteredHandler>(services);
                x.RegisterEventHandler<ContractAmountChanged, ChangeContractAmountFeature>(services);
                x.RegisterEventHandler<ContractTermChanged, ChangeContractTermFeature>(services);
                x.RegisterEventHandler<ContractCancelled, ContractCancelledHandler>(services);
                x.RegisterEventHandler<CustomerRegistered, CustomerRegisteredHandler>(services);
                x.RegisterEventHandler<ProductRegistered, ProductRegisteredHandler>(services);
            }

            if (rabbitMQOptions != null)
            {
                x.UsingRabbitMq((context, config) =>
                {
                    config.Host(rabbitMQOptions.Host, h =>
                    {
                        h.Username(rabbitMQOptions.Username);
                        h.Password(rabbitMQOptions.Password);
                    });

                    config.ConfigureEndpoints(context);

                    config.UseMessageRetry(r =>
                    {
                        r.Incremental(5, TimeSpan.Zero, TimeSpan.FromSeconds(1));
                        r.Handle<Application.Common.Exceptions.ConcurrencyException>();
                    });
                });
            }
        });

        services.AddTransient<IEventPublisher, MassTransitEventPublisher>();

        return services;
    }

    private static void RegisterEventHandler<TEvent, THandler>(
        this IRegistrationConfigurator massTransitRegistrationConfigurator,
        IServiceCollection services)
        where TEvent : Domain.Common.Event
        where THandler : class, IEventHandler<TEvent>
    {
        // Add the application layer event handler to the container.
        services.AddTransient<IEventHandler<TEvent>, THandler>();

        // Add a Mass Transit consumer that subscribes to the event and forwards it to the event handler.
        massTransitRegistrationConfigurator.AddConsumer<EventConsumer<TEvent>>();
    }

    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services
            .AddScoped<IAggregateRepository<Contract>, EFEventSourcedAggregateRepository<Contract>>()
            .AddScoped<IContractReadModelRepository, EFContractReadModelRepository>()
            .AddScoped<ICustomerReadModelRepository, EFCustomerReadModelRepository>()
            .AddScoped<IProductReadModelRepository, EFProductReadModelRepository>();

        return services;
    }

    private static IServiceCollection AddUnitOfWork(this IServiceCollection services)
    {
        services
            .AddScoped<IUnitOfWork, PersistenceAndMessagingUnitOfWork>();

        return services;
    }
}
