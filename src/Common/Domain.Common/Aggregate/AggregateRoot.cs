using Domain.Common.Exceptions;

namespace Domain.Common;

/// <summary>
/// Base class for an aggregate root.
/// </summary>
public abstract class AggregateRoot : Entity, IAggregateRoot
{
    private List<string> _businessRuleViolations;

    /// <summary>
    /// Indication whether the aggregate is in a valid state (true) or not (false).
    /// </summary>
    public bool IsValid => !_businessRuleViolations.Any();

    /// <summary>
    /// Indication whether the aggregate is new (true) or not (false). New means that no 
    /// events have been applied to this aggregate yet.
    /// </summary>
    public bool IsNew => Version == 0;

    /// <summary>
    /// The current version of the Aggregate.
    /// </summary>
    public uint Version { get; set; }

    /// <summary>
    /// The list of domain events that are created when handling a command.
    /// </summary>
    protected readonly List<Event> _domainEvents;

    /// <summary>
    /// Constructor for creating an empty aggregate.
    /// </summary>
    /// <remarks>This constructor can be used by an ORM.</remarks>
    public AggregateRoot() : this(0)
    {
    }

    /// <summary>
    /// Constructor for creating an empty aggregate.
    /// </summary>
    /// <remarks>This constructor can be used by an ORM.</remarks>
    public AggregateRoot(uint originalVersion)
    {
        _domainEvents = new();
        _businessRuleViolations = new();
        Version = originalVersion;
    }

    /// <summary>
    /// Constructor for creating a rehydrated aggregate.
    /// </summary>
    public AggregateRoot(IList<Event> domainEvents) : this((uint)domainEvents.Count)
    {
        foreach (var domainEvent in domainEvents)
        {
            TryHandleDomainEvent(domainEvent);
        }

        _domainEvents.Clear();
    }    

    /// <summary>
    /// Get the domain events that were created by executing a command.
    /// </summary>
    public IEnumerable<Event> GetDomainEvents()
    {
        return _domainEvents;
    }

    /// <summary>
    /// Add a domainevent as the result of handling a command for later processing.
    /// </summary>
    /// <param name="domainEvent">The domain event to add.</param>
    protected void AddDomainEvent(Event domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }

    /// <summary>
    /// Add a business-rule violation. This violation must be a clear description of the 
    /// business-rule that was violated.
    /// </summary>
    /// <param name="violation">The business-rule violation message to add.</param>
    public void AddBusinessRuleViolation(string violation)
    {
        _businessRuleViolations.Add(violation);
    }

    /// <summary>
    /// Get the list of business-rule violations that occurred.
    /// </summary>
    /// <returns></returns>
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

        // add the domain event
        AddDomainEvent(domainEvent);
    }


    /// <summary>
    /// Try to handle a domain event. This method must be implemented by deriving 
    /// aggregate roots. In this method, only internal state changes are allowed. This 
    /// is because this method is also called when replaying events when rehydrating 
    /// the state of the aggregate from the event store.
    /// </summary>
    /// <param name="domainEvent">The domain event to handle.</param>
    /// <returns>An indication whether the aggregate was able to handle to event (true) 
    /// or not (false).</returns>
    protected abstract bool TryHandleDomainEvent(Event domainEvent);    
}
