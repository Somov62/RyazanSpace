using RyazanSpace.DAL.Entities.Account;
using RyazanSpace.DAL.WebApiClients.Repositories.Base;
using System.Net;

namespace RyazanSpace.DAL.WebApiClients.Repositories.Account
{
    public class WebUserRepository : WebNamedRepository<User>
    {
        public WebUserRepository(HttpClient client) : base(client) { }

        public async Task<bool> ExistEmail(string email, CancellationToken cancel = default)
        {
            var response = await _client.GetAsync($"exist/email/{email}", cancel).ConfigureAwait(false);
            return response.IsSuccessStatusCode && response.StatusCode != HttpStatusCode.NotFound;
        }
    }
}
