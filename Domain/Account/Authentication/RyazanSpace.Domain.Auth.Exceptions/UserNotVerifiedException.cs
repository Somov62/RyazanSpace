namespace RyazanSpace.Domain.Auth.Exceptions
{
    public class UserNotVerifiedException : UnauthorizedAccessException
    {
        public UserNotVerifiedException(string message) : base(message) { }
    }
}
