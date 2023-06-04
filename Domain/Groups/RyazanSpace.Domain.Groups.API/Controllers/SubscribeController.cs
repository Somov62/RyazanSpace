using Microsoft.AspNetCore.Mvc;
using RyazanSpace.Core.Exceptions;
using RyazanSpace.Domain.Groups.DTO;
using RyazanSpace.Domain.Groups.Services;
using RyazanSpace.Interfaces.Repositories;

namespace RyazanSpace.Domain.Groups.API.Controllers
{
    public class SubscribeController : Base.BaseGroupController
    {
        private readonly SubscribeService _service;

        public SubscribeController(SubscribeService service) => _service = service;


        [HttpGet("subscribe")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Subscribe(int groupId, string token)
        {
            try
            {
                await _service.Subscribe(groupId, token);
                return Ok();
            }
            catch (NotFoundException ex) { return NotFound(ex.Message); }
            catch (UnauthorizedException ex) { return Unauthorized(ex.Message); }
            catch (BadRequestException ex) { return BadRequest(ex.Message); }
        }

        [HttpGet("unsubscribe")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> UnSubscribe(int groupId, string token)
        {
            try
            {
                await _service.UnSubscribe(groupId, token);
                return Ok();
            }
            catch (NotFoundException ex) { return NotFound(ex.Message); }
            catch (UnauthorizedException ex) { return Unauthorized(ex.Message); }
            catch (BadRequestException ex) { return BadRequest(ex.Message); }
        }

        [HttpGet("userssubscribe")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<IPage<GroupDTO>>> GetSubscribedGroups(string token, int userId = 0)
        {
            try
            {
                return Ok(await _service.GetSubscribedGroups(token, userId));
            }
            catch (UnauthorizedException ex) { return Unauthorized(ex.Message); }
        }
    }
}
