using RyazanSpace.DAL.Entities.Base;
using RyazanSpace.DAL.Entities.Resources;
using System.ComponentModel.DataAnnotations;

namespace RyazanSpace.DAL.Entities.Account
{
    public class User : NamedEntity
    {

        [Required]
        [MaxLength(100)]
        public string Email { get; set; }

        [MaxLength(50)]
        public string Password { get; set; }

        public bool IsEmailVerified { get; set; }

        public Image ProfileImage { get; set; }
    }
}
