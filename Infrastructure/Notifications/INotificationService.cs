using System.Threading.Tasks;

namespace Infrastructure.Notifications
{
    public interface INotificationService
    {
        Task UserRegistered(string username, string email, string password, string token);
    }
}