using Microsoft.AspNetCore.Mvc;
using RyazanSpace.DAL.Entities.Base;
using RyazanSpace.Interfaces.Repositories;

namespace RyazanSpace.DAL.API.Controllers.Base
{
    public class NamedEntityController<T> : EntityController<T> where T : NamedEntity
    {
        protected INamedRepository<T> _namedRepository => _repository as INamedRepository<T>;
        public NamedEntityController(INamedRepository<T> repository) : base(repository) { }


        [HttpGet("exist/name/{name}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(bool))]
        public async Task<IActionResult> ExistName(string name) =>
            await _namedRepository.ExistName(name) ? Ok(true) : NotFound(false);


        [HttpGet("find/name/{name}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByName(string name) => 
            await _namedRepository.GetByName(name) is { } item ? Ok(item) : NotFound();

        [HttpDelete("{name}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(string name)
        {
            if (await _namedRepository.DeleteByName(name) is not { } result)
                return NotFound(name);
            return Ok(result);
        }
    }
}
