using System.Threading.Tasks;
using Services.Events.Models;

namespace Services.Events.EventHandling.Interfaces
{
    public interface IEventHandler<in TEvent> where TEvent : EventBase
    {
        Task HandleEvent(TEvent eventData);
    }
}