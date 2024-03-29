namespace Domain.Common;

/// <summary>
/// Base class for an aggregate root.
/// </summary>
public abstract class AggregateRoot : Entity, IAggregateRoot
{
    private List<string> _businessRuleViolations;

    /// <summary>
    /// Indication whether the aggregate is new (true) or not (false). New means that no 
    /// events have been applied to this aggregate yet.
    /// </summary>
    public bool IsNew => Version == 0;

    /// <summary>
    /// Indication whether the aggregate is in a valid state (true) or not (false).
    /// </summary>
    public bool IsValid => !_businessRuleViolations.Any();

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
    public AggregateRoot()
    {
        _domainEvents = new();
        _businessRuleViolations = new();
        Version = 0;
    }

    /// <summary>
    /// Constructor for creating a rehydrated aggregate.
    /// </summary>
    public void ReplayEvents(IList<Event> domainEvents)
    {
        foreach (var domainEvent in domainEvents)
        {
            HandleDomainEvent(domainEvent);
        }
        Version = (uint)domainEvents.Count;
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
        HandleDomainEvent(domainEvent);

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
    /// Handle a domain event. This method must be implemented by deriving aggregate roots. 
    /// In this method, only internal state changes are allowed. This is because this method 
    /// is also called when replaying events when rehydrating the state of the aggregate from 
    /// the event store.
    /// </summary>
    /// <param name="domainEvent">The domain event to handle.</param>
    protected virtual void HandleDomainEvent(Event domainEvent)
    {
        
    }

    /// <summary>
    /// Check overall consistency of the aggregate. This method must be implemented by 
    /// deriving aggregate roots. Never throw an exception from this method. If a 
    /// consistency issues is detected, edd a business-rule violation (using the the 
    /// AddBusinessRuleViolation method) that clearly describes the violation.
    /// </summary>
    public virtual void EnsureConsistency()
    {
        // Implement in derived class
    }
}
