using Microsoft.AspNetCore.Mvc;
using RyazanSpace.DAL.Entities.Groups;
using RyazanSpace.DAL.Repositories.Groups;
using RyazanSpace.Interfaces.Repositories;

namespace RyazanSpace.DAL.API.Controllers.Groups
{
    public class PostController : Base.EntityController<Post>
    {
        private readonly DbPostRepository _actualRepository;

        public PostController(DbPostRepository repository) : base(repository) => _actualRepository = repository;

        [HttpGet("group/{groupId:int}/page/{pageIndex:int}/{pageSize:int}")]
        [HttpGet("group/{groupId:int}/page[[{pageIndex:int}:{pageSize:int}]]")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IPage<Post>>> GetPage(int groupId, int pageIndex, int pageSize)
        {
            var result = await _actualRepository.GetByGroupPage(groupId, pageIndex, pageSize);
            return result.Items.Any() ? Ok(result) : NotFound(result);
        }

    }
}
