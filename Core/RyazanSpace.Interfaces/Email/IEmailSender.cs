using System.Net.Mail;

namespace RyazanSpace.Interfaces.Email
{
    public interface IEmailSender
    {
        Task SendEmailAsync(MailMessage message, CancellationToken cancel = default);
    }
}
