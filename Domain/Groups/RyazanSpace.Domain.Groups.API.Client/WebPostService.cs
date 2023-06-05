using RyazanSpace.Core.API;
using RyazanSpace.Core.DTO;
using RyazanSpace.Core.Exceptions;
using RyazanSpace.Domain.Groups.DTO;
using RyazanSpace.Interfaces.Repositories;
using System.Net;
using System.Net.Http.Json;

namespace RyazanSpace.Domain.Groups.API.Client
{
    public class WebPostService : WebService
    {
        public WebPostService(HttpClient client) : base(client) { }


        /// <summary>
        /// Позволяет получить страницу постов определенной группы
        /// </summary>
        /// <param name="groupId">Id группы</param>
        /// <param name="pageIndex">Номер страницы</param>
        /// <param name="pageSize">Размер страницы</param>
        /// <param name="token">Токен доступа</param>
        /// <param name="cancel"></param>
        /// <returns></returns>
        /// <exception cref="UnauthorizedException"></exception>
        /// <exception cref="NotFoundException"></exception>
        public async Task<IPage<PostDTO>> GetByGroupPage(
            int groupId,
            int pageIndex,
            int pageSize,
            CancellationToken cancel = default)
        {
            var response = await HttpClient.GetAsync($"group/{groupId}/page[{pageIndex}:{pageSize}]", cancel).ConfigureAwait(false);
            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return new Page<PostDTO>(
                    Enumerable.Empty<PostDTO>(),
                    0,
                    pageIndex,
                    pageSize
                );
            }

            return await response
                .EnsureSuccessStatusCode()
                .Content
                .ReadFromJsonAsync<Page<PostDTO>>(cancellationToken: cancel)
                .ConfigureAwait(false);
        }

        /// <summary>
        /// Позволяет получить список постов постранично
        /// </summary>
        /// <param name="pageIndex">Номер страницы</param>
        /// <param name="pageSize">Размер страницы</param>
        /// <param name="token">Токен доступа</param>
        /// <param name="cancel"></param>
        /// <returns></returns>
        /// <exception cref="UnauthorizedException"></exception>
        public async Task<IPage<PostDTO>> GetPage(
            int pageIndex,
            int pageSize,
            string token,
            CancellationToken cancel = default)
        {
            var response = await HttpClient.GetAsync($"page/{pageIndex}/{pageSize}?token={token}", cancel).ConfigureAwait(false);
            if (response.IsSuccessStatusCode)
                return await response
                 .Content
                     .ReadFromJsonAsync<Page<PostDTO>>(cancellationToken: cancel)
                     .ConfigureAwait(false);

            if (response.StatusCode == HttpStatusCode.Unauthorized)
                throw new UnauthorizedException();
            throw await ThrowWebException(response, cancel).ConfigureAwait(false);
        }

        /// <summary>
        /// Позволяет создать пост
        /// </summary>
        /// <param name="model"><see cref="CreatePostDTO"/></param>
        /// <param name="token">токен доступа создателя группы</param>
        /// <param name="cancel"></param>
        /// <returns><see cref="PostDTO"/></returns>
        /// <exception cref="UnauthorizedException"></exception>
        public async Task<PostDTO> CreatePost(CreatePostDTO model, string token, CancellationToken cancel = default)
        {
            var response = await HttpClient.PostAsJsonAsync($"?token={token}", model, cancel).ConfigureAwait(false);
            if (response.IsSuccessStatusCode)
                return await response
                 .Content
                     .ReadFromJsonAsync<PostDTO>(cancellationToken: cancel)
                     .ConfigureAwait(false);

            if (response.StatusCode == HttpStatusCode.Unauthorized)
                throw new UnauthorizedException();
            throw await ThrowWebException(response, cancel).ConfigureAwait(false);
        }
    }
}
