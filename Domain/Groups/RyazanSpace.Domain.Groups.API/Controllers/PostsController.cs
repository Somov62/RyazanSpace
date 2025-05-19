using Microsoft.AspNetCore.Mvc;
using RyazanSpace.Core.Exceptions;
using RyazanSpace.Domain.Groups.DTO;
using RyazanSpace.Domain.Groups.Services;
using RyazanSpace.Interfaces.Repositories;

namespace RyazanSpace.Domain.Groups.API.Controllers
{
    public class PostsController : Base.BaseGroupController
    {
        private readonly PostService _service;

        public PostsController(PostService service) => _service = service;

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> CreatePost(CreatePostDTO model, string token)
        {
            try
            {
                return Ok(await _service.CreatePost(model, token));
            }
            catch (UnauthorizedException ex) { return Unauthorized(ex.Message); }
        }

        [HttpGet("page/{pageIndex:int}/{pageSize:int}")]
        [HttpGet("page[[{pageIndex:int}:{pageSize:int}]]")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<IPage<GroupDTO>>> GetPage(int pageIndex, int pageSize, string token)
        {
            try
            {
                var result = await _service.GetPage(pageIndex, pageSize, token);
                return result.Items.Any() ? Ok(result) : NotFound(result);
            }
            catch (NotFoundException ex) { return NotFound(ex.Message); }
            catch (UnauthorizedException ex) { return Unauthorized(ex.Message); }
        }

        [HttpGet("group/{groupId:int}/page/{pageIndex:int}/{pageSize:int}")]
        [HttpGet("group/{groupId:int}/page[[{pageIndex:int}:{pageSize:int}]]")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<IPage<GroupDTO>>> GetByGroupPage(int groupId, int pageIndex, int pageSize, string token)
        {
            try
            {
                var result = await _service.GetByGroupPage(groupId, pageIndex, pageSize, token);
                return result.Items.Any() ? Ok(result) : NotFound(result);
            }
            catch (NotFoundException ex) { return NotFound(ex.Message); }
            catch (UnauthorizedException ex) { return Unauthorized(ex.Message); }
        }
    }
}
