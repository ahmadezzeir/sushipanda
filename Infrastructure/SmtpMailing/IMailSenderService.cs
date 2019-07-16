using System.Threading.Tasks;

namespace Infrastructure.SmtpMailing
{
    public interface IMailSenderService
    {
        Task SendEmailAsync(MailMessage mailMessage);
    }
}
