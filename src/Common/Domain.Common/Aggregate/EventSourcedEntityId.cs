namespace Domain.Common;

public record EventSourcedEntityId(string Value)
{
    public static implicit operator string(EventSourcedEntityId aggregateId) => aggregateId.Value;

    public static implicit operator EventSourcedEntityId(string aggregateId) => new EventSourcedEntityId(aggregateId);
}
