using Microsoft.AspNetCore.Mvc;
using RyazanSpace.Domain.Auth.DTO;
using RyazanSpace.Domain.Auth.Exceptions;
using RyazanSpace.Domain.Auth.Services;

namespace RyazanSpace.Domain.Auth.API.Controllers
{
    public class AuthController : Base.BaseAuthenticationController
    {
        private readonly AuthService _service;

        public AuthController(AuthService service) => _service = service;


        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Login(AuthRequestDTO model)
        {
            try
            {
                return Ok(await _service.Login(
                    model,
                    $"{Request.Scheme}://{Request.Host}{Request.PathBase}{HttpContext.Request.Path.Value.Replace("login", "break?tokenId=ID")}",
                    HttpContext.Connection.RemoteIpAddress.ToString()));
            }
            catch (NotFoundException ex) { return NotFound(ex.Message); }
            catch (UserNotVerifiedException ex) { return Unauthorized(ex.Message); }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        [HttpGet("logout")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Logout(string token)
        {
            try
            {
                await _service.Logout(token);
                return Ok();
            }
            catch (NotFoundException ex) { return NotFound(ex.Message); }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        [HttpGet("refresh")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> RefreshToken(string token)
        {
            try
            {
                return Ok(await _service.RefreshToken(token));
            }
            catch (NotFoundException ex) { return NotFound(ex.Message); }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        [HttpGet("break")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> BreakToken(int tokenId)
        {
            try
            {
                await _service.BreakToken(tokenId);
                return Ok("Успешно!");
            }
            catch (NotFoundException ex) { return NotFound(ex.Message); }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }
    }
}
