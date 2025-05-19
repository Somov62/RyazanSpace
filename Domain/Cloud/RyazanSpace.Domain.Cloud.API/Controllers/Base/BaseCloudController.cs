using Microsoft.AspNetCore.Mvc;

namespace RyazanSpace.Domain.Cloud.API.Controllers.Base
{
    [Route("api/Cloud/[controller]")]
    [ApiController]
    public abstract class BaseCloudController : ControllerBase { }
}
