using System.Net.Mail;

namespace RyazanSpace.Domain.Auth.Mails
{
    public class SuccessResetPasswordMessage : MailMessage
    {
        public SuccessResetPasswordMessage(string email, string username, string freezeRequestPath)
        {
            base.To.Add(new MailAddress(email, username));
            base.Subject = "Ryazan space - сброс пароля прошёл успешно";
            string pathToHtml = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Views", "SuccessResetPasswordMessage.html");
            base.Body = File.ReadAllText(pathToHtml).Replace("USERNAME", username).Replace("REPLACE_ADDRESS", freezeRequestPath);
            base.IsBodyHtml = true;
        }
    }
}
