using RyazanSpace.DAL.Entities.Credentials;
using RyazanSpace.DAL.WebApiClients.Repositories.Account;
using RyazanSpace.Interfaces.Repositories;

namespace RyazanSpace.Domain.Auth.Services
{
    public class ResetPasswordService
    {
        private readonly IRepository<ResetPasswordSession> _passwordRepository;
        private readonly WebUserRepository _userRepository;

        public ResetPasswordService(
            IRepository<ResetPasswordSession> passwordRepository,
            WebUserRepository userRepository)
        {
            _passwordRepository = passwordRepository;
            _userRepository = userRepository;
        }



    }
}
