namespace RyazanSpace.Domain.Auth.Exceptions
{
    public class TimeOutSessionException : TimeoutException
    {
        public TimeOutSessionException(string message) : base(message) { }
    }
}
