namespace Domain.Common;

public interface IEntity<TId>
{
    TId Id { get; }

    void EnsureConsistency();
}
