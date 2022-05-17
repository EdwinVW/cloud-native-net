namespace Application.Common.Interfaces;

public interface IEventPublisher
{
    ValueTask PublishDomainEventAsync(Event domainEvent);

    ValueTask PublishDomainEventsAsync(IEnumerable<Event> domainEvents);

    ValueTask PublishIntegrationEventAsync(Event integrationEvent);

    ValueTask PublishIntegrationEventsAsync(IEnumerable<Event> integrationEvents);    
}
