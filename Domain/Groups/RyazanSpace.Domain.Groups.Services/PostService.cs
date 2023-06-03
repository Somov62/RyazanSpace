using RyazanSpace.DAL.Client.Repositories.Groups;
using RyazanSpace.DAL.Entities.Resources.Base;
using RyazanSpace.Domain.Auth.API.Client;
using RyazanSpace.Interfaces.Repositories;

namespace RyazanSpace.Domain.Groups.Services
{
    public class PostService
    {
        private readonly WebAuthService _authService;
        private readonly WebGroupsRepository _groupRepository;
        private readonly WebPostRepository _postRepository;

        public PostService(
            WebAuthService authService,
            WebGroupsRepository groupRepository,
            WebPostRepository postRepository)
        {
            _authService = authService;
            _groupRepository = groupRepository;
            _postRepository = postRepository;
        }

        
    }
}
