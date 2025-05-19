using System.Net.Mail;

namespace RyazanSpace.Domain.Auth.Mails
{
    public class EmailVerificationMessage : MailMessage
    {
        public EmailVerificationMessage(string recipient, int code)
        { 
            base.To.Add(new MailAddress(recipient));
            base.Subject = "Ryazan space - завершение регистрации";
            string pathToHtml = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Views", "EmailVerificationMessage.html");
            base.Body = File.ReadAllText(pathToHtml).Replace("REPLACE_CODE", code.ToString());
            base.IsBodyHtml = true;
        }
    }
}
