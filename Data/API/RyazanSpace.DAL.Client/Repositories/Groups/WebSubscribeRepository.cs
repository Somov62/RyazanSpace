using RyazanSpace.DAL.Client.Repositories.Base;
using RyazanSpace.DAL.Entities.Groups;
using System.Net.Http.Json;
using System.Net;
using RyazanSpace.DAL.Entities.Account;

namespace RyazanSpace.DAL.Client.Repositories.Groups
{
    public class WebSubscribeRepository : WebBaseRepository<GroupSubscriber>
    {
        public WebSubscribeRepository(HttpClient client) : base(client) { }

        public async Task<GroupSubscriber> GetById(int groupId, int userId, CancellationToken cancel = default)
        {
            var response = await HttpClient.GetAsync($"find[{groupId}:{userId}]", cancel).ConfigureAwait(false);
            if (response.StatusCode == HttpStatusCode.NotFound)
                return default;
            return await response
                .EnsureSuccessStatusCode()
                .Content
                .ReadFromJsonAsync<GroupSubscriber>(cancellationToken: cancel)
                .ConfigureAwait(false);
        }

        public async Task<bool> Exist(int groupId, int userId, CancellationToken cancel = default)
        {
            var response = await HttpClient.GetAsync($"exist[{groupId}:{userId}]", cancel).ConfigureAwait(false);
            return response.IsSuccessStatusCode && response.StatusCode != HttpStatusCode.NotFound;
        }

        public async Task<List<User>> GetGroupSubscribers(int groupId, CancellationToken cancel = default)
        {
            var response = await HttpClient.GetAsync($"{groupId}/subs", cancel).ConfigureAwait(false);
            return await response
                .EnsureSuccessStatusCode()
                .Content
                .ReadFromJsonAsync<List<User>>(cancellationToken: cancel)
                .ConfigureAwait(false);
        }

        public async Task<List<Group>> GetUserGroups(int userId, CancellationToken cancel = default)
        {
            var response = await HttpClient.GetAsync($"{userId}/groups", cancel).ConfigureAwait(false);
            return await response
                .EnsureSuccessStatusCode()
                .Content
                .ReadFromJsonAsync<List<Group>>(cancellationToken: cancel)
                .ConfigureAwait(false);
        }

        public async Task<int> GetCountGroupSubscribers(int groupId, CancellationToken cancel = default)
        {
            var response = await HttpClient.GetAsync($"{groupId}/subs/count", cancel).ConfigureAwait(false);
            return await response
                .EnsureSuccessStatusCode()
                .Content
                .ReadFromJsonAsync<int>(cancellationToken: cancel)
                .ConfigureAwait(false);
        }

        public async Task<int> GetCountUserGroups(int userId, CancellationToken cancel = default)
        {
            var response = await HttpClient.GetAsync($"{userId}/groups/count", cancel).ConfigureAwait(false);
            return await response
                .EnsureSuccessStatusCode()
                .Content
                .ReadFromJsonAsync<int>(cancellationToken: cancel)
                .ConfigureAwait(false);
        }
    }
}
