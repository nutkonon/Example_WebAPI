using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebAPI_Example.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebAPI_Example.Controllers
{
    [Route("api/Test")]
    public class TodoController : Controller
    {
        private readonly TodoContext _context;
        public TodoController(TodoContext context)
        {
            _context = context;

            if (_context.TodoItems.Count() == 0)
            {
                _context.TodoItems.Add(new TodoItem { Name = "Item1" });
                _context.SaveChanges();
            }
        }

        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<TodoItem> GetAll()
        {
            return _context.TodoItems.ToList();
        }

        // GET api/<controller>/5
        [HttpGet("{id}", Name = "GetTodo")]
        public IActionResult GetById(long id)
        {
            var item = _context.TodoItems.FirstOrDefault(t => t.Id == id);
            if (item == null)
            {
                return NotFound();
            }
            return new ObjectResult(item);
        }

        // POST api/<controller>
        [HttpPost("[action]")]
        public IActionResult Create([FromBody] TodoItem item)
        {
            if (item == null)
            {
                return BadRequest();
            }
            _context.TodoItems.Add(item);
            _context.SaveChanges();
            
            return CreatedAtRoute("GetTodo", new { id = item.Id }, item);
        }

        // PUT api/<controller>/5
        [HttpPut("[action]/{id}")]
        public IActionResult Update(long id, [FromBody] TodoItem item)
        {
            if (item == null || item.Id != id)
            {
                return BadRequest();
            }

            var todo = _context.TodoItems.FirstOrDefault(t => t.Id == id);
            if (todo == null)
            {
                return NotFound();
            }

            todo.IsComplete = item.IsComplete;
            todo.Name = item.Name;
            _context.TodoItems.Update(todo);
            _context.SaveChanges();
            return new NoContentResult();
        }

        // DELETE api/<controller>/5
        [HttpDelete("[action]/{id}")]
        public IActionResult Delete(long id)
        {
            var todo = _context.TodoItems.FirstOrDefault(t => t.Id == id);
            if (todo == null)
            {
                return NotFound();
            }
            _context.TodoItems.Remove(todo);
            _context.SaveChanges();
            return new NoContentResult();
        }
    }
}
