using Microsoft.AspNetCore.Mvc;
using RyazanSpace.Core.Exceptions;
using RyazanSpace.Domain.Cloud.DTO;
using RyazanSpace.Domain.Cloud.Services;

namespace RyazanSpace.Domain.Cloud.API.Controllers
{
    public class ResourcesController : Base.BaseCloudController
    {
        private readonly CloudService _service;

        public ResourcesController(CloudService service) => _service = service;


        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> UploadResource([FromBody] UploadRequestDTO model, [FromQuery] string token)
        {
            try
            {
                return Ok(await _service.UploadFile(model, token));
            }
            catch (UnauthorizedException ex) { return Unauthorized(ex.Message); }
        }

    }
}
