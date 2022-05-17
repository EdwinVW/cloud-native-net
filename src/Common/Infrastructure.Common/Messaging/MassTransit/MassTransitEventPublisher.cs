using MassTransit;

namespace Infrastructure.Common.Messaging.MassTransit;

public class MassTransitEventPublisher : IEventPublisher
{
    private readonly IBus _bus;
    private readonly ILogger _logger;

    public MassTransitEventPublisher(IBus bus, ILogger<MassTransitEventPublisher> logger)
    {
        _bus = bus;
        _logger = logger;
    }

    public async ValueTask PublishDomainEventsAsync(IEnumerable<Domain.Common.Event> events)
    {
        foreach (var @event in events)
        {
            await PublishDomainEventAsync(@event);
        }
    }

    public async ValueTask PublishIntegrationEventsAsync(IEnumerable<Domain.Common.Event> events)
    {
        foreach (var @event in events)
        {
            await PublishIntegrationEventAsync(@event);
        }
    }    

    public async ValueTask PublishDomainEventAsync(Domain.Common.Event @event)
    {
        _logger.LogInformation("Publish {EventType} '{MessageType}'. Message: {@Message}", 
            "DomainEvent", @event.Type, @event);

        await _bus.Publish(
            message: @event, 
            messageType: @event.GetType(), 
            callback: ctx => ctx.Headers.Set("EventType", "DomainEvent")); 
    }

    public async ValueTask PublishIntegrationEventAsync(Domain.Common.Event @event)
    {
        _logger.LogInformation("Publish {EventType} '{MessageType}'. Message: {@Message}", 
            "IntegrationEvent", @event.Type, @event);

        await _bus.Publish(
            message: @event, 
            messageType: @event.GetType(), 
            callback: ctx => ctx.Headers.Set("EventType", "IntegrationEvent"));    
    }
}