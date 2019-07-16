using System.Threading.Tasks;
using Infrastructure.Events.Models;

namespace Infrastructure.EventHandling.Interfaces
{
    public interface IEventHandlerManager
    {
        Task HandleEvent<TEvent>(TEvent eventData) where TEvent : EventBase;
    }
}