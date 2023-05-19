using System.Net.Mail;
using System.Net;

namespace RyazanSpace.MailService
{
    public class EmailSender
    {
        private readonly NetworkCredential _credentials;

        public EmailSender(NetworkCredential credentials) => _credentials = credentials;

        public async Task SendEmailAsync(MailMessage message, CancellationToken cancel = default)
        {
            if (message.From == default) 
            {
                message.From = new MailAddress(_credentials.UserName, "Ryazan Space");
            }

            SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587)
            {
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = _credentials,
                EnableSsl = true
            };
            try
            {
                await smtp.SendMailAsync(message, cancel);
            }
            catch { }

        }
    }
}