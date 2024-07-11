using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PersonApi.Models;
using System.IO;

namespace PersonApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonsController : ControllerBase
    {
        private readonly PersonContext _context;
        private readonly string _filePath = "PersonList.json";

        public PersonsController(PersonContext context)
        {
            _context = context;
        }

        // Function to write data to file
        private async Task WritePersonsToFile(Person p)
        {
            List<Person> plist;

            // Read existing data out of file
            if (System.IO.File.Exists(_filePath))
            {
                var thejson = await System.IO.File.ReadAllTextAsync(_filePath);
                plist = JsonSerializer.Deserialize<List<Person>>(thejson) ?? new List<Person>();
                System.IO.File.Delete(_filePath);
            }
            else
            {
                plist = new List<Person>();
            }
            plist.Add(p);

            // Add data to json file
            var Wjson = JsonSerializer.Serialize(plist, new JsonSerializerOptions { WriteIndented = true });
            await System.IO.File.AppendAllTextAsync(_filePath, Wjson);
        }


        // GET: api/Persons
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Person>>> GetPersons()
        {
            return await _context.Persons.Include(p => p.Accounts).ToListAsync();
            //return await _context.Persons.ToListAsync();
        }

        // GET: api/Persons/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Person>> GetPerson(long id)
        {
            var person = await _context.Persons.FindAsync(id);
            if (person == null)
            {
                return NotFound();
            }
            return person;
        } 


        // POST: api/TodoItems
        [HttpPost]
        public async Task<ActionResult<Person>> PostPerson(Person person)
        {
            _context.Persons.Add(person);
            await _context.SaveChangesAsync();

            person.countVowelsAndConsonants(); // Count vowels and consonants of person name
            person.setReversedName(); // Set the name of prson reversed
            person.ConsoleOutput(); // Output in console Person

            // Write to file
            await WritePersonsToFile(person);

            //return CreatedAtAction("GetTodoItem", new { id = todoItem.Id }, todoItem);
            return CreatedAtAction(nameof(GetPerson), new { id = person.Id }, person);
        }

        // DELETE: api/TodoItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodoItem(long id)
        {
            var person = await _context.Persons.FindAsync(id);
            if (person == null)
            {
                return NotFound();
            }
            _context.Persons.Remove(person);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
