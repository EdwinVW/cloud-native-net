namespace Domain.Common;

/// <summary>
/// Represents an Entity in the domain (DDD).
/// </summary>
public abstract class Entity : IEntity
{
    /// <summary>
    /// The unique Id of the entity.
    /// </summary>
    /// <remarks></remarks>
    public abstract string Id { get; }

    public override bool Equals(object? obj)
    {
        if (obj is not null)
        {
            return Equals(obj as IEntity);
        }
        return false;
    }

    public virtual bool Equals(IEntity? other)
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

    public static bool operator ==(Entity left, Entity right)
    {
        if (left.Id is null)
        {
            return false;
        }
        return left.Id.Equals(right.Id);
    }

    public static bool operator !=(Entity left, Entity right)
    {
        return !(left == right);
    }
}