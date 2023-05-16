using RyazanSpace.DAL.API.Controllers.Base;
using RyazanSpace.DAL.Entities.Resources;
using RyazanSpace.Interfaces.Repositories;

namespace RyazanSpace.DAL.API.Controllers.Resources
{
    public class ImagesController : EntityController<Image>
    {
        public ImagesController(IRepository<Image> repository) : base(repository) { }
    }
}
