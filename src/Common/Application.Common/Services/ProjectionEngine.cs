namespace Application.Common.Services;

public class ProjectionEngine : IApplicationService, IProjectionEngine
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger _logger;

    public ProjectionEngine(IServiceProvider serviceProvider, ILogger<ProjectionEngine> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    public async ValueTask RunProjectionsAsync(IEnumerable<Event> events)
    {
        foreach (var @event in events)
        {
            var eventType = @event.GetType();

            var projectionType = typeof(IProjection<>).MakeGenericType(eventType);

            _logger.LogInformation("Running projections for {EventType}...", eventType.Name);

            var projections = _serviceProvider.GetServices(projectionType);

            if (projections.Any())
            {
                foreach (object? projection in projections)
                {
                    MethodInfo? methodInfo = projectionType.GetMethod("ProjectAsync");
                    if (methodInfo != null)
                    {
                        ValueTask? task = (ValueTask?)methodInfo.Invoke(projection, new object[] { @event });
                        if (task.HasValue)
                        {
                            await task.Value;
                        }
                    }
                }
            }
        }
    }

}
