using RyazanSpace.Interfaces.Validation;
using System.ComponentModel.DataAnnotations;

namespace RyazanSpace.Domain.Auth.DTO
{
    public record ResetPasswordRequestDTO(
        [Required] string UserName,
        [Required][Email] string UserEmail
        );
}
