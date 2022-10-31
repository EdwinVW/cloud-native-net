namespace Domain.Common;

public interface IEntity : IEquatable<IEntity>
{
    string Id { get; }
}
