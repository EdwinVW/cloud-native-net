using Domain.Common.Exceptions;

namespace Domain.Common;

/// <summary>
/// Base class for an aggregate root.
/// </summary>
public abstract class AggregateRoot : Entity, IAggregateRoot
{
    private List<string> _businessRuleViolations;

    /// <summary>
    /// Indication whether the aggregate in in a valid state (true) or not (false).
    /// </summary>
    public bool IsValid => !_businessRuleViolations.Any();

    public bool IsNew => Version.Value == 0;

    public virtual AggregateVersion Version { get; set; }

    /// <summary>
    /// The list of domain events that are created when handling a command.
    /// </summary>
    protected readonly List<Event> _domainEvents;

    /// <summary>
    /// Constructor for creating an empty aggregate.
    /// </summary>
    /// <remarks>This constructor can be used by an ORM.</remarks>
    public AggregateRoot() : this(string.Empty, 0)
    {
    }


    /// <summary>
    /// Constructor for creating an empty aggregate.
    /// </summary>
    /// <remarks>This constructor can be used by an ORM.</remarks>
    public AggregateRoot(string id) : this(id, 0)
    {
    }

    /// <summary>
    /// Constructor for creating an empty aggregate.
    /// </summary>
    /// <remarks>This constructor can be used by an ORM.</remarks>
    public AggregateRoot(string id, AggregateVersion originalVersion)
        : base(id)
    {
        _domainEvents = new();
        _businessRuleViolations = new();
        Version = originalVersion;
    }

    /// <summary>
    /// Constructor for creating a rehydrated aggregate.
    /// </summary>
    public AggregateRoot(string id, IList<Event> domainEvents)
        : this(id, (ulong)domainEvents.Count)
    {
        foreach (var domainEvent in domainEvents)
        {
            TryHandleDomainEvent(domainEvent);
        }

        ClearDomainEvents();
    }    

    /// <summary>
    /// Get the domain events that were created by executing a command.
    /// </summary>
    public IEnumerable<Event> GetDomainEvents()
    {
        return _domainEvents;
    }

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }

    protected void PublishDomainEvent(Event domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }

    public void AddBusinessRuleViolation(string violation)
    {
        _businessRuleViolations.Add(violation);
    }

    public IEnumerable<string> GetBusinessRuleViolations()
    {
        return _businessRuleViolations;
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
