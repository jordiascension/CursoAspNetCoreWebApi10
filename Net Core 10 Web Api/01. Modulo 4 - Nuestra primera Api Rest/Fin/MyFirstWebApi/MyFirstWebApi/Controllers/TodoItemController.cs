using Microsoft.AspNetCore.Mvc;

using MyFirstWebApi.Models;
using MyFirstWebApi.Repositories;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MyFirstWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoItemController : ControllerBase
    {
        private readonly TodoItemRepository _todoItemRepository;

        public TodoItemController(TodoItemRepository todoItemRepository)
        {
            _todoItemRepository = todoItemRepository;
        }

        // GET: api/<TodoItemController>
        [HttpGet]
        public IActionResult Get()
        {
            var items = _todoItemRepository.GetAll();
            return Ok(items);
        }

        // GET api/<TodoItemController>/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var item = _todoItemRepository.Get(id);
            if (item == null)
                return NotFound();

            return Ok(item);
        }

        // POST api/<TodoItemController>
        [HttpPost]
        public IActionResult Create([FromBody] TodoItem item)
        {
            if (item == null)
                return BadRequest();

            var createdItem = _todoItemRepository.Create(item);
            return CreatedAtAction(nameof(Get), new { id = createdItem.Id }, createdItem);
        }

        // PUT api/<TodoItemController>/5
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] TodoItem item)
        {
            return _todoItemRepository.Get(id) is null ? NotFound() : Ok(_todoItemRepository.Update(id, item));
        }

        // DELETE api/<TodoItemController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
           return _todoItemRepository.Delete(id) ? NoContent() : NotFound();
        }
    }
}
