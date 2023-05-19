using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace RyazanSpace.Core.Validation
{
    public class MD5Attribute : ValidationAttribute
    {
        public MD5Attribute()
        {
            ErrorMessage = "Пароль должен быть в формате MD5";
        }

        public override bool IsValid(object value)
        {
            if (value == null) return true;
            if (value is string email)
            {
                string pattern = "^[0-9a-fA-F]{32}$";
                return Regex.IsMatch(email, pattern);
            }
            return false;
        }
    }
}
