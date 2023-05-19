using RyazanSpace.Core.API;
using RyazanSpace.Core.Exceptions;
using RyazanSpace.Domain.Auth.DTO;
using RyazanSpace.Domain.Auth.Exceptions;
using System.Net;
using System.Net.Http.Json;

namespace RyazanSpace.Domain.Auth.API.Client
{
    public class WebResetPasswordService : WebService
    {
        public WebResetPasswordService(HttpClient client) : base(client) { }

        /// <summary>
        /// Метод для принудительного завершения сессии сброса пароля.
        /// </summary>
        /// <param name="sessionId">id cecсии сброса пароля из бд</param>
        /// <returns></returns>
        public async Task BreakSession(int sessionId, CancellationToken cancel)
        {
            var response = await HttpClient.GetAsync($"{sessionId}/break", cancel).ConfigureAwait(false);
            if (response.IsSuccessStatusCode)
                return;

            throw await ThrowWebException(response, cancel).ConfigureAwait(false);
        }

        /// <summary>
        ///     <para>Создает сессию для сброса пароля.</para>
        /// Генерирует случайный пятизначный код подтверждения. <br/>
        /// Отправляет сообщение с кодом на электронную почту пользователя.
        /// </summary>
        /// <param name="model"><see cref="ResetPasswordRequestDTO"/> - модель с логином и почтой пользователя</param>
        /// <exception cref="NotFoundException"/>
        /// <returns>id созданной сессии</returns>
        public async Task<int> CreateSession(ResetPasswordRequestDTO model, CancellationToken cancel)
        {
            var response = await HttpClient.PostAsJsonAsync($"", model, cancel).ConfigureAwait(false);
            if (response.IsSuccessStatusCode)
                return await response
                    .Content
                    .ReadFromJsonAsync<int>(cancellationToken: cancel)
                    .ConfigureAwait(false);

            throw await ThrowWebException(response, cancel).ConfigureAwait(false);
        }

        /// <summary>
        ///     <para>Метод для сброса пароля</para>
        /// Сопоставляет присланный код подтверждения с настоящим
        /// </summary>
        /// <param name="model"><see cref="ConfirmResetPasswordDTO"/>
        /// - модель с кодом сессии, кодом подтверждения и новым паролем</param>
        /// <exception cref="NotFoundException"/>
        /// <exception cref="TimeOutSessionException"/>
        /// <exception cref="ArgumentException"/>
        /// <returns>true - в случае успеха</returns>
        public async Task<bool> ConfirmSession(ConfirmResetPasswordDTO model, CancellationToken cancel)
        {
            var response = await HttpClient.PostAsJsonAsync($"confirm", model, cancel).ConfigureAwait(false);
            if (response.IsSuccessStatusCode)
                return await response
                    .Content
                    .ReadFromJsonAsync<bool>(cancellationToken: cancel)
                    .ConfigureAwait(false);

            string message = await response.Content.ReadAsStringAsync(cancel).ConfigureAwait(false);
            if (response.StatusCode == HttpStatusCode.NotFound)
                throw new NotFoundException(message);
            if (response.StatusCode == HttpStatusCode.RequestTimeout)
                throw new TimeOutSessionException(message);
            throw new WebException(message);
        }
    }
}
