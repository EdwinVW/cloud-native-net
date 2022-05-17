namespace Domain.Common;

public abstract class EntityCollection<TId, TEntity> : KeyedCollection<TId, TEntity>
    where TId: notnull
    where TEntity : Entity<TId>
{
    protected EntityCollection()
    {
    }

    protected EntityCollection(IEnumerable<TEntity> items)
    {
        foreach (var item in items)
        {
            Add(item);
        }
    }

    protected override TId GetKeyForItem(TEntity item) => item.Id;
}
