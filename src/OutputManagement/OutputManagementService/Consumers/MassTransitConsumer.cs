using ContractManagement.Application.IntegrationEvents;

namespace OutputManagementService.Consumers;

public class MassTransitConsumer<T> : IConsumer<T> where T : class
{
    protected ILogger _logger;

    public MassTransitConsumer(ILogger logger)
    {
        this._logger = logger;
    }

    private void LogContextInformation(ConsumeContext<T> context)
    {
        string? eventType = context.Headers.Get<string>("EventType");

        _logger.LogInformation(
            "Consume {EventType} '{MessageType}'. Message: {@Message}",
            eventType,
            context.Message.GetType().Name,
            context.Message);
    }

    public Task Consume(ConsumeContext<T> context)
    {
        LogContextInformation(context);
        return ConsumeMessage(context.Message);
    }

    protected virtual Task ConsumeMessage(T message)
    {
        return Task.CompletedTask;
    }
}
