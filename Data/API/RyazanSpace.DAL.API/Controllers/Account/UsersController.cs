using RyazanSpace.DAL.API.Controllers.Base;
using RyazanSpace.DAL.Entities.Account;
using RyazanSpace.Interfaces.Repositories;

namespace RyazanSpace.DAL.API.Controllers.Account
{
    public class UsersController : NamedEntityController<User>
    {
        public UsersController(INamedRepository<User> repository) : base(repository) { }

    }
}
