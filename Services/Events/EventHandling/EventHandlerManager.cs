using System.Threading.Tasks;
using Services.Events.EventHandling.Interfaces;
using Services.Events.Models;

namespace Services.Events.EventHandling
{
    public class EventHandlerManager : IEventHandlerManager
    {
        private readonly IEventHandlerFactory _eventHandlerFactory;

        public EventHandlerManager(IEventHandlerFactory eventHandlerFactory)
        {
            _eventHandlerFactory = eventHandlerFactory;
        }

        public async Task HandleEvent<TEvent>(TEvent eventData) where TEvent : EventBase
        {
            var eventHandler = _eventHandlerFactory.GetEventHandler<TEvent>();
            await eventHandler.HandleEvent(eventData);
        }
    }
}