using System.Net.Mail;

namespace RyazanSpace.Domain.Auth.Mails
{
    public class ResetPasswordMessage : MailMessage
    {
        public ResetPasswordMessage(string email, int code, string cancelRequestPath)
        {
            base.To.Add(new MailAddress(email, ""));
            base.Subject = "Ryazan space - восстановление пароля";
            string pathToHtml = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Views", "ResetPasswordMessage.html");
            base.Body = File.ReadAllText(pathToHtml).Replace("REPLACE_CODE", code.ToString()).Replace("REPLACE_ADDRESS", cancelRequestPath);
            base.IsBodyHtml = true;
        }
    }
}
