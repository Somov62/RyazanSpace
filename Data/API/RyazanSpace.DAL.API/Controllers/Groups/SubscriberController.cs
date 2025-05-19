using Microsoft.AspNetCore.Mvc;
using RyazanSpace.DAL.API.Controllers.Base;
using RyazanSpace.DAL.Entities.Groups;
using RyazanSpace.DAL.Repositories.Groups;

namespace RyazanSpace.DAL.API.Controllers.Groups
{
    public class SubscriberController : BaseEntityController<GroupSubscriber>
    {
        private readonly DbGroupSubscribeRepository _actualRepository;

        public SubscriberController(DbGroupSubscribeRepository repository) : base(repository)
        {
            _actualRepository = repository;
        }

        [HttpGet("find[[{groupId:int}:{userId:int}]]")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int groupId, int userId)
            => await _actualRepository.GetById(groupId, userId) is { } item ? Ok(item) : NotFound();

        [HttpGet("exist[[{groupId:int}:{userId:int}]]")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(bool))]
        public async Task<IActionResult> Exist(int groupId, int userId)
            => await _actualRepository.Exist(groupId, userId) ? Ok(true) : NotFound(false);

        [HttpGet("{groupId:int}/subs")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetGroupSubscribers(int groupId)
            => Ok(await _actualRepository.GetGroupSubscribers(groupId));

        [HttpGet("{userId:int}/groups")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetUserGroups(int userId)
            => Ok(await _actualRepository.GetUserGroups(userId));

        [HttpGet("{groupId:int}/subs/count")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCountGroupSubscribers(int groupId)
            => Ok(await _actualRepository.GetCountGroupSubscribers(groupId));

        [HttpGet("{userId:int}/groups/count")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCountUserGroups(int userId)
            => Ok(await _actualRepository.GetCountUserGroups(userId));
    }
}
