using RyazanSpace.DAL.Client.Repositories.Base;
using RyazanSpace.DAL.Entities.Groups;
using System.Net.Http.Json;

namespace RyazanSpace.DAL.Client.Repositories.Groups
{
    public class WebGroupsRepository : WebNamedRepository<Group>
    {
        public WebGroupsRepository(HttpClient client) : base(client) { }


        public async Task<List<Group>> GetManagedGroups(int userId, CancellationToken cancel = default)
        {
            var response = await HttpClient.GetAsync($"managed?userId={userId}", cancel).ConfigureAwait(false);
            return await response
                .EnsureSuccessStatusCode()
                .Content
                .ReadFromJsonAsync<List<Group>>(cancellationToken: cancel)
                .ConfigureAwait(false);
        }
    }
}
