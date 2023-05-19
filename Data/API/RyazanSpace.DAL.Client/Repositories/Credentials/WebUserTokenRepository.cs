using RyazanSpace.DAL.Entities.Credentials;
using RyazanSpace.DAL.Client.Repositories.Base;
using System.Net;
using System.Net.Http.Json;

namespace RyazanSpace.DAL.Client.Repositories.Credentials
{
    public class WebUserTokenRepository : WebRepository<UserToken>
    {
        public WebUserTokenRepository(HttpClient client) : base(client) { }


        public async Task<bool> ExistToken(string token, CancellationToken cancel = default)
        {
            var response = await _client.GetAsync($"exist/token/{token}", cancel).ConfigureAwait(false);
            return response.IsSuccessStatusCode && response.StatusCode != HttpStatusCode.NotFound;
        }

        public async Task<UserToken> GetByToken(string token, CancellationToken cancel = default)
        {
            var response = await _client.GetAsync($"find/token/{token}", cancel).ConfigureAwait(false);
            if (response.StatusCode == HttpStatusCode.NotFound)
                return default;
            return await response
                .EnsureSuccessStatusCode()
                .Content
                .ReadFromJsonAsync<UserToken>(cancellationToken: cancel)
                .ConfigureAwait(false);
        }

        public async Task<UserToken> DeleteByToken(string token, CancellationToken cancel = default)
        {
            var response = await _client.DeleteAsync($"{token}", cancel).ConfigureAwait(false);
            if (response.StatusCode == HttpStatusCode.NotFound)
                return default;

            return await response
              .EnsureSuccessStatusCode()
              .Content
              .ReadFromJsonAsync<UserToken>(cancellationToken: cancel)
              .ConfigureAwait(false);
        }
    }
}
