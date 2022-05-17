namespace ContractManagement.Infrastructure.Persistence.EFCore.Repositories.Aggregate;

public class AggregateEntity
{
    public EventSourcedEntityId AggregateId { get; set; } = default!;

    public AggregateVersion Version { get; set; } = default!;
}