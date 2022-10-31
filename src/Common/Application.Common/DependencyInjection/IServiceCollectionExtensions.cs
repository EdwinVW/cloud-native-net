namespace Microsoft.Extensions.DependencyInjection;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services, string assemblyNamePrefix)
    {
        return services
            .AddAggregates(assemblyNamePrefix)
            .AddDomainServices(assemblyNamePrefix)
            .AddApplicationServices(assemblyNamePrefix)
            .AddCommandHandlers(assemblyNamePrefix)
            .AddProjections(assemblyNamePrefix);
    }

    private static IServiceCollection AddAggregates(this IServiceCollection services, string assemblyNamePrefix)
    {
        services
            .Scan(scan => scan
                .FromApplicationDependencies(a => a.FullName!.StartsWith(assemblyNamePrefix))
                .AddClasses(classes => classes
                    .AssignableTo(typeof(IAggregateService<>)), publicOnly: true)
                    .AsImplementedInterfaces()
                    .WithScopedLifetime());

        return services;
    }

    private static IServiceCollection AddDomainServices(this IServiceCollection services, string assemblyNamePrefix)
    {
        services
            .Scan(scan => scan
                .FromApplicationDependencies(a => a.FullName!.StartsWith(assemblyNamePrefix))
                .AddClasses(classes => classes
                    .AssignableTo<IDomainService>(), publicOnly: true)
                    .AsImplementedInterfaces()
                    .WithScopedLifetime());

        return services;
    }

    private static IServiceCollection AddApplicationServices(this IServiceCollection services, string assemblyNamePrefix)
    {
        services
            .Scan(scan => scan
                .FromApplicationDependencies(a => a.FullName!.StartsWith(assemblyNamePrefix))
                .AddClasses(classes => classes
                    .AssignableTo<IApplicationService>(), publicOnly: true)
                    .AsImplementedInterfaces()
                    .WithScopedLifetime());

        return services;
    }

    private static IServiceCollection AddCommandHandlers(this IServiceCollection services, string assemblyNamePrefix)
    {
        services
            .Scan(scan => scan
                .FromApplicationDependencies(a => a.FullName!.StartsWith(assemblyNamePrefix))
                .AddClasses(classes => classes
                    .AssignableTo(typeof(ICommandHandler<>)), publicOnly: true)
                    .AsImplementedInterfaces()
                    .WithTransientLifetime());

        return services;
    }

    private static IServiceCollection AddProjections(this IServiceCollection services, string assemblyNamePrefix)
    {
        services
            .Scan(scan => scan
                .FromApplicationDependencies(a => a.FullName!.StartsWith(assemblyNamePrefix))
                .AddClasses(classes => classes
                    .AssignableTo(typeof(IProjection<>)), publicOnly: true)
                    .AsImplementedInterfaces()
                    .WithScopedLifetime());

        return services;
    }
}
