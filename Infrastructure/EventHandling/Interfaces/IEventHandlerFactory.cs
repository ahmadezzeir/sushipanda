using Infrastructure.Events.Models;

namespace Infrastructure.EventHandling.Interfaces
{
    public interface IEventHandlerFactory
    {
        IEventHandler<TEvent> GetEventHandler<TEvent>() where TEvent : EventBase;
    }
}