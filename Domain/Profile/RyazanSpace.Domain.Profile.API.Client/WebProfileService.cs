using RyazanSpace.Core.API;
using RyazanSpace.Core.DTO;
using RyazanSpace.Core.Exceptions;
using RyazanSpace.Domain.Profile.DTO;
using System.Net;
using System.Net.Http.Json;

namespace RyazanSpace.Domain.Profile.API.Client
{
    public class WebProfileService : WebService
    {
        public WebProfileService(HttpClient client) : base(client) { }

        /// <summary>
        /// Позволяет установить аватар на профиль пользователя
        /// </summary>
        /// <param name="resourceId">id фотографии</param>
        /// <param name="token">токен доступа владельца страницы</param>
        /// <param name="cancel"></param>
        /// <returns></returns>
        /// <exception cref="UnauthorizedException"></exception>
        /// <exception cref="NotFoundException"></exception>
        public async Task<CloudResourceDTO> SetAvatar(
            int imageId, 
            string token,
            CancellationToken cancel = default)
        {
            var response = await HttpClient.PostAsync($"avatar?imageId={imageId}&token={token}", null, cancel).ConfigureAwait(false);
            if (response.IsSuccessStatusCode)
                return await response
                 .Content
                     .ReadFromJsonAsync<CloudResourceDTO>(cancellationToken: cancel)
                     .ConfigureAwait(false);

            string message = await response.Content.ReadAsStringAsync(cancel).ConfigureAwait(false);
            if (response.StatusCode == HttpStatusCode.NotFound)
                throw new NotFoundException(message);
            if (response.StatusCode == HttpStatusCode.Unauthorized)
                throw new UnauthorizedException();
            throw await ThrowWebException(response, cancel).ConfigureAwait(false);
        }


        /// <summary>
        /// Позволяет получить краткую информацию о пользователе<br/>
        /// Это его id, имя и аватар.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="token"></param>
        /// <param name="cancel"></param>
        /// <returns></returns>
        /// <exception cref="UnauthorizedException"></exception>
        /// <exception cref="NotFoundException"></exception>
        public async Task<UserInfoDTO> GetUserInfo(int userId, string token, CancellationToken cancel = default)
        {
            var response = await HttpClient.GetAsync($"info/{userId}?token={token}", cancel).ConfigureAwait(false);
            if (response.IsSuccessStatusCode)
                return await response
                 .Content
                     .ReadFromJsonAsync<UserInfoDTO>(cancellationToken: cancel)
                     .ConfigureAwait(false);

            string message = await response.Content.ReadAsStringAsync(cancel).ConfigureAwait(false);
            if (response.StatusCode == HttpStatusCode.NotFound)
                throw new NotFoundException(message);
            if (response.StatusCode == HttpStatusCode.Unauthorized)
                throw new UnauthorizedException();
            throw await ThrowWebException(response, cancel).ConfigureAwait(false);
        }

        /// <summary>
        /// Позволяет получить данные профиля пользователя, такие как:<br/>
        /// Имя, аватар, статус, дата регистрации
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="token"></param>
        /// <param name="cancel"></param>
        /// <returns></returns>
        /// <exception cref="UnauthorizedException"></exception>
        /// <exception cref="NotFoundException"></exception>
        public async Task<ProfileDTO> GetProfileById(int userId, string token, CancellationToken cancel = default)
        {
            var response = await HttpClient.GetAsync($"{userId}?token={token}", cancel).ConfigureAwait(false);
            if (response.IsSuccessStatusCode)
                return await response
                 .Content
                     .ReadFromJsonAsync<ProfileDTO>(cancellationToken: cancel)
                     .ConfigureAwait(false);

            string message = await response.Content.ReadAsStringAsync(cancel).ConfigureAwait(false);
            if (response.StatusCode == HttpStatusCode.NotFound)
                throw new NotFoundException(message);
            if (response.StatusCode == HttpStatusCode.Unauthorized)
                throw new UnauthorizedException();
            throw await ThrowWebException(response, cancel).ConfigureAwait(false);
        }

        /// <summary>
        /// Позволяет установить статус в профиле
        /// </summary>
        /// <param name="status">новый статус</param>
        /// <param name="token">токен доступа владельца страницы</param>
        /// <param name="cancel"></param>
        /// <returns></returns>
        /// <exception cref="UnauthorizedException"></exception>
        /// <exception cref="NotFoundException"></exception>
        public async Task SetStatus(
           string status,
           string token,
           CancellationToken cancel = default)
        {
            var response = await HttpClient.PostAsync($"status?status={status}&token={token}", null, cancel).ConfigureAwait(false);
            if (response.IsSuccessStatusCode)
                return;

            string message = await response.Content.ReadAsStringAsync(cancel).ConfigureAwait(false);
            if (response.StatusCode == HttpStatusCode.NotFound)
                throw new NotFoundException(message);
            if (response.StatusCode == HttpStatusCode.Unauthorized)
                throw new UnauthorizedException();
            throw await ThrowWebException(response, cancel).ConfigureAwait(false);
        }


    }
}