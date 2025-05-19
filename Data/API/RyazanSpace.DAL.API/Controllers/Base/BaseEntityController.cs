using Microsoft.AspNetCore.Mvc;
using RyazanSpace.DAL.Entities.Base;
using RyazanSpace.Interfaces.Repositories;

namespace RyazanSpace.DAL.API.Controllers.Base
{
    [Route("api/Database/[controller]")]
    [ApiController]
    public abstract class BaseEntityController<T> : ControllerBase where T : BaseEntity
    {
        protected readonly IBaseRepository<T> _baseRepository;

        protected BaseEntityController(IBaseRepository<T> repository) => _baseRepository = repository;

        [HttpGet("count")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
        public async Task<IActionResult> GetItemsCount() =>
            Ok(await _baseRepository.GetCount());


        [HttpPost("exist")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(bool))]
        public async Task<IActionResult> Exist(T item) => await _baseRepository.Exist(item) ? Ok(true) : NotFound(false);


        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll() =>
           Ok(await _baseRepository.GetAll());

        [HttpGet("items[[{skip:int}:{count:int}]]")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<T>>> Get(int skip, int count) =>
            Ok(await _baseRepository.Get(skip, count));

        [HttpGet("page/{pageIndex:int}/{pageSize:int}")]
        [HttpGet("page[[{pageIndex:int}:{pageSize:int}]]")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IPage<T>>> GetPage(int pageIndex, int pageSize)
        {
            var result = await _baseRepository.GetPage(pageIndex, pageSize);
            return result.Items.Any() ? Ok(result) : NotFound(result);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> Add(T item)
        {
            var result = await _baseRepository.Add(item);
            return Ok(result);
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(T item)
        {
            if (await _baseRepository.Update(item) is not { } result)
                return NotFound(item);
            return Ok(result);
        }

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(T item)
        {
            if (await _baseRepository.Delete(item) is not { } result)
                return NotFound(item);
            return Ok(result);
        }
    }
}
