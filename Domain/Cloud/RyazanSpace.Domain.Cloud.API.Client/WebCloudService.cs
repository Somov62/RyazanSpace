using RyazanSpace.Core.API;
using RyazanSpace.Domain.Cloud.DTO;
using System.Net.Http.Json;

namespace RyazanSpace.Domain.Cloud.API.Client
{
    public class WebCloudService : WebService
    {
        public WebCloudService(HttpClient client) : base(client) { }


        /// <summary>
        /// Позволяет загрузить файл в облако RyazanSpace
        /// </summary>
        /// <param name="model"><see cref="UploadRequestDTO"/></param>
        /// <param name="token"></param>
        /// <param name="cancel"></param>
        /// <returns><see cref="CloudResourceDTO"/></returns>
        public async Task<CloudResourceDTO> Upload(
            UploadRequestDTO model, 
            string token, 
            CancellationToken cancel = default)
        {
            var response = await HttpClient.PostAsJsonAsync($"?token={token}", model, cancel).ConfigureAwait(false);
            if (response.IsSuccessStatusCode)
                return await response
                 .Content
                     .ReadFromJsonAsync<CloudResourceDTO>(cancellationToken: cancel)
                     .ConfigureAwait(false);

            throw await ThrowWebException(response, cancel).ConfigureAwait(false);
        }
    }
}