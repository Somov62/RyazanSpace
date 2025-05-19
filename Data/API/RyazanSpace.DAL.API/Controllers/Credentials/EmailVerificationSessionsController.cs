using RyazanSpace.DAL.API.Controllers.Base;
using RyazanSpace.DAL.Entities.Credentials;
using RyazanSpace.Interfaces.Repositories;

namespace RyazanSpace.DAL.API.Controllers.Credentials
{
    public class EmailVerificationSessionsController : EntityController<EmailVerificationSession>
    {
        public EmailVerificationSessionsController(IRepository<EmailVerificationSession> repository) : base(repository) { }
    }
}
