namespace RyazanSpace.Domain.Auth.Exceptions
{
    /// <summary>
    /// Исключение, возникающее в случае попытки зарегистрировать пользователя с занятыми учетными данными
    /// </summary>
    public class UserAlreadyExistsException : Exception
    {
        public UserAlreadyExistsException(string message = null) : base(message) { }
    }
}