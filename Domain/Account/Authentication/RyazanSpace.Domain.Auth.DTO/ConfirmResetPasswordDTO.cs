using RyazanSpace.Core.Validation;
using System.ComponentModel.DataAnnotations;

namespace RyazanSpace.Domain.Auth.DTO
{
    public record ConfirmResetPasswordDTO(
        [Required] int SessionId,
        [Required] int VerificationCode,
        [Required][MD5] string NewPassword);
}
