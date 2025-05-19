using RyazanSpace.Core.API;
using RyazanSpace.Interfaces.Entities;
using RyazanSpace.Interfaces.Repositories;
using System.Net.Http.Json;
using System.Net;
using RyazanSpace.Core.DTO;

namespace RyazanSpace.DAL.Client.Repositories.Base
{
    public class WebBaseRepository<T> : WebService, IBaseRepository<T> where T : IBaseEntity
    {
        public WebBaseRepository(HttpClient client) : base(client) { }

        public async Task<bool> Exist(T item, CancellationToken cancel = default)
        {
            var response = await HttpClient.PostAsJsonAsync("exist", item, cancel).ConfigureAwait(false);
            return response.IsSuccessStatusCode && response.StatusCode != HttpStatusCode.NotFound;
        }

        public async Task<IPage<T>> GetPage(int pageIndex, int pageSize, CancellationToken cancel = default)
        {
            var response = await HttpClient.GetAsync($"page[{pageIndex}:{pageSize}]", cancel).ConfigureAwait(false);
            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return new Page<T>(
                    Enumerable.Empty<T>(),
                    0,
                    pageIndex,
                    pageSize
                );
            }
            return await response
                .EnsureSuccessStatusCode()
                .Content
                .ReadFromJsonAsync<Page<T>>(cancellationToken: cancel)
                .ConfigureAwait(false);
        }

        public async Task<T> Add(T item, CancellationToken cancel = default)
        {
            var response = await HttpClient.PostAsJsonAsync("", item, cancel).ConfigureAwait(false);
            return await response
              .EnsureSuccessStatusCode()
              .Content
              .ReadFromJsonAsync<T>(cancellationToken: cancel)
              .ConfigureAwait(false);
        }

        public async Task<T> Update(T item, CancellationToken cancel = default)
        {
            var response = await HttpClient.PutAsJsonAsync("", item, cancel).ConfigureAwait(false);
            if (response.StatusCode == HttpStatusCode.NotFound)
                return default;

            return await response
              .EnsureSuccessStatusCode()
              .Content
              .ReadFromJsonAsync<T>(cancellationToken: cancel)
              .ConfigureAwait(false);
        }

        public async Task<T> Delete(T item, CancellationToken cancel = default)
        {
            var request = new HttpRequestMessage(HttpMethod.Delete, "")
            {
                Content = JsonContent.Create(item)
            };

            var response = await HttpClient.SendAsync(request, cancel).ConfigureAwait(false);
            if (response.StatusCode == HttpStatusCode.NotFound)
                return default;

            return await response
              .EnsureSuccessStatusCode()
              .Content
              .ReadFromJsonAsync<T>(cancellationToken: cancel)
              .ConfigureAwait(false);
        }

        public async Task<int> GetCount(CancellationToken cancel = default) =>
            await HttpClient.GetFromJsonAsync<int>("count", cancel).ConfigureAwait(false);

        public async Task<IEnumerable<T>> Get(int skip, int count, CancellationToken cancel = default) =>
            await HttpClient.GetFromJsonAsync<IEnumerable<T>>($"items[{skip}:{count}]", cancel).ConfigureAwait(false);

        public async Task<IEnumerable<T>> GetAll(CancellationToken cancel = default) =>
            await HttpClient.GetFromJsonAsync<IEnumerable<T>>("", cancel).ConfigureAwait(false);

    }
}
