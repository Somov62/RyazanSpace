using RyazanSpace.Core.DTO;
using RyazanSpace.Core.Exceptions;
using RyazanSpace.DAL.Client.Repositories.Groups;
using RyazanSpace.DAL.Entities.Groups;
using RyazanSpace.DAL.Entities.Resources.Base;
using RyazanSpace.Domain.Auth.API.Client;
using RyazanSpace.Domain.Groups.DTO;
using RyazanSpace.Interfaces.Repositories;
using System.Reflection;

namespace RyazanSpace.Domain.Groups.Services
{
    public class PostService
    {
        private readonly WebAuthService _authService;
        private readonly WebGroupsRepository _groupRepository;
        private readonly WebPostRepository _postRepository;
        private readonly WebSubscribeRepository _subcribeRepository;

        public PostService(
            WebAuthService authService,
            WebGroupsRepository groupRepository,
            WebPostRepository postRepository,
            WebSubscribeRepository subcribeRepository)
        {
            _authService = authService;
            _groupRepository = groupRepository;
            _postRepository = postRepository;
            _subcribeRepository = subcribeRepository;
        }

        /// <summary>
        /// Создает пост в указанной группе
        /// </summary>
        /// <param name="model"><see cref="CreatePostDTO"/></param>
        /// <param name="token">Токен администратора группа</param>
        /// <param name="cancel"></param>
        /// <returns></returns>
        /// <exception cref="UnauthorizedException"></exception>
        /// <exception cref="NotFoundException"></exception>
        /// <exception cref="NotAccessException"></exception>
        /// <exception cref="BadRequestException"></exception>
        public async Task<PostDTO> CreatePost(CreatePostDTO model, string token, CancellationToken cancel = default)
        {
            var clientId = await _authService.TryGetUserByToken(token, cancel).ConfigureAwait(false);
            if (clientId == null) throw new UnauthorizedException();

            if (model.GroupId < 1) throw new NotFoundException("Группа не найдена");
            var group = await _groupRepository.GetById(model.GroupId, cancel).ConfigureAwait(false);
            if (group == null) throw new NotFoundException("Группа не найдена");

            if (group.OwnerId != clientId) throw new NotAccessException();

            if ((model.Resources == null || model.Resources.Count == 0) &&
                string.IsNullOrWhiteSpace(model.Text))
                throw new BadRequestException("Укажите контент поста");

            Post post = model.MapToEntity();
            post.CreationTime = DateTimeOffset.Now;
            post = await _postRepository.Add(post, cancel).ConfigureAwait(false);
            return new PostDTO(post);
        }

        /// <summary>
        /// Позволяет получить страницу постов по всем группам
        /// </summary>
        /// <param name="pageIndex">Номер страницы</param>
        /// <param name="pageSize">Размер страницы</param>
        /// <param name="token">Токен доступа</param>
        /// <param name="cancel"></param>
        /// <returns></returns>
        /// <exception cref="UnauthorizedException"></exception>
        public async Task<IPage<PostDTO>> GetPage(
            int pageIndex,
            int pageSize,
            string token,
            CancellationToken cancel = default)
        {
            var clientId = await _authService.TryGetUserByToken(token, cancel).ConfigureAwait(false);
            if (clientId == null) throw new UnauthorizedException();

            var page = await _postRepository.GetPage(pageIndex, pageSize, cancel).ConfigureAwait(false);

            var dtos = new List<PostDTO>();
            foreach (var item in page.Items)
            {
                var dto = new PostDTO(item);
                dto.Group.IsSubscibed = await _subcribeRepository.Exist(item.Id, clientId.Value, cancel).ConfigureAwait(false);
                dto.Group.IsOwner = item.Group.OwnerId == clientId.Value;
                dto.Group.SubsCount = await _subcribeRepository.GetCountGroupSubscribers(item.Id, cancel).ConfigureAwait(false);
                dtos.Add(dto);
            }
            var pageDTO = new Page<PostDTO>()
            {
                PageIndex = page.PageIndex,
                PageSize = page.PageSize,
                TotalCount = page.TotalCount,
                Items = dtos
            };
            return pageDTO;
        }


        /// <summary>
        /// Позволяет получить страницу постов определенной группы
        /// </summary>
        /// <param name="groupId">Id группы</param>
        /// <param name="pageIndex">Номер страницы</param>
        /// <param name="pageSize">Размер страницы</param>
        /// <param name="token">Токен доступа</param>
        /// <param name="cancel"></param>
        /// <returns></returns>
        /// <exception cref="UnauthorizedException"></exception>
        /// <exception cref="NotFoundException"></exception>
        public async Task<IPage<PostDTO>> GetByGroupPage(
            int groupId,
            int pageIndex,
            int pageSize,
            string token,
            CancellationToken cancel = default)
        {
            //Проверка токена доступа
            var clientId = await _authService.TryGetUserByToken(token, cancel).ConfigureAwait(false);
            if (clientId == null) throw new UnauthorizedException();

            //Проверка существования группы
            if (groupId < 1) throw new NotFoundException("Группа не найдена");
            var group = await _groupRepository.GetById(groupId, cancel).ConfigureAwait(false);
            if (group == null) throw new NotFoundException("Группа не найдена");

            //Получение постов группы
            var page = await _postRepository.GetByGroupPage(groupId, pageIndex, pageSize, cancel).ConfigureAwait(false);

            //Получение кол-ва подписчиков группы
            int subsCount = await _subcribeRepository.GetCountGroupSubscribers(groupId, cancel).ConfigureAwait(false);
            //Подписан ли пользователь на группу
            bool isSubscribed = await _subcribeRepository.Exist(groupId, clientId.Value, cancel).ConfigureAwait(false);
            //Владеет ли пользователь группой
            bool isOwner = group.Owner.Id == clientId.Value;

            var dtos = new List<PostDTO>();
            foreach (var item in page.Items)
            {
                var dto = new PostDTO(item);
                dto.Group.IsSubscibed = isSubscribed;
                dto.Group.IsOwner = isOwner;
                dto.Group.SubsCount = subsCount;
                dtos.Add(dto);
            }
            var pageDTO = new Page<PostDTO>()
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
