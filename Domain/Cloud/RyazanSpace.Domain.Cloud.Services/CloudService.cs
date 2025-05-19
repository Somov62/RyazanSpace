using RyazanSpace.Core.DTO;
using RyazanSpace.Core.Exceptions;
using RyazanSpace.DAL.Entities.Resources.Base;
using RyazanSpace.Domain.Auth.API.Client;
using RyazanSpace.Domain.Cloud.DTO;
using RyazanSpace.Interfaces.Cloud;
using RyazanSpace.Interfaces.Repositories;

namespace RyazanSpace.Domain.Cloud.Services
{
    public class CloudService
    {
        private readonly ICloud _cloud;
        private readonly IRepository<CloudResource> _repository;
        private readonly WebAuthService _authService;

        public CloudService(ICloud cloud, IRepository<CloudResource> repository, WebAuthService authService)
        {
            _cloud = cloud;
            _repository = repository;
            _authService = authService;
        }


        /// <summary>
        /// Позволяет загрузить файл в облако RyazanSpace
        /// </summary>
        /// <param name="file"></param>
        /// <param name="type"> тип файла</param>
        /// <param name="accessToken"> токен пользователя</param>
        /// <param name="cancel"></param>
        /// <returns><see cref="CloudResourceDTO"/>, null в случае ошибки при загрузке в облако</returns>
        /// <exception cref="UnauthorizedException"></exception>
        public async Task<CloudResourceDTO> UploadFile(
            UploadRequestDTO model, 
            string accessToken, 
            CancellationToken cancel = default) 
        {
            //Аунтетификация
            var clientId = await _authService.TryGetUserByToken(accessToken, cancel).ConfigureAwait(false);
            if (clientId == null) throw new UnauthorizedException();

            var downloadLink = await _cloud.Upload(model.File, cancel).ConfigureAwait(false);
            CloudResource resource = new()
            {
                OwnerId = clientId.Value,
                DownloadLink = downloadLink,
                Type = model.Type
            };
            resource = await _repository.Add(resource, cancel).ConfigureAwait(false);
            return new CloudResourceDTO(resource);
        }

    }
}