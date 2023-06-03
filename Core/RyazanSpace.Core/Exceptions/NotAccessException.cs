namespace RyazanSpace.Core.Exceptions
{
    public class NotAccessException : Exception
    {
        public NotAccessException() : base("У вас нет доступа к этому ресурсу или функции") { }
    }
}
