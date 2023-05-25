namespace RyazanSpace.Core.Exceptions
{
    public class UnauthorizedException : Exception
    {
        public UnauthorizedException() : base("Токен доступа недействителен или устарел. Повторите процедуру аутентификации") { }
    }
}
