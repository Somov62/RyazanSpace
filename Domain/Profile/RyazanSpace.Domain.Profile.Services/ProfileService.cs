using RyazanSpace.Core.DTO;
using RyazanSpace.Core.Exceptions;
using RyazanSpace.DAL.Client.Repositories.Account;
using RyazanSpace.DAL.Entities.Resources.Base;
using RyazanSpace.Domain.Auth.API.Client;
using RyazanSpace.Domain.Profile.DTO;
using RyazanSpace.Interfaces.Repositories;

namespace RyazanSpace.Domain.Profile.Services
{
    public class ProfileService
    {
        private readonly WebUserRepository _userRepository;
        private readonly WebAuthService _authService;
        private readonly IRepository<CloudResource> _resourceRepository;

        public ProfileService(
            WebUserRepository userRepository,
            WebAuthService authService,
            IRepository<CloudResource> resourceRepository
            )
        {
            _userRepository = userRepository;
            _authService = authService;
            _resourceRepository = resourceRepository;
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
            var clientId = await _authService.TryGetUserByToken(token, cancel).ConfigureAwait(false);
            if (clientId == null) throw new UnauthorizedException();

            if (userId < 1) throw new NotFoundException("Пользователь не найден");
            var user = await _userRepository.GetById(userId).ConfigureAwait(false);
            if (user == null) throw new NotFoundException("Пользователь не найден");

            return new UserInfoDTO(user);
        }

        /// <summary>
        /// Позволяет установить аватар на профиль пользователя
        /// </summary>
        /// <param name="resourceId">id фотографии</param>
        /// <param name="token">токен доступа владельца страницы</param>
        /// <param name="cancel"></param>
        /// <returns></returns>
        /// <exception cref="UnauthorizedException"></exception>
        /// <exception cref="NotFoundException"></exception>
        public async Task<CloudResourceDTO> SetAvatar(int resourceId, string token, CancellationToken cancel = default)
        {
            var clientId = await _authService.TryGetUserByToken(token, cancel).ConfigureAwait(false);
            if (clientId == null) throw new UnauthorizedException();

            var user = await _userRepository.GetById(clientId.Value).ConfigureAwait(false);
            if (user == null) throw new NotFoundException("Пользователь не найден");

            if (resourceId < 1) throw new NotFoundException("Фото не найдено");
            var resource = await _resourceRepository.GetById(resourceId).ConfigureAwait(false);
            if (resource == null || resource.Type != CloudResourceType.Image) 
                throw new NotFoundException("Фото не найдено. Убедитесь в верном формате изображения");

            user.Avatar = resource;
            await _userRepository.Update(user).ConfigureAwait(false);
            return new CloudResourceDTO(resource);
        }

        /// <summary>
        /// Позволяет получить данные профиля пользователя, такие как:<br/>
        /// Имя, аватар, статус, дата регистрации
        /// </summary>
        /// <param name="userId">id пользователя</param>
        /// <param name="token"></param>
        /// <param name="cancel"></param>
        /// <returns></returns>
        /// <exception cref="UnauthorizedException"></exception>
        /// <exception cref="NotFoundException"></exception>
        public async Task<ProfileDTO> GetProfileById(int userId, string token, CancellationToken cancel = default)
        {
            var clientId = await _authService.TryGetUserByToken(token, cancel).ConfigureAwait(false);
            if (clientId == null) throw new UnauthorizedException();

            if (userId < 1) throw new NotFoundException("Пользователь не найден");
            var user = await _userRepository.GetById(userId, cancel).ConfigureAwait(false);
            if (user == null) throw new NotFoundException("Пользователь не найден");

            return new ProfileDTO(user);
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
        public async Task SetStatus(string status, string token, CancellationToken cancel = default)
        {
            var clientId = await _authService.TryGetUserByToken(token, cancel).ConfigureAwait(false);
            if (clientId == null) throw new UnauthorizedException();

            var user = await _userRepository.GetById(clientId.Value, cancel).ConfigureAwait(false);
            if (user == null) throw new NotFoundException("Пользователь не найден");

            user.Status = status;
            await _userRepository.Update(user, cancel).ConfigureAwait(false);
        }
    }
}
