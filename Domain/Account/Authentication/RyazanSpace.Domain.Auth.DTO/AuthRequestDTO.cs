using RyazanSpace.Interfaces.Validation;
using System.ComponentModel.DataAnnotations;

namespace RyazanSpace.Domain.Auth.DTO
{
    public record AuthRequestDTO(
        [Required] string Login,
        [Required][MD5] string Password );
}
