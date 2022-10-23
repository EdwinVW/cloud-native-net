namespace Domain.Common;

public interface IAggregateRoot : IEntity
{
    bool IsNew { get; }

    bool IsValid { get; }

    uint Version { get; set; }

    void AddBusinessRuleViolation(string violation);

    IEnumerable<string> GetBusinessRuleViolations();

    IEnumerable<Event> GetDomainEvents();
}