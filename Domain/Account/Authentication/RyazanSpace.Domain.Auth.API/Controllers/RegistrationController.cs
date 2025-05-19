using Microsoft.AspNetCore.Mvc;
using RyazanSpace.Domain.Auth.DTO;
using RyazanSpace.Domain.Auth.Exceptions;
using RyazanSpace.Domain.Auth.Services;

namespace RyazanSpace.Domain.Auth.API.Controllers
{
    public class RegistrationController : Base.BaseAuthenticationController
    {
        private readonly RegistrationService _service;

        public RegistrationController(RegistrationService service) => _service = service;


        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RegResponseDTO))]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Register([FromBody] RegRequestDTO model)
        {
            try
            {
                var user = await _service.Register(model);
                return Ok(user);
            }
            catch (UserAlreadyExistsException ex) { return Conflict(ex.Message); }
        }
    }
}
