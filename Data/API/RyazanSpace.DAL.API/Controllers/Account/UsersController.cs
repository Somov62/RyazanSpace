using Microsoft.AspNetCore.Mvc;
using RyazanSpace.DAL.API.Controllers.Base;
using RyazanSpace.DAL.Entities.Account;
using RyazanSpace.DAL.Repositories.Account;

namespace RyazanSpace.DAL.API.Controllers.Account
{
    public class UsersController : NamedEntityController<User>
    {
        private readonly DbUserRepository _actualRepository;

        public UsersController(DbUserRepository repository) : base(repository) =>
            _actualRepository = repository;

        [HttpGet("exist/email/{email}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(bool))]
        public async Task<IActionResult> ExistEmail(string email) =>
            await _actualRepository.ExistEmail(email) ? Ok(true) : NotFound(false);
    }
}
