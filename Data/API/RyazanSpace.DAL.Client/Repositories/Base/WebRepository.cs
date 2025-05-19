using RyazanSpace.Interfaces.Entities;
using RyazanSpace.Interfaces.Repositories;
using System.Net;
using System.Net.Http.Json;

namespace RyazanSpace.DAL.Client.Repositories.Base
{
    public class WebRepository<T> : WebBaseRepository<T>, IRepository<T> where T : IEntity
    {

        public WebRepository(HttpClient client) : base(client) { }

        
        public async Task<bool> ExistId(int id, CancellationToken cancel = default)
        {
            var response = await HttpClient.GetAsync($"exist/id/{id}", cancel).ConfigureAwait(false);
            return response.IsSuccessStatusCode && response.StatusCode != HttpStatusCode.NotFound;
        }

        public async Task<T> GetById(int id, CancellationToken cancel = default)
        {
            var response = await HttpClient.GetAsync($"{id}", cancel).ConfigureAwait(false);
            if (response.StatusCode == HttpStatusCode.NotFound)
                return default;
            return await response
                .EnsureSuccessStatusCode()
                .Content
                .ReadFromJsonAsync<T>(cancellationToken: cancel)
                .ConfigureAwait(false);
        }

        public async Task<T> DeleteById(int id, CancellationToken cancel = default)
        {
            var response = await HttpClient.DeleteAsync($"{id}", cancel).ConfigureAwait(false);
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