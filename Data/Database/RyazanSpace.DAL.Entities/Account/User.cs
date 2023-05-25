using RyazanSpace.DAL.Entities.Base;
using RyazanSpace.DAL.Entities.Resources.Base;
using System.ComponentModel.DataAnnotations;

namespace RyazanSpace.DAL.Entities.Account
{
    public class User : NamedEntity
    {

        [Required]
        [MaxLength(100)]
        public string Email { get; set; }

        [MaxLength(50)]
        [RegularExpression("^[0-9a-fA-F]{32}$", ErrorMessage = "Пароль должен быть в формате MD5")]
        public string Password { get; set; }

        public bool IsEmailVerified { get; set; }

        public CloudResource Avatar { get; set; }

        public string Status { get; set; }

        public DateTimeOffset RegDate { get; set; }
    }
}
