namespace Domain.Common;

public abstract class EntityCollection<TEntity> : KeyedCollection<string, TEntity>
    where TEntity : Entity
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

    protected override string GetKeyForItem(TEntity item) => item.Id;
}
