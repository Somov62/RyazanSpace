using Microsoft.AspNetCore.Mvc;
using RyazanSpace.Domain.Auth.DTO;
using RyazanSpace.Domain.Auth.Exceptions;
using RyazanSpace.Domain.Auth.Services;
using System.ComponentModel.DataAnnotations;

namespace RyazanSpace.Domain.Auth.API.Controllers
{
    public class EmailVerificationController : Base.BaseAuthenticationController
    {
        private readonly EmailVerificationService _service;

        public EmailVerificationController(EmailVerificationService service) => _service = service;

        [HttpGet("check")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> CheckEmailVerified([Required] int userId)
        {
            try
            {
                return Ok(await _service.CheckEmailVerified(userId));
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost()]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> CreateSession([Required][FromQuery] int userId)
        {
            try
            {
                return Ok(await _service.CreateSession(userId));
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex) 
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("confirm")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status408RequestTimeout)]
        public async Task<IActionResult> ConfirmSession([Required]EmailVerificationRequestDTO model)
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
