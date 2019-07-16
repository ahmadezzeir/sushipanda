using System.Threading.Tasks;
using Infrastructure.EventHandling.Interfaces;
using Infrastructure.Events.Models;
using Infrastructure.Notifications;

namespace Infrastructure.Events
{
    public class UserRegisteredEventHandler : IEventHandler<UserRegisteredEvent>
    {
        private readonly INotificationService _notificationService;

        public UserRegisteredEventHandler(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        public async Task HandleEvent(UserRegisteredEvent eventData)
        {
            await _notificationService.UserRegistered(eventData.Username, eventData.Email,
                eventData.Password, eventData.Token);
        }
    }
}