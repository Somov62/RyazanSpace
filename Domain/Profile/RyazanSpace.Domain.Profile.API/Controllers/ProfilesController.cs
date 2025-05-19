using Microsoft.AspNetCore.Mvc;
using RyazanSpace.Core.Exceptions;
using RyazanSpace.Domain.Profile.DTO;
using RyazanSpace.Domain.Profile.Services;

namespace RyazanSpace.Domain.Profile.API.Controllers
{
    public class ProfilesController : Base.BaseProfilesController
    {
        private readonly ProfileService _service;

        public ProfilesController(ProfileService service) => _service = service;


        [HttpGet("info/{userId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetUserInfo(int userId, string token)
        {
            try
            {
                return Ok(await _service.GetUserInfo(userId, token));
            }
            catch (NotFoundException ex) { return NotFound(ex.Message); }
            catch (UnauthorizedException ex) { return Unauthorized(ex.Message); }
        }

        [HttpGet("{userId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetProfileById(int userId, string token)
        {
            try
            {
                return Ok(await _service.GetProfileById(userId, token));
            }
            catch (NotFoundException ex) { return NotFound(ex.Message); }
            catch (UnauthorizedException ex) { return Unauthorized(ex.Message); }
        }


        [HttpPost("avatar")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> SetAvatar(int imageId, string token)
        {
            try
            {
                return Ok(await _service.SetAvatar(imageId, token));
            }
            catch (NotFoundException ex) { return NotFound(ex.Message); }
            catch (UnauthorizedException ex) { return Unauthorized(ex.Message); }
        }

        [HttpPost("status")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> SetStatus(string status, string token)
        {
            try
            {
                await _service.SetStatus(status, token);
                return Ok();
            }
            catch (NotFoundException ex) { return NotFound(ex.Message); }
            catch (UnauthorizedException ex) { return Unauthorized(ex.Message); }
        }
    }
}
