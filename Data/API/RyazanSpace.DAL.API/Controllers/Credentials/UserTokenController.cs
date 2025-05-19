using Microsoft.AspNetCore.Mvc;
using RyazanSpace.DAL.API.Controllers.Base;
using RyazanSpace.DAL.Entities.Credentials;
using RyazanSpace.DAL.Repositories.Credentials;

namespace RyazanSpace.DAL.API.Controllers.Credentials
{
    public class UserTokenController : EntityController<UserToken>
    {
        private readonly DbUserTokenRepository _actualRepository;

        public UserTokenController(DbUserTokenRepository repository) : base(repository) 
            => _actualRepository = repository;


        [HttpGet("exist/token/{token}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(bool))]
        public async Task<IActionResult> ExistToken(string token) =>
            await _actualRepository.ExistToken(token) ? Ok(true) : NotFound(false);


        [HttpGet("find/token/{token}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByToken(string token) =>
            await _actualRepository.GetByToken(token) is { } item ? Ok(item) : NotFound();

        [HttpDelete("{token}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(string token)
        {
            if (await _actualRepository.DeleteByToken(token) is not { } result)
                return NotFound(token);
            return Ok(result);
        }
    }
}
