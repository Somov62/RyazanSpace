using System.ComponentModel.DataAnnotations;

namespace RyazanSpace.Domain.Auth.DTO
{
    public class EmailVerificationRequestDTO
    {
        [Required]
        public int SessionId { get; set; }

        [Required]
        public int VerificationCode { get; set; }
    }
}
