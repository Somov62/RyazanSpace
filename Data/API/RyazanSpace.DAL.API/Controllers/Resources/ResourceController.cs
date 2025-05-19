using RyazanSpace.DAL.API.Controllers.Base;
using RyazanSpace.DAL.Entities.Resources.Base;
using RyazanSpace.Interfaces.Repositories;

namespace RyazanSpace.DAL.API.Controllers.Resources
{
    public class ResourceController : EntityController<CloudResource>
    {
        public ResourceController(IRepository<CloudResource> repository) : base(repository) { }
    }
}
