using RyazanSpace.DAL.API.Controllers.Base;
using RyazanSpace.DAL.Entities.Credentials;
using RyazanSpace.Interfaces.Repositories;

namespace RyazanSpace.DAL.API.Controllers.Credentials
{
    public class ResetPasswordSessionsController : EntityController<ResetPasswordSession>
    {
        public ResetPasswordSessionsController(IRepository<ResetPasswordSession> repository) : base(repository) { }
    }
}
