namespace Infrastructure.SmtpMailing
{
    public class MailMessage
    {
        public string From { get; set; }

        public string To { get; set; }

        public string Subject { get; set; }

        public string BodyHtml { get; set; }
    }
}