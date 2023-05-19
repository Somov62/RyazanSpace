﻿using RyazanSpace.Core.API;
using RyazanSpace.Interfaces.Entities;
using RyazanSpace.Interfaces.Repositories;
using System.Net;
using System.Net.Http.Json;

namespace RyazanSpace.DAL.Client.Repositories.Base
{
    public class WebRepository<T> : WebService, IRepository<T> where T : IEntity
    {

        public WebRepository(HttpClient client) : base(client) { }

        public async Task<bool> Exist(T item, CancellationToken cancel = default)
        {
            var response = await HttpClient.PostAsJsonAsync("exist", item, cancel).ConfigureAwait(false);
            return response.IsSuccessStatusCode && response.StatusCode != HttpStatusCode.NotFound;
        }

        public async Task<bool> ExistId(int id, CancellationToken cancel = default)
        {
            var response = await HttpClient.GetAsync($"exist/id/{id}", cancel).ConfigureAwait(false);
            return response.IsSuccessStatusCode && response.StatusCode != HttpStatusCode.NotFound;
        }

        public async Task<int> GetCount(CancellationToken cancel = default) =>
            await HttpClient.GetFromJsonAsync<int>("count", cancel).ConfigureAwait(false);

        public async Task<IEnumerable<T>> Get(int skip, int count, CancellationToken cancel = default) =>
            await HttpClient.GetFromJsonAsync<IEnumerable<T>>($"items[{skip}:{count}]", cancel).ConfigureAwait(false);

        public async Task<IEnumerable<T>> GetAll(CancellationToken cancel = default) =>
            await HttpClient.GetFromJsonAsync<IEnumerable<T>>("", cancel).ConfigureAwait(false);


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


        public async Task<IPage<T>> GetPage(int pageIndex, int pageSize, CancellationToken cancel = default)
        {
            var response = await HttpClient.GetAsync($"page[{pageIndex}:{pageSize}]", cancel).ConfigureAwait(false);
            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return new PageItems()
                {
                    Items = Enumerable.Empty<T>(),
                    PageIndex = pageIndex,
                    PageSize = pageSize,
                    TotalCount = 0
                };
            }
            return await response
                .EnsureSuccessStatusCode()
                .Content
                .ReadFromJsonAsync<PageItems>(cancellationToken: cancel)
                .ConfigureAwait(false);
        }

        private class PageItems : IPage<T>
        {
            public IEnumerable<T> Items { get; init; }
            public int TotalCount { get; init; }
            public int PageIndex { get; init; }
            public int PageSize { get; init; }
            public int TotalPagesCount => (int)Math.Ceiling((double)TotalCount / PageSize);
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
                return await response
                  .Content
                  .ReadFromJsonAsync<T>(cancellationToken: cancel)
                  .ConfigureAwait(false);

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