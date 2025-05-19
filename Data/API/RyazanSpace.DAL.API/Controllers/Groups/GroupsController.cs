using Microsoft.AspNetCore.Mvc;
using RyazanSpace.DAL.Entities.Groups;
using RyazanSpace.DAL.Repositories.Groups;

namespace RyazanSpace.DAL.API.Controllers.Groups
{
    public class GroupsController : Base.NamedEntityController<Group>
    {
        private readonly DbGroupRepository _actualRepository;

        public GroupsController(DbGroupRepository repository) : base(repository) => _actualRepository = repository;

        [HttpGet("managed")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetManagedGroups(int userId)
            => Ok(await _actualRepository.GetManagedGroups(userId));
    }
}
