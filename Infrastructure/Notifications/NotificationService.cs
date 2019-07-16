using System.Threading.Tasks;
using Emails;
using Emails.ViewModels;
using Infrastructure.SmtpMailing;

namespace Infrastructure.Notifications
{
    public class NotificationService : INotificationService
    {
        private readonly IMailSenderService _mailSenderService;
        private readonly IRazorViewToStringRenderer _razorViewToStringRenderer;

        public NotificationService(IMailSenderService mailSenderService, IRazorViewToStringRenderer razorViewToStringRenderer)
        {
            _mailSenderService = mailSenderService;
            _razorViewToStringRenderer = razorViewToStringRenderer;
        }

        public async Task UserRegistered(string username, string email, string password, string token)
        {
            var emailView = new ConfirmMailboxEmailViewModel
            {
                Token = token,
                Username = username,
                Password = password
            };

            var message = new MailMessage
            {
                BodyHtml = await _razorViewToStringRenderer.RenderViewToStringAsync("ConfirmMailboxEmailView", emailView),
                From = "admin@example.com",
                To = email,
                Subject = "Thank you for your registration"
            };
            await _mailSenderService.SendEmailAsync(message);
        }
    }
}