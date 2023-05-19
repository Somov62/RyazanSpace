using RyazanSpace.DAL.Entities.Account;
using System.Security.Cryptography;
using System.Text;

namespace RyazanSpace.Domain.Auth.Services
{
    public class TokenGenerator
    {
        private readonly Random _random = new();
        private readonly string _symbols = "QWERTYUIOPASDFGHJKLZXCVBNMqwertyuiopasdfghjklzxcvbnm1234567890";
        public string GenerateToken(User user)
        {
            string data = RemixString(user.Email, user.Email.Length);
            data += RemixString(user.Name, user.Name.Length);
            data += RemixString(_symbols, 15);

            return Encrypt(data);
        }


        private string RemixString(string str, int outputLength)
        {
            string output = string.Empty;
            List<int> strIndexes = new List<int>();
            for (int i = 0; i < outputLength; i++)
            {
                int index = _random.Next(str.Length);
                while (strIndexes.Contains(index))
                {
                    index = _random.Next(str.Length);
                }
                output += str[index];
            }
            return output;
        }

        private string Encrypt(string token)
        {
            token = Convert.ToBase64String(Encoding.ASCII.GetBytes(token)).Replace("=", "");
            using MD5 md5 = MD5.Create();
            byte[] inputBytes = Encoding.UTF8.GetBytes(token);
            byte[] hashBytes = md5.ComputeHash(inputBytes);

            return Convert.ToHexString(hashBytes);
        }
    }
}
