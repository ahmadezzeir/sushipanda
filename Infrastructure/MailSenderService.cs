using System.Threading.Tasks;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;

namespace Infrastructure
{
    public class MailSenderService : IMailSenderService
    {
        private readonly SmtpConfiguration _smtpConfiguration;

        public MailSenderService(IOptions<SmtpConfiguration> smtpConfigurationOptions)
        {
            _smtpConfiguration = smtpConfigurationOptions.Value;
        }

        public async Task SendEmailAsync(MailMessage mailMessage)
        {
            var msg = new MimeMessage
            {
                From = { new MailboxAddress(mailMessage.From) },
                To = { new MailboxAddress(mailMessage.To) },
                Subject = mailMessage.Subject,
                Body = new TextPart(TextFormat.Html)
                {
                    Text = mailMessage.BodyHtml
                }
            };

            using (var client = new SmtpClient())
            {
                client.ServerCertificateValidationCallback = (s, c, h, e) => true;
                await client.ConnectAsync(_smtpConfiguration.Host, _smtpConfiguration.Port);
                await client.AuthenticateAsync(_smtpConfiguration.Username, _smtpConfiguration.Password);
                await client.SendAsync(msg);
                await client.DisconnectAsync(true);
            }
        }
    }
}