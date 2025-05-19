using RyazanSpace.Core.API;
using RyazanSpace.Interfaces.Cloud;
using System.Net.Http.Json;

namespace RyazanSpace.Services.ServerCloud.API.Client
{
    public class WebServerCloudService : WebService, ICloud
    {
        public WebServerCloudService(HttpClient client) : base(client) 
        {
            client.BaseAddress = new Uri("http://localhost:5005/Cloud");
        }

        public async Task<string> Upload(byte[] file, CancellationToken cancel)
        {
            var response = await HttpClient.PostAsJsonAsync("", file, cancel).ConfigureAwait(false);
            if (response.IsSuccessStatusCode)
                if (response.Headers.TryGetValues("Location", out IEnumerable<string> values))
                    return values.First();
                
            return null;
        }
    }
}