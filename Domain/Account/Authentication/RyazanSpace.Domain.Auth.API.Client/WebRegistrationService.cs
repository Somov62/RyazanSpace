using RyazanSpace.Core.API;
using RyazanSpace.Domain.Auth.DTO;
using RyazanSpace.Domain.Auth.Exceptions;
using System.Net;
using System.Net.Http.Json;

namespace RyazanSpace.Domain.Auth.API.Client
{
    public class WebRegistrationService : WebService
    {
        public WebRegistrationService(HttpClient client) : base(client) { }

        /// <summary>
        /// Регистрация пользователя в системе
        /// </summary>
        /// <param name="model"><see cref="RegRequestDTO"/> - модель первичных данных</param>
        /// <returns><see cref="RegResponseDTO"/> - модель пользоваетеля</returns>
        /// <exception cref="UserAlreadyExistsException"></exception>
        /// <exception cref="WebException"></exception>
        public async Task<RegResponseDTO> Register(RegRequestDTO model, CancellationToken cancel = default)
        {
            var response = await HttpClient.PostAsJsonAsync("register", model, cancel).ConfigureAwait(false);
            if (response.IsSuccessStatusCode)
                return await response
                    .Content
                    .ReadFromJsonAsync<RegResponseDTO>(cancellationToken: cancel)
                    .ConfigureAwait(false);

            string message = await response.Content.ReadAsStringAsync(cancel).ConfigureAwait(false);
            if (response.StatusCode == HttpStatusCode.Conflict)
                throw new UserAlreadyExistsException(message);
            throw new WebException(message);

        }
    }
}
