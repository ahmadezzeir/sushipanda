using System.Threading.Tasks;
using MimeKit;

namespace Infrastructure
{
    public interface IMailSenderService
    {
        Task SendEmailAsync(MailMessage mailMessage);
    }
}
