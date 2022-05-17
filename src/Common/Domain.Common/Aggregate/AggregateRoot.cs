namespace Domain.Common;

/// <summary>
/// Base class for an aggregate root.
/// </summary>
public abstract class AggregateRoot<TId> : Entity<TId>, IAggregateRoot<TId>
{
    private List<string> _businessRuleViolations;

    public bool IsNew => Version is null;

    public virtual AggregateVersion? Version { get; set; }

    /// <summary>
    /// The list of domain events that are created when handling a command.
    /// </summary>
    protected readonly List<Event> _domainEvents;

    public bool IsConsistent { get; private set; }

    /// <summary>
    /// Constructor for creating an empty aggregate.
    /// </summary>
    /// <remarks>This constructor can be used by an ORM.</remarks>
    protected AggregateRoot(TId id, AggregateVersion? originalVersion = null)
        : base(id)
    {
        _domainEvents = new();
        _businessRuleViolations = new List<string>();
        IsConsistent = true;
        Version = originalVersion;
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
        IsConsistent = false;
    }

    public IEnumerable<string> GetBusinessRuleViolations()
    {
        return _businessRuleViolations;
    }
}
