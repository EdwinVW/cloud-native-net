namespace Application.Common.Interfaces;

public interface IProjection<in T> where T : Event
{
    ValueTask ProjectAsync(T domainEvent);
}
