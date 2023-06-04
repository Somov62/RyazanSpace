using Microsoft.AspNetCore.Mvc;
using RyazanSpace.Core.Exceptions;
using RyazanSpace.Services.ServerCloud.API.Services;
using System.ComponentModel.DataAnnotations;

namespace RyazanSpace.Services.ServerCloud.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CloudController : ControllerBase
    {
        private readonly CloudService _service = new();

        [HttpGet("{filename}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Produces("application/octet-stream")]
        public async Task<IActionResult> Download([Required] string filename) => 
            File(await _service.Download(filename), "application/octet-stream");

        [HttpPost()]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Upload([Required][FromBody] byte[] file)
        {
            try
            {
                return CreatedAtAction(nameof(Download), new { filename = await _service.Upload(file) }, null);
            }
            catch (NotFoundException ex) { return NotFound(ex.Message); }
        }
    }
}
