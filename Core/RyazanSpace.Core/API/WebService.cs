namespace RyazanSpace.Core.API
{
    public abstract class WebService
    {
        public WebService(HttpClient client) => HttpClient = client;
        protected HttpClient HttpClient { get; set; }
    }
}
