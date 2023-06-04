using Microsoft.AspNetCore.Mvc;
using RyazanSpace.Core.Exceptions;
using RyazanSpace.Domain.Groups.DTO;
using RyazanSpace.Domain.Groups.Services;
using RyazanSpace.Interfaces.Repositories;

namespace RyazanSpace.Domain.Groups.API.Controllers
{
    public class GroupsController : Base.BaseGroupController
    {
        private readonly GroupService _service;

        public GroupsController(GroupService service) => _service = service;

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> CreateGroup(CreateGroupDTO model, string token)
        {
            try
            {
                return Ok(await _service.CreateGroup(model, token));
            }
            catch (UnauthorizedException ex) { return Unauthorized(ex.Message); }
        }

        [HttpGet("{groupId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetGroupById(int groupId, string token)
        {
            try
            {
                return Ok(await _service.GetGroupById(groupId, token));
            }
            catch (NotFoundException ex) { return NotFound(ex.Message); }
            catch (UnauthorizedException ex) { return Unauthorized(ex.Message); }
        }


        [HttpPost("{groupId:int}/logo")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> SetLogo(int groupId, int imageId, string token)
        {
            try
            {
                return Ok(await _service.SetLogo(groupId, imageId, token));
            }
            catch (NotFoundException ex) { return NotFound(ex.Message); }
            catch (UnauthorizedException ex) { return Unauthorized(ex.Message); }
            catch (NotAccessException ex) { return Forbid(ex.Message); }
        }

        [HttpPost("{groupId:int}/description")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> SetDescription(int groupId, string description, string token)
        {
            try
            {
                await _service.SetDescription(description, groupId, token);
                return Ok();
            }
            catch (NotFoundException ex) { return NotFound(ex.Message); }
            catch (UnauthorizedException ex) { return Unauthorized(ex.Message); }
            catch (NotAccessException ex) { return Forbid(ex.Message); }
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

        [HttpGet("managed")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<IPage<GroupDTO>>> GetManagedGroups(string token, int userId = 0)
        {
            try
            {
                return Ok(await _service.GetManagedGroups(token, userId));
            }
            catch (UnauthorizedException ex) { return Unauthorized(ex.Message); }
        }
    }
}
