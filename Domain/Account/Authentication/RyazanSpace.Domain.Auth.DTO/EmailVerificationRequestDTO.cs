using System.ComponentModel.DataAnnotations;

namespace RyazanSpace.Domain.Auth.DTO
{
    public record EmailVerificationRequestDTO(
        [Required] int SessionId,
        [Required] int VerificationCode
        );
}
