using RyazanSpace.DAL.Entities.Account;
using RyazanSpace.DAL.Client.Repositories.Base;
using System.Net;
using System.Net.Http.Json;

namespace RyazanSpace.DAL.Client.Repositories.Account
{
    public class WebUserRepository : WebNamedRepository<User>
    {
        public WebUserRepository(HttpClient client) : base(client) { }

        public async Task<bool> ExistEmail(string email, CancellationToken cancel = default)
        {
            var response = await HttpClient.GetAsync($"exist/email/{email}", cancel).ConfigureAwait(false);
            return response.IsSuccessStatusCode && response.StatusCode != HttpStatusCode.NotFound;
        }

        public async Task<User> GetByEmail(string email, CancellationToken cancel = default)
        {
            var response = await HttpClient.GetAsync($"find/email/{email}", cancel).ConfigureAwait(false);
            if (response.StatusCode == HttpStatusCode.NotFound)
                return default;
            return await response
                .EnsureSuccessStatusCode()
                .Content
                .ReadFromJsonAsync<User>(cancellationToken: cancel)
                .ConfigureAwait(false);
        }
    }
}
