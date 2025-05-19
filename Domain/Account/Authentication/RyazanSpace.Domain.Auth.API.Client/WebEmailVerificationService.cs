using RyazanSpace.Core.API;
using RyazanSpace.Core.Exceptions;
using RyazanSpace.Domain.Auth.DTO;
using RyazanSpace.Domain.Auth.Exceptions;
using System.Net;
using System.Net.Http.Json;

namespace RyazanSpace.Domain.Auth.API.Client
{
    public class WebEmailVerificationService : WebService
    {
        public WebEmailVerificationService(HttpClient client) : base(client) { }

        /// <summary>
        ///     <para>Проверяет статус почты пользователя.</para>
        /// </summary>
        /// <param name="userId">id пользователя</param>
        /// <exception cref="NotFoundException"/>
        /// <returns>true - почта подтверждена, в остальных случаях - false</returns>
        public async Task<bool> CheckEmailVerified(int userId, CancellationToken cancel = default)
        {
            var response = await HttpClient.GetAsync($"check?userId={userId}", cancel).ConfigureAwait(false);
            if (response.IsSuccessStatusCode)
                return  await response
                    .Content
                    .ReadFromJsonAsync<bool>(cancellationToken : cancel)
                    .ConfigureAwait(false);

            throw await ThrowWebException(response, cancel).ConfigureAwait(false);
        }

        /// <summary>
        ///     <para>Создает сессию для подтверждения почты.</para>
        /// Генерирует случайный пятизначный код подтверждения. <br/>
        /// Отправляет сообщение с кодом на электронную почту пользователя.
        /// </summary>
        /// <param name="userId">id пользователя</param>
        /// <exception cref="NotFoundException"/>
        /// <returns>id созданной сессии</returns>
        public async Task<int> CreateSession(int userId, CancellationToken cancel = default)
        {
            var response = await HttpClient.PostAsync($"?userId={userId}", null, cancel).ConfigureAwait(false);
            if (response.IsSuccessStatusCode)
                return await response
                    .Content
                    .ReadFromJsonAsync<int>(cancellationToken: cancel)
                    .ConfigureAwait(false);

            throw await ThrowWebException(response, cancel).ConfigureAwait(false);
        }

        /// <summary>
        ///     <para>Метод для подтверждения почты</para>
        /// Сопоставляет присланный код подтверждения с настоящим
        /// </summary>
        /// <param name="model"> Модель с кодом сессии и кодом подтверждения</param>
        /// <exception cref="NotFoundException"/>
        /// <exception cref="TimeOutSessionException"/>
        /// <exception cref="ArgumentException"/>
        /// <returns>true - в случае успеха</returns>
        public async Task<bool> ConfirmSession(EmailVerificationRequestDTO model, CancellationToken cancel = default)
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
