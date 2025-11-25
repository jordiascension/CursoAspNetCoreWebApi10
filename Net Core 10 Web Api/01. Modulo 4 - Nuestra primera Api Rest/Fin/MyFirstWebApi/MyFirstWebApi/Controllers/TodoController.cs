using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using MyFirstWebApi.Models;
using MyFirstWebApi.Repositories;

namespace MyFirstWebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TodoController : Controller
    {
        private readonly TodoRepository _repo;

        public TodoController(TodoRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public IActionResult GetAll() =>
            Ok(_repo.GetAll());

        [HttpGet("{id:int}")]
        public IActionResult Get(int id)
        {
            var item = _repo.Get(id);
            return item is null ? NotFound() : Ok(item);
        }

        [HttpPost]
        public IActionResult Create([FromBody] TodoItem item)
        {
            var created = _repo.Create(item);
            return Created($"/api/todos/{created.Id}", created);
        }

        [HttpPut("{id:int}")]
        public IActionResult Update(int id, [FromBody] TodoItem item)
        {
            return _repo.Update(id, item) ? Ok(item) : NotFound();
        }

        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            return _repo.Delete(id) ? NoContent() : NotFound();
        }
    }
}
