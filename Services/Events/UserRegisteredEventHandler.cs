using System.Threading.Tasks;
using Infrastructure.Notifications;
using Services.Events.EventHandling.Interfaces;
using Services.Events.Models;

namespace Services.Events
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