using RyazanSpace.DAL.Entities.Groups;
using RyazanSpace.DAL.Repositories.Groups;

namespace RyazanSpace.DAL.API.Controllers.Groups
{
    public class GroupsController : Base.NamedEntityController<Group>
    {
        public GroupsController(DbGroupRepository repository) : base(repository) { }
    }
}
