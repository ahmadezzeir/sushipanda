using System.Threading.Tasks;
using Services.Events.Models;

namespace Services.Events.EventHandling.Interfaces
{
    public interface IEventHandlerManager
    {
        Task HandleEvent<TEvent>(TEvent eventData) where TEvent : EventBase;
    }
}