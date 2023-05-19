using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace RyazanSpace.Core.Validation
{
    public class EmailAttribute : ValidationAttribute
    {
        public EmailAttribute()
        {
            ErrorMessage = "Укажите корректную электронную почту";
        }
        
        public override bool IsValid(object value)
        {
            if (value == null) return true;
            if (value is string email)
            {
                string pattern = @"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*"
                         + "@"
                         + @"((([\w]+([-\w]*[\w]+)*\.)+[a-zA-Z]+)|"
                         + @"((([01]?[0-9]{1,2}|2[0-4][0-9]|25[0-5]).){3}[01]?[0-9]{1,2}|2[0-4][0-9]|25[0-5]))\z";
                return Regex.IsMatch(email, pattern);
            }
            return false;
        }
    }
}
