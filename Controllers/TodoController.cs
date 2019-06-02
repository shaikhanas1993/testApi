using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using testapi.Models;


namespace testapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoController : ControllerBase
    {
        private  readonly  TodoContext _context;

        public TodoController(TodoContext context){
            _context = context;

            if(_context.TodoItems.Count() == 0)
            {
                _context.TodoItems.Add( new TodoItem {Name ="item1"});
                _context.SaveChanges();
            }

        }

        // GET api/values
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoItem>>>  Get()
        {
            return await _context.TodoItems.ToListAsync();
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public  async Task<ActionResult<TodoItem>>  Get(long id)
        {
            var element = await _context.TodoItems.FindAsync(id);
            if(element == null)
            {
                return NotFound();
            }
            return element;
        }

        // POST api/values
        [HttpPost]
        public async Task<ActionResult<TodoItem>> Post(TodoItem item)
        {
           await  _context.TodoItems.AddAsync(item);
           await _context.SaveChangesAsync();


            return CreatedAtAction(nameof(Get),new { id = item.Id} ,item);
            
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(long id, TodoItem item)
        {
            if (id != item.Id)
            {
                return BadRequest();
            }

            _context.Entry(item).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var item = await _context.TodoItems.FindAsync(id);
            if(item == null)
            {
                return NotFound();
            }

            _context.TodoItems.Remove(item);
            await _context.SaveChangesAsync();


            return NoContent();
        }
    }
}
