namespace Domain.Common;

/// <summary>
/// Represents an Entity in the domain (DDD).
/// </summary>
public abstract class Entity<TId> : IEquatable<Entity<TId>>, IEntity<TId>
{
    /// <summary>
    /// The unique Id of the entity.
    /// </summary>
    public TId Id { get; private set; }

    protected Entity(TId id)
    {
        Id = id;
    }

    public virtual void EnsureConsistency() { }

    public override bool Equals(object? obj)
    {
        if (obj is not null)
        {
            return Equals(obj as Entity<TId>);
        }
        return false;
    }

    public virtual bool Equals(Entity<TId>? other)
    {
        if (Id is null || other is null)
        {
            return false;
        }
        return Id.Equals(other.Id);

    }

    public override int GetHashCode()
    {
        if (Id is null)
        {
            return 0;
        }
        return Id.GetHashCode();
    }

    public static bool operator ==(Entity<TId> left, Entity<TId> right)
    {
        if (left.Id is null)
        {
            return false;
        }
        return left.Id.Equals(right.Id);
    }

    public static bool operator !=(Entity<TId> left, Entity<TId> right)
    {
        return !(left == right);
    }
}