using MassTransit;

namespace Infrastructure.Common.Messaging.MassTransit;

public class EventConsumer<TEvent> : IConsumer<TEvent>
    where TEvent : Domain.Common.Event
{
    private readonly IEventHandler<TEvent> _eventHandler;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger _logger;

    public EventConsumer(
        IEventHandler<TEvent> eventHandler,
        IUnitOfWork unitOfWork,
        ILogger<EventConsumer<TEvent>> logger)
    {
        _eventHandler = eventHandler;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<TEvent> context)
    {
        string? eventType = context.Headers.Get<string>("EventType");

        _logger.LogInformation(
            "Consume {EventType} '{MessageType}'. Message: {@Message}.",
            eventType,
            typeof(TEvent).Name,
            context.Message);

        await _eventHandler.HandleAsync(context.Message);

        await _unitOfWork.CommitAsync();
    }
}