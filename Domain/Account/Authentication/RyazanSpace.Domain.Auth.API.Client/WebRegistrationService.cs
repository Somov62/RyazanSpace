using RyazanSpace.Core.API;

namespace RyazanSpace.Domain.Auth.API.Client
{
    public class WebRegistrationService : WebService
    {
        public WebRegistrationService(HttpClient client) : base(client) { }
    }
}
