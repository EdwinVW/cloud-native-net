namespace Application.Common.Interfaces;

public interface IProjectionEngine
{
    ValueTask RunProjectionsAsync(IEnumerable<Event> events);
}
