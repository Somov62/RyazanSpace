using RyazanSpace.Core.DTO;
using RyazanSpace.DAL.Client.Repositories.Base;
using RyazanSpace.DAL.Entities.Groups;
using RyazanSpace.Interfaces.Repositories;
using System.Net.Http.Json;
using System.Net;

namespace RyazanSpace.DAL.Client.Repositories.Groups
{
    public class WebPostRepository : WebRepository<Post>
    {
        public WebPostRepository(HttpClient client) : base(client) { }

        public async Task<IPage<Post>> GetByGroupPage(
            int groupId,
            int pageIndex,
            int pageSize,
            CancellationToken cancel = default)
        {
            var response = await HttpClient.GetAsync($"group/{groupId}/page[{pageIndex}:{pageSize}]", cancel).ConfigureAwait(false);
            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return new Page<Post>(
                    Enumerable.Empty<Post>(),
                    0,
                    pageIndex,
                    pageSize
                );
            }

            return await response
                .EnsureSuccessStatusCode()
                .Content
                .ReadFromJsonAsync<Page<Post>>(cancellationToken: cancel)
                .ConfigureAwait(false);
        }
    }
}
