using System.Net.Mail;

namespace RyazanSpace.Domain.Auth.Mails
{
    public class SuccessEmailVerificationMessage : MailMessage
    {
        public SuccessEmailVerificationMessage(string email, string username) 
        {
            base.To.Add(new MailAddress(email, username));
            base.Subject = "Добро пожаловать в Ryazan Space!";
            string pathToHtml = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Views", "SuccessEmailVerificationMessage.html");
            base.Body = File.ReadAllText(pathToHtml).Replace("USERNAME", username);
            base.IsBodyHtml = true;
        }
    }
}
