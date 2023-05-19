using RyazanSpace.Core.Exceptions;
using System.Net;

namespace RyazanSpace.Core.API
{
    public abstract class WebService
    {
        public WebService(HttpClient client) => HttpClient = client;
        protected HttpClient HttpClient { get; set; }

        protected async Task<Exception> ThrowWebException(HttpResponseMessage response, CancellationToken cancel)
        {
            string message = await response.Content.ReadAsStringAsync(cancel).ConfigureAwait(false);
            if (response.StatusCode == HttpStatusCode.NotFound)
                throw new NotFoundException(message);
            throw new WebException(message);
        }
    }
}
