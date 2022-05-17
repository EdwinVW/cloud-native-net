namespace Application.Common.Interfaces;

public interface IUnitOfWork
{
    void AddDomainEventToPublish(Event domainEvents);
    void AddDomainEventsToPublish(IEnumerable<Event> domainEvents);
    void AddIntegrationEventToPublish(Event integrationEvents);
    void AddIntegrationEventsToPublish(IEnumerable<Event> integrationEvents);
    Task CommitAsync();
}
