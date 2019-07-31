using Services.Events.Models;

namespace Services.Events.EventHandling.Interfaces
{
    public interface IEventHandlerFactory
    {
        IEventHandler<TEvent> GetEventHandler<TEvent>() where TEvent : EventBase;
    }
}