namespace ContractManagement.Infrastructure.Persistence.EFCore.Repositories.Aggregate;

public class EventEntity
{
    public Guid Id { get; set; }

    public EventSourcedEntityId AggregateId { get; set; } = default!;

    public AggregateVersion Version { get; set; } = default!;

    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    public string MessageType { get; set; } = string.Empty;

    public string EventData { get; set; } = string.Empty;
}