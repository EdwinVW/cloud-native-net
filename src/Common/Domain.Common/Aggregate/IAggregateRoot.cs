namespace Domain.Common;

public interface IAggregateRoot : IEntity
{
    bool IsNew { get; }

    AggregateVersion Version { get; set; }

    void AddBusinessRuleViolation(string violation);

    bool IsValid { get; }

    IEnumerable<string> GetBusinessRuleViolations();

    IEnumerable<Event> GetDomainEvents();

    void ClearDomainEvents();
}