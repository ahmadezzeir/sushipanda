using System.Threading.Tasks;
using Infrastructure.Events.Models;

namespace Infrastructure.EventHandling.Interfaces
{
    public interface IEventHandler<in TEvent> where TEvent : EventBase
    {
        Task HandleEvent(TEvent eventData);
    }
}