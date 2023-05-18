using Microsoft.AspNetCore.Mvc;
using RyazanSpace.Domain.Auth.DTO;
using RyazanSpace.Domain.Auth.Exceptions;
using RyazanSpace.Domain.Auth.Services;
using System.ComponentModel.DataAnnotations;

namespace RyazanSpace.Domain.Auth.API.Controllers
{
    public class ResetPasswordController : Base.BaseAuthenticationController
    {
        private readonly ResetPasswordService _service;
        public ResetPasswordController(ResetPasswordService service) => _service = service;

        [HttpGet("{sessionId}/break")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> BreakSession([Required] int sessionId)
        {
            try
            {
                await _service.BreakSession(sessionId);
                return Ok();
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost()]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> CreateSession(
            [Required][FromBody] 
            ResetPasswordRequestDTO model)
        {
            try
            {
                return Ok(await _service.CreateSession(model));
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost("confirm")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status408RequestTimeout)]
        public async Task<IActionResult> ConfirmSession(
            [Required][FromBody] 
            ConfirmResetPasswordDTO model)
        {
            try
            {
                return Ok(await _service.ConfirmSession(model));
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (TimeOutSessionException ex)
            {
                return StatusCode(StatusCodes.Status408RequestTimeout, ex.Message);
            }
        }
    }
}
