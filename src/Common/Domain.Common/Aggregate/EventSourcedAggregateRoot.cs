namespace Domain.Common;

using Domain.Common.Exceptions;

/// <summary>
/// Base class for an event sourced aggregate root.
/// </summary>
public abstract class EventSourcedAggregateRoot : AggregateRoot<EventSourcedEntityId>
{
    protected EventSourcedAggregateRoot(EventSourcedEntityId id)
        : base(id, null)
    {
    }

    protected EventSourcedAggregateRoot(EventSourcedEntityId id, IList<Event> domainEvents)
        : base(id, (ulong)domainEvents.Count)
    {
        foreach (var domainEvent in domainEvents)
        {
            TryHandleDomainEvent(domainEvent);
        }

        ClearDomainEvents();
    }

    /// <summary>
    /// Let the aggregate handle an event and save it in the list of events
    /// so it can be used outside the aggregate (persisted, published on a bus, ...).
    /// </summary>
    /// <param name="domainEvent">The event to handle.</param>
    /// <remarks>Use GetEvents to retrieve the list of events.</remarks>
    protected void ApplyDomainEvent(Event domainEvent)
    {
        // let the derived aggregate handle the event
        bool domainEventHandled = TryHandleDomainEvent(domainEvent);

        // if it was not handled, there is no handler implemented for it
        if (!domainEventHandled)
        {
            throw new DomainEventHandlerNotFoundException(
                $"No handler found for {domainEvent.Type} domain event.");
        }

        // check the overall consistency of the aggregate after the changes
        EnsureConsistency();
        if (!IsValid)
        {
            return;
        }

        // publish the domain event
        PublishDomainEvent(domainEvent);
    }

    protected abstract bool TryHandleDomainEvent(Event domainEvent);
}
