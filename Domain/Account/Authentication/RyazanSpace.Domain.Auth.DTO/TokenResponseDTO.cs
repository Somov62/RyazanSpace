namespace RyazanSpace.Domain.Auth.DTO
{
    public record TokenResponseDTO(string Token, DateTime DateExpired);
}
