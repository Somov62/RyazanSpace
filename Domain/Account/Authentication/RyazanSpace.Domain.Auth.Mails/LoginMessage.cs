using System.Net.Mail;

namespace RyazanSpace.Domain.Auth.Mails
{
    public class LoginMessage : MailMessage
    {
        public LoginMessage(string recipient, string resetAddress, string ipAddress)
        {
            base.To.Add(new MailAddress(recipient));
            base.Subject = "Ryazan Space - выполнен вход в аккаунт";
            string pathToHtml = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Views", "LoginMessage.html");
            base.Body = File.ReadAllText(pathToHtml).Replace("REPLACE_IPADDRESS", ipAddress).Replace("REPLACE_ADDRESS", resetAddress).Replace("REPLACE_DATE", DateTime.Now.ToString("F"));
            base.IsBodyHtml = true;
        }
    }
}
