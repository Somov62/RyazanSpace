using RyazanSpace.Core.API;
using RyazanSpace.Core.Exceptions;
using RyazanSpace.Domain.Auth.DTO;
using RyazanSpace.Domain.Auth.Exceptions;
using System.Net;
using System.Net.Http.Json;

namespace RyazanSpace.Domain.Auth.API.Client
{
    public class WebAuthService : WebService
    {
        public WebAuthService(HttpClient client) : base(client) { }

        /// <summary>
        ///  /// <para>Метод для входа в аккаунт.</para>
        /// Создает токен доступа
        /// </summary>
        /// <param name="model">Конфиденциальные данные</param>
        /// <param name="cancel"></param>
        /// <returns></returns>
        /// <exception cref="NotFoundException"></exception>
        /// <exception cref="UserNotVerifiedException"></exception>
        public async Task<TokenResponseDTO> Login(AuthRequestDTO model, CancellationToken cancel = default)
        {
            var response = await HttpClient.PostAsJsonAsync($"login", model, cancel).ConfigureAwait(false);
            if (response.IsSuccessStatusCode)
                return await response
                .Content
                    .ReadFromJsonAsync<TokenResponseDTO>(cancellationToken: cancel)
                    .ConfigureAwait(false);

            string message = await response.Content.ReadAsStringAsync(cancel).ConfigureAwait(false);
            if (response.StatusCode == HttpStatusCode.NotFound)
                throw new NotFoundException(message);
            if (response.StatusCode == HttpStatusCode.Unauthorized)
                throw new UserNotVerifiedException(message);
            throw new WebException(message);
        }

        /// <summary>
        /// Метод для продления токена доступа
        /// </summary>
        /// <param name="token"></param>
        /// <param name="cancel"></param>
        /// <returns></returns>
        /// <exception cref="NotFoundException"></exception>
        /// <exception cref="TimeOutSessionException"></exception>
        public async Task Logout(string token, CancellationToken cancel = default)
        {
            var response = await HttpClient.GetAsync($"logout?token={token}", cancel).ConfigureAwait(false);
            if (response.IsSuccessStatusCode)
                return;

            throw await ThrowWebException(response, cancel).ConfigureAwait(false);
        }

        /// <summary>
        /// <para>Метод для выхода из аккаунта.</para>
        /// Токен доступа удаляется и больше недействителен.
        /// </summary>
        /// <param name="token"></param>
        /// <param name="cancel"></param>
        /// <returns></returns>
        /// <exception cref="NotFoundException"></exception>
        public async Task<TokenResponseDTO> RefreshToken(string token, CancellationToken cancel = default)
        {
            var response = await HttpClient.GetAsync($"refresh?token={token}", cancel).ConfigureAwait(false);
            if (response.IsSuccessStatusCode)
                return await response
                    .Content
                    .ReadFromJsonAsync<TokenResponseDTO>(cancellationToken: cancel)
                    .ConfigureAwait(false);

            throw await ThrowWebException(response, cancel).ConfigureAwait(false);
        }

        /// <summary>
        /// <para>Метод принудительного удаления токена</para>
        /// </summary>
        /// <param name="tokenId">ID токена в базе данных</param>
        /// <param name="cancel"></param>
        /// <returns></returns>
        /// <exception cref="NotFoundException"></exception>
        public async Task BreakToken(string token, CancellationToken cancel = default)
        {
            var response = await HttpClient.GetAsync($"logout?token={token}", cancel).ConfigureAwait(false);
            if (response.IsSuccessStatusCode)
                return;

            throw await ThrowWebException(response, cancel).ConfigureAwait(false);
        }
    }
}
