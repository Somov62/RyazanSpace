using Microsoft.AspNetCore.Mvc;
using RyazanSpace.DAL.Entities.Account;
using RyazanSpace.Interfaces.Repositories;

namespace RyazanSpace.DAL.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly INamedRepository<User> _repository;

        public UsersController(INamedRepository<User> repository) => _repository = repository;

        [HttpGet("count")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
        public async Task<IActionResult> GetItemsCount() =>
            Ok(await _repository.GetCount());

        [HttpGet("exist/id/{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(bool))]
        public async Task<IActionResult> ExistId(int id) =>
            await _repository.ExistId(id) ? Ok(true) : NotFound(false);


        [HttpGet("exist")]
        [HttpPost("exist")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(bool))]
        public async Task<IActionResult> Exist(User item) => await _repository.Exist(item) ? Ok(true) : NotFound(false);


        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll() =>
            Ok(await _repository.GetAll());

        [HttpGet("items[[{skip:int}:{count:int}]]")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<User>>> Get(int skip, int count) =>
            Ok(await _repository.Get(skip, count));

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id) => await _repository.GetById(id) is { } item ? Ok(item) : NotFound();

        [HttpGet("page/{pageIndex:int}/{pageSize:int}")]
        [HttpGet("page[[{pageIndex:int}:{pageSize:int}]]")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IPage<User>>> GetPage(int pageIndex, int pageSize)
        {
            var result = await _repository.GetPage(pageIndex, pageSize);
            return result.Items.Any() ? Ok(result) : NotFound(result);
        }


        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> Add(User item)
        {
            var result = await _repository.Add(item);
            return CreatedAtAction(nameof(GetById), new {id = result.Id});
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(User item)
        {
            if (await _repository.Update(item) is not { } result)
                return NotFound(item);
            return AcceptedAtAction(nameof(GetById), new { id = result.Id });
        }

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(User item)
        {
            if (await _repository.Delete(item) is not { } result)
                return NotFound(item);
            return Ok(result);
        }
    }
}
