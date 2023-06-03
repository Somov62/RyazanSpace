using RyazanSpace.Core.Exceptions;
using RyazanSpace.DAL.Client.Repositories.Groups;
using RyazanSpace.DAL.Entities.Groups;
using RyazanSpace.Domain.Auth.API.Client;

namespace RyazanSpace.Domain.Groups.Services
{
    public class SubscribeService
    {

        private readonly WebAuthService _authService;
        private readonly WebGroupsRepository _groupRepository;
        private readonly WebSubscribeRepository _subcribeRepository;

        public SubscribeService(
            WebAuthService authService,
            WebGroupsRepository groupRepository,
            WebSubscribeRepository subscribeRepository)
        {
            _authService = authService;
            _groupRepository = groupRepository;
            _subcribeRepository = subscribeRepository;
        }

        /// <summary>
        /// Функция подписки пользователя на группу
        /// </summary>
        /// <param name="groupId">Id группы</param>
        /// <param name="token">токен доступа пользователя, подписывающегося на группу</param>
        /// <param name="cancel"></param>
        /// <returns></returns>
        /// <exception cref="UnauthorizedException"></exception>
        /// <exception cref="NotFoundException"></exception>
        /// <exception cref="BadRequestException"></exception>
        public async Task Subscribe(int groupId, string token, CancellationToken cancel = default)
        {
            var clientId = await _authService.TryGetUserByToken(token, cancel).ConfigureAwait(false);
            if (clientId == null) throw new UnauthorizedException();

            if (groupId < 1) throw new NotFoundException("Группа не найдена");
            var group = await _groupRepository.GetById(groupId, cancel).ConfigureAwait(false);
            if (group == null) throw new NotFoundException("Группа не найдена");

            if (await _subcribeRepository.Exist(groupId, clientId.Value, cancel).ConfigureAwait(false))
                throw new BadRequestException("Пользователь уже подписан на группу");

            var entity = new GroupSubscriber() { GroupId = groupId, UserId = clientId.Value };
            await _subcribeRepository.Add(entity, cancel).ConfigureAwait(false);
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
        public async Task UnSubscribe(int groupId, string token, CancellationToken cancel = default)
        {
            var clientId = await _authService.TryGetUserByToken(token, cancel).ConfigureAwait(false);
            if (clientId == null) throw new UnauthorizedException();

            if (groupId < 1) 
                throw new NotFoundException("Группа не найдена");
            var group = await _groupRepository.GetById(groupId, cancel).ConfigureAwait(false);
            if (group == null) 
                throw new NotFoundException("Группа не найдена");

            var entity = await _subcribeRepository.GetById(groupId, clientId.Value, cancel).ConfigureAwait(false);
            if (entity == null)
                throw new BadRequestException("Пользователь не подписан на группу");

            await _subcribeRepository.Delete(entity, cancel).ConfigureAwait(false);
        }
    }
}
