namespace Microsoft.Extensions.DependencyInjection;

public static class IServiceCollectionExtensions
{
    private static string CORP_NAME = "BankingCorp";

    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        return services
            .AddAggregates()
            .AddDomainServices()
            .AddApplicationServices()
            .AddCommandHandlers()
            .AddProjections();
    }

    private static IServiceCollection AddAggregates(this IServiceCollection services)
    {
        services
            .Scan(scan => scan
                .FromApplicationDependencies(a => a.FullName!.StartsWith(CORP_NAME))
                .AddClasses(classes => classes
                    .AssignableTo(typeof(IAggregateService<,>)), publicOnly: true)
                    .AsImplementedInterfaces()
                    .WithScopedLifetime());

        return services;
    }

    private static IServiceCollection AddDomainServices(this IServiceCollection services)
    {
        services
            .Scan(scan => scan
                .FromApplicationDependencies(a => a.FullName!.StartsWith(CORP_NAME))
                .AddClasses(classes => classes
                    .AssignableTo<IDomainService>(), publicOnly: true)
                    .AsImplementedInterfaces()
                    .WithScopedLifetime());

        return services;
    }

    private static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services
            .Scan(scan => scan
                .FromApplicationDependencies(a => a.FullName!.StartsWith(CORP_NAME))
                .AddClasses(classes => classes
                    .AssignableTo<IApplicationService>(), publicOnly: true)
                    .AsImplementedInterfaces()
                    .WithScopedLifetime());

        return services;
    }

    private static IServiceCollection AddCommandHandlers(this IServiceCollection services)
    {
        services
            .Scan(scan => scan
                .FromApplicationDependencies(a => a.FullName!.StartsWith(CORP_NAME))
                .AddClasses(classes => classes
                    .AssignableTo(typeof(ICommandHandler<>)), publicOnly: true)
                    .AsImplementedInterfaces()
                    .WithTransientLifetime());

        return services;
    }

    private static IServiceCollection AddProjections(this IServiceCollection services)
    {
        services
            .Scan(scan => scan
                .FromApplicationDependencies(a => a.FullName!.StartsWith(CORP_NAME))
                .AddClasses(classes => classes
                    .AssignableTo(typeof(IProjection<>)), publicOnly: true)
                    .AsImplementedInterfaces()
                    .WithScopedLifetime());

        return services;
    }
}
