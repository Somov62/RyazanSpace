using RyazanSpace.Core.API;
using RyazanSpace.Core.DTO;
using RyazanSpace.Core.Exceptions;
using RyazanSpace.Domain.Groups.DTO;
using RyazanSpace.Interfaces.Repositories;
using System.Net;
using System.Net.Http.Json;

namespace RyazanSpace.Domain.Groups.API.Client
{
    public class WebGroupService : WebService
    {
        public WebGroupService(HttpClient client) : base(client) { }


        /// <summary>
        /// Позволяет создать группу
        /// </summary>
        /// <param name="model"><see cref="CreateGroupDTO"/></param>
        /// <param name="token">токен доступа создателя группы</param>
        /// <param name="cancel"></param>
        /// <returns>id созданной группы</returns>
        /// <exception cref="UnauthorizedException"></exception>
        public async Task<int> CreateGroup(CreateGroupDTO model, string token, CancellationToken cancel = default)
        {
            var response = await HttpClient.PostAsJsonAsync($"?token={token}", model, cancel).ConfigureAwait(false);
            if (response.IsSuccessStatusCode)
                return await response
                 .Content
                     .ReadFromJsonAsync<int>(cancellationToken: cancel)
                     .ConfigureAwait(false);

            if (response.StatusCode == HttpStatusCode.Unauthorized)
                throw new UnauthorizedException();
            throw await ThrowWebException(response, cancel).ConfigureAwait(false);
        }

        /// <summary>
        /// Позволяет установить аватар на группу
        /// </summary>
        /// <param name="groupId">id группы</param>
        /// <param name="imageId">id фотографии</param>
        /// <param name="token">токен доступа владельца группы</param>
        /// <param name="cancel"></param>
        /// <returns></returns>
        /// <exception cref="UnauthorizedException"></exception>
        /// <exception cref="NotFoundException"></exception>
        /// <exception cref="NotAccessException"></exception>
        public async Task<CloudResourceDTO> SetLogo(
            int groupId,
            int imageId,
            string token,
            CancellationToken cancel = default)
        {
            var response = await HttpClient.PostAsync($"{groupId}/logo?imageId={imageId}&token={token}", null, cancel).ConfigureAwait(false);
            if (response.IsSuccessStatusCode)
                return await response
                 .Content
                     .ReadFromJsonAsync<CloudResourceDTO>(cancellationToken: cancel)
                     .ConfigureAwait(false);

            if (response.StatusCode == HttpStatusCode.Unauthorized)
                throw new UnauthorizedException();
            if (response.StatusCode == HttpStatusCode.Forbidden)
                throw new NotAccessException();

            string message = await response.Content.ReadAsStringAsync(cancel).ConfigureAwait(false);
            if (response.StatusCode == HttpStatusCode.NotFound)
                throw new NotFoundException(message);
            
            throw await ThrowWebException(response, cancel).ConfigureAwait(false);
        }


        /// <summary>
        /// Позволяет получить данные группы, такие как:<br/>
        /// Имя, аватар, описание, дата регистрации
        /// </summary>
        /// <param name="groupId">id группы</param>
        /// <param name="token"></param>
        /// <param name="cancel"></param>
        /// <returns></returns>
        /// <exception cref="UnauthorizedException"></exception>
        /// <exception cref="NotFoundException"></exception>
        public async Task<GroupDTO> GetGroupById(int groupId, string token, CancellationToken cancel = default)
        {
            var response = await HttpClient.GetAsync($"{groupId}?token={token}", cancel).ConfigureAwait(false);
            if (response.IsSuccessStatusCode)
                return await response
                 .Content
                     .ReadFromJsonAsync<GroupDTO>(cancellationToken: cancel)
                     .ConfigureAwait(false);

            if (response.StatusCode == HttpStatusCode.Unauthorized)
                throw new UnauthorizedException();

            string message = await response.Content.ReadAsStringAsync(cancel).ConfigureAwait(false);
            if (response.StatusCode == HttpStatusCode.NotFound)
                throw new NotFoundException(message);
            
            throw await ThrowWebException(response, cancel).ConfigureAwait(false);
        }

        /// <summary>
        /// Позволяет установить описание в группе
        /// </summary>
        /// <param name="description">новое описание</param>
        /// <param name="groupId">id группы</param>
        /// <param name="token">токен доступа владельца группы</param>
        /// <param name="cancel"></param>
        /// <returns></returns>
        /// <exception cref="UnauthorizedException"></exception>
        /// <exception cref="NotFoundException"></exception>
        /// <exception cref="NotAccessException"></exception>
        public async Task SetDescription(
           int groupId,
           string description,
           string token,
           CancellationToken cancel = default)
        {
            var response = await HttpClient.PostAsync($"{groupId}/description?description={description}&token={token}", null, cancel).ConfigureAwait(false);
            if (response.IsSuccessStatusCode)
                return;

            if (response.StatusCode == HttpStatusCode.Unauthorized)
                throw new UnauthorizedException();
            if (response.StatusCode == HttpStatusCode.Forbidden)
                throw new NotAccessException();

            string message = await response.Content.ReadAsStringAsync(cancel).ConfigureAwait(false);
            if (response.StatusCode == HttpStatusCode.NotFound)
                throw new NotFoundException(message);
            
            throw await ThrowWebException(response, cancel).ConfigureAwait(false);
        }

        /// <summary>
        /// Позволяет получить список групп постранично
        /// </summary>
        /// <param name="pageIndex">Номер страницы</param>
        /// <param name="pageSize">Размер страницы</param>
        /// <param name="token">Токен доступа</param>
        /// <param name="cancel"></param>
        /// <returns></returns>
        /// <exception cref="UnauthorizedException"></exception>
        public async Task<IPage<GroupDTO>> GetPage(
            int pageIndex,
            int pageSize,
            string token,
            CancellationToken cancel = default)
        {
            var response = await HttpClient.GetAsync($"page/{pageIndex}/{pageSize}?token={token}", cancel).ConfigureAwait(false);
            if (response.IsSuccessStatusCode)
                return await response
                 .Content
                     .ReadFromJsonAsync<Page<GroupDTO>>(cancellationToken: cancel)
                     .ConfigureAwait(false);

            if (response.StatusCode == HttpStatusCode.Unauthorized)
                throw new UnauthorizedException();
            throw await ThrowWebException(response, cancel).ConfigureAwait(false);
        }

        /// <summary>
        /// Возвращает список управляемых пользователем групп
        /// </summary>
        /// <param name="token">токен пользователя</param>
        /// <param name="userId">при значении 0 вернет список управляемых групп владельца токена</param>
        /// <param name="cancel"></param>
        /// <returns></returns>
        /// <exception cref="UnauthorizedException"></exception>
        public async Task<List<GroupDTO>> GetManagedGroups(string token, int userId = 0, CancellationToken cancel = default)
        {
            var response = await HttpClient.GetAsync($"managed?token={token}&userId={userId}", cancel).ConfigureAwait(false);
            if (response.IsSuccessStatusCode)
                return await response
                .EnsureSuccessStatusCode()
                .Content
                .ReadFromJsonAsync<List<GroupDTO>>(cancellationToken: cancel)
                .ConfigureAwait(false);

            if (response.StatusCode == HttpStatusCode.Unauthorized)
                throw new UnauthorizedException();
            throw await ThrowWebException(response, cancel).ConfigureAwait(false);
        }
    }
}