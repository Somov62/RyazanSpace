using RyazanSpace.Core.DTO;
using RyazanSpace.Core.Exceptions;
using RyazanSpace.DAL.Client.Repositories.Groups;
using RyazanSpace.DAL.Entities.Groups;
using RyazanSpace.DAL.Entities.Resources.Base;
using RyazanSpace.Domain.Auth.API.Client;
using RyazanSpace.Domain.Groups.DTO;
using RyazanSpace.Interfaces.Repositories;

namespace RyazanSpace.Domain.Groups.Services
{
    public class GroupService
    {
        private readonly WebAuthService _authService;
        private readonly WebGroupsRepository _groupRepository;
        private readonly IRepository<CloudResource> _resourceRepository;
        private readonly WebSubscribeRepository _subcribeRepository;

        public GroupService(
            WebAuthService authService,
            WebGroupsRepository groupRepository,
            IRepository<CloudResource> resourceRepository,
            WebSubscribeRepository subcribeRepository
            )
        {
            _authService = authService;
            _groupRepository = groupRepository;
            _resourceRepository = resourceRepository;
            _subcribeRepository = subcribeRepository;
        }

        /// <summary>
        /// Позволяет создать группу
        /// </summary>
        /// <param name="model"><see cref="CreateGroupDTO"/></param>
        /// <param name="token">токен доступа создателя группы</param>
        /// <param name="cancel"></param>
        /// <returns>id созданной группы</returns>
        /// <exception cref="UnauthorizedException"></exception>
        public async Task<int> CreateGroup(CreateGroupDTO model, string token, CancellationToken cancel = default)
        {
            var clientId = await _authService.TryGetUserByToken(token, cancel).ConfigureAwait(false);
            if (clientId == null) throw new UnauthorizedException();

            Group newGroup = model.MapToEntity();

            newGroup.RegDate = DateTimeOffset.Now;
            newGroup.OwnerId = clientId;

            newGroup = await _groupRepository.Add(newGroup, cancel).ConfigureAwait(false);
            return newGroup.Id;
        }

        /// <summary>
        /// Позволяет установить аватар на группу
        /// </summary>
        /// <param name="groupId">id группы</param>
        /// <param name="resourceId">id фотографии</param>
        /// <param name="token">токен доступа владельца группы</param>
        /// <param name="cancel"></param>
        /// <returns></returns>
        /// <exception cref="UnauthorizedException"></exception>
        /// <exception cref="NotFoundException"></exception>
        /// <exception cref="NotAccessException"></exception>
        public async Task<CloudResourceDTO> SetLogo(int groupId, int resourceId, string token, CancellationToken cancel = default)
        {
            var clientId = await _authService.TryGetUserByToken(token, cancel).ConfigureAwait(false);
            if (clientId == null) throw new UnauthorizedException();

            if (groupId < 1) throw new NotFoundException("Группа не найдена");
            var group = await _groupRepository.GetById(groupId, cancel).ConfigureAwait(false);
            if (group == null) throw new NotFoundException("Группа не найдена");

            if (group.OwnerId != clientId) throw new NotAccessException();

            if (resourceId < 1) throw new NotFoundException("Фото не найдено");
            var resource = await _resourceRepository.GetById(resourceId, cancel).ConfigureAwait(false);
            if (resource == null || resource.Type != CloudResourceType.Image)
                throw new NotFoundException("Фото не найдено. Убедитесь в верном формате изображения");

            group.Logo = resource;
            await _groupRepository.Update(group, cancel).ConfigureAwait(false);
            return new CloudResourceDTO(resource);
        }

        /// <summary>
        /// Позволяет получить данные группы, такие как:<br/>
        /// Имя, аватар, описание, дата регистрации
        /// </summary>
        /// <param name="groupId">id группы</param>
        /// <param name="token"></param>
        /// <param name="cancel"></param>
        /// <returns></returns>
        /// <exception cref="UnauthorizedException"></exception>
        /// <exception cref="NotFoundException"></exception>
        public async Task<GroupDTO> GetGroupById(int groupId, string token, CancellationToken cancel = default)
        {
            var clientId = await _authService.TryGetUserByToken(token, cancel).ConfigureAwait(false);
            if (clientId == null) throw new UnauthorizedException();

            if (groupId < 1) throw new NotFoundException("Группа не найдена");
            var user = await _groupRepository.GetById(groupId, cancel).ConfigureAwait(false);
            if (user == null) throw new NotFoundException("Группа не найдена");

            return new GroupDTO(user);
        }


        /// <summary>
        /// Позволяет установить описание в группе
        /// </summary>
        /// <param name="description">новое описание</param>
        /// <param name="groupId">id группы</param>
        /// <param name="token">токен доступа владельца группы</param>
        /// <param name="cancel"></param>
        /// <returns></returns>
        /// <exception cref="UnauthorizedException"></exception>
        /// <exception cref="NotFoundException"></exception>
        /// <exception cref="NotAccessException"></exception>
        public async Task SetDescription(string description, int groupId, string token, CancellationToken cancel = default)
        {
            var clientId = await _authService.TryGetUserByToken(token, cancel).ConfigureAwait(false);
            if (clientId == null) throw new UnauthorizedException();

            if (groupId < 1) throw new NotFoundException("Группа не найдена");
            var group = await _groupRepository.GetById(groupId, cancel).ConfigureAwait(false);
            if (group == null) throw new NotFoundException("Группа не найдена");

            if (group.OwnerId != clientId) throw new NotAccessException();

            group.Description = description;
            await _groupRepository.Update(group, cancel).ConfigureAwait(false);
        }


        /// <summary>
        /// Позволяет получить список групп постранично
        /// </summary>
        /// <param name="pageIndex">Номер страницы</param>
        /// <param name="pageSize">Размер страницы</param>
        /// <param name="token">Токен доступа</param>
        /// <param name="cancel"></param>
        /// <returns></returns>
        /// <exception cref="UnauthorizedException"></exception>
        public async Task<IPage<GroupDTO>> GetPage(
            int pageIndex,
            int pageSize,
            string token,
            CancellationToken cancel = default)
        {
            var clientId = await _authService.TryGetUserByToken(token, cancel).ConfigureAwait(false);
            if (clientId == null) throw new UnauthorizedException();

            var page = await _groupRepository.GetPage(pageIndex, pageSize, cancel).ConfigureAwait(false);

            var dtos = new List<GroupDTO>();
            foreach (var item in page.Items)
            {
                var dto = new GroupDTO(item);
                dto.IsSubscibed = await _subcribeRepository.Exist(item.Id, clientId.Value, cancel).ConfigureAwait(false);
                dto.SubsCount = await _subcribeRepository.GetCountGroupSubscribers(item.Id, cancel).ConfigureAwait(false);
                dtos.Add(dto);
            }
            var pageDTO = new Page<GroupDTO>()
            {
                PageIndex = page.PageIndex,
                PageSize = page.PageSize,
                TotalCount = page.TotalCount,
                Items = dtos
            };


            return pageDTO;
        }
    }
}