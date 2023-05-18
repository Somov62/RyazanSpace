using System.ComponentModel.DataAnnotations;

namespace RyazanSpace.Domain.Auth.DTO
{
    public record ConfirmResetPasswordDTO(
        int SessionId, 
        int VerificationCode, 
        [RegularExpression("^[0-9a-fA-F]{32}$", ErrorMessage = "Пароль должен быть в формате MD5")]
        string NewPassword);
}
