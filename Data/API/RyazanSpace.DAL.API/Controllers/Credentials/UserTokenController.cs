using RyazanSpace.DAL.API.Controllers.Base;
using RyazanSpace.DAL.Entities.Credentials;
using RyazanSpace.Interfaces.Repositories;

namespace RyazanSpace.DAL.API.Controllers.Credentials
{
    public class UserTokenController : EntityController<UserToken>
    {
        public UserTokenController(IRepository<UserToken> repository) : base(repository) { }
    }
}
