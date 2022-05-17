namespace Application.Common.Interfaces;

public interface IEventHandler<TEvent> where TEvent : Event
{
    ValueTask HandleAsync(TEvent domainEvent);
}
