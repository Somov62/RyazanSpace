using RyazanSpace.Interfaces.Entities;
using RyazanSpace.Interfaces.Repositories;
using System.Net;
using System.Net.Http.Json;

namespace RyazanSpace.DAL.Client.Repositories.Base
{
    public class WebNamedRepository<T> : WebRepository<T>, INamedRepository<T> where T : INamedEntity
    {
        public WebNamedRepository(HttpClient client) : base(client) { }

        public async Task<bool> ExistName(string name, CancellationToken cancel = default)
        {
            var response = await HttpClient.GetAsync($"exist/name/{name}", cancel).ConfigureAwait(false);
            return response.IsSuccessStatusCode && response.StatusCode != HttpStatusCode.NotFound;
        }

        public async Task<T> GetByName(string name, CancellationToken cancel = default)
        {
            var response = await HttpClient.GetAsync($"find/name/{name}", cancel).ConfigureAwait(false);
            if (response.StatusCode == HttpStatusCode.NotFound)
                return default;
            return await response
                .EnsureSuccessStatusCode()
                .Content
                .ReadFromJsonAsync<T>(cancellationToken: cancel)
                .ConfigureAwait(false);
        }

        public async Task<T> DeleteByName(string name, CancellationToken cancel = default)
        {
            var response = await HttpClient.DeleteAsync($"{name}", cancel).ConfigureAwait(false);
            if (response.StatusCode == HttpStatusCode.NotFound)
                return default;

            return await response
              .EnsureSuccessStatusCode()
              .Content
              .ReadFromJsonAsync<T>(cancellationToken: cancel)
              .ConfigureAwait(false);
        }
    }
}
