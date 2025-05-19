using RyazanSpace.DAL.Entities.Account;
using RyazanSpace.DAL.Entities.Base;
using System.ComponentModel.DataAnnotations;

namespace RyazanSpace.DAL.Entities.Credentials
{
    public class UserToken : Entity
    {
        [Required]
        public string Token { get; set; }

        [Required]
        public DateTimeOffset DateExpire { get; set; }

        public string DeviceName { get; set; }

        public User Owner { get; set; }
    }
}
