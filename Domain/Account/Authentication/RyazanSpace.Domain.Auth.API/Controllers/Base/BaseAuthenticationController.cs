using Microsoft.AspNetCore.Mvc;

namespace RyazanSpace.Domain.Auth.API.Controllers.Base
{
    [Route("api/Authentication/[controller]")]
    [ApiController]
    public abstract class BaseAuthenticationController : ControllerBase { }
}
