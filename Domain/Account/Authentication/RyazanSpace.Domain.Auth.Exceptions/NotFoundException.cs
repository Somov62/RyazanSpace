namespace RyazanSpace.Domain.Auth.Exceptions
{
    public class NotFoundException : ArgumentException
    {
        public NotFoundException(string message) : base(message) { }
    }
}
