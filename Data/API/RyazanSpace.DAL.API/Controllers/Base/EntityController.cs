using Microsoft.AspNetCore.Mvc;
using RyazanSpace.DAL.Entities.Base;
using RyazanSpace.Interfaces.Repositories;

namespace RyazanSpace.DAL.API.Controllers.Base
{
    public abstract class EntityController<T> : BaseEntityController<T> where T : Entity
    {
        protected readonly IRepository<T> _repository;

        protected EntityController(IRepository<T> repository) : base (repository) => _repository = repository;

        

        [HttpGet("exist/id/{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(bool))]
        public async Task<IActionResult> ExistId(int id) =>
            await _repository.ExistId(id) ? Ok(true) : NotFound(false);

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id) => await _repository.GetById(id) is { } item ? Ok(item) : NotFound();

       
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            if (await _repository.DeleteById(id) is not { } result)
                return NotFound(id);
            return Ok(result);
        }
    }
}
