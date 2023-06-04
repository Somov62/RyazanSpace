using RyazanSpace.Core.API;
using RyazanSpace.Core.Exceptions;
using RyazanSpace.Domain.Groups.DTO;
using System.Net;
using System.Net.Http.Json;

namespace RyazanSpace.Domain.Groups.API.Client
{
    public class WebSubscribeService : WebService
    {
        public WebSubscribeService(HttpClient client) : base(client) { }

        /// <summary>
        /// Функция подписки пользователя на группу
        /// </summary>
        /// <param name="groupId">Id группы</param>
        /// <param name="token">токен доступа пользователя, подписывающегося на группу</param>
        /// <param name="cancel"></param>
        /// <returns></returns>
        /// <exception cref="UnauthorizedException"></exception>
        /// <exception cref="NotFoundException"></exception>n>
        public async Task Subscribe(int groupId, string token, CancellationToken cancel = default)
        {
            var response = await HttpClient.GetAsync($"subscribe?groupId={groupId}&token={token}", cancel).ConfigureAwait(false);
            if (response.IsSuccessStatusCode)
                return;

            if (response.StatusCode == HttpStatusCode.Unauthorized)
                throw new UnauthorizedException();

            string message = await response.Content.ReadAsStringAsync(cancel).ConfigureAwait(false);
            if (response.StatusCode == HttpStatusCode.NotFound)
                throw new NotFoundException(message);

            throw await ThrowWebException(response, cancel).ConfigureAwait(false);
        }


        /// <summary>
        /// Функция отписки пользователя от группы
        /// </summary>
        /// <param name="groupId">Id группы</param>
        /// <param name="token">токен доступа пользователя, которого нужно отписать</param>
        /// <param name="cancel"></param>
        /// <returns></returns>
        /// <exception cref="UnauthorizedException"></exception>
        /// <exception cref="NotFoundException"></exception>
        /// <exception cref="BadRequestException"></exception>
        public async Task Unsubscribe(int groupId, string token, CancellationToken cancel = default)
        {
            var response = await HttpClient.GetAsync($"unsubscribe?groupId={groupId}&token={token}", cancel).ConfigureAwait(false);
            if (response.IsSuccessStatusCode)
                return;

            if (response.StatusCode == HttpStatusCode.Unauthorized)
                throw new UnauthorizedException();

            string message = await response.Content.ReadAsStringAsync(cancel).ConfigureAwait(false);
            if (response.StatusCode == HttpStatusCode.NotFound)
                throw new NotFoundException(message);

            throw await ThrowWebException(response, cancel).ConfigureAwait(false);
        }

        /// <summary>
        /// Возвращает список групп, на которые пользователь подписан
        /// </summary>
        /// <param name="token">токен пользователя</param>
        /// <param name="userId">при значении 0 вернет список подписок владельца токена</param>
        /// <param name="cancel"></param>
        /// <returns></returns>
        /// <exception cref="UnauthorizedException"></exception>
        public async Task<List<GroupDTO>> GetSubscribedGroups(string token, int userId = 0, CancellationToken cancel = default)
        {
            var response = await HttpClient.GetAsync($"userssubscribe?token={token}&userId={userId}", cancel).ConfigureAwait(false);
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
