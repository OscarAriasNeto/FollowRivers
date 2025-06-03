using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FollowRivers.Data;
using FollowRivers.Models;

namespace FollowRivers.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PersonController : ControllerBase
    {
        private readonly FollowRiversContext _context;

        public PersonController(FollowRiversContext context)
        {
            _context = context;
        }

        // GET: api/person
        [HttpGet]
        public async Task<IActionResult> GetPersons()
        {
            var persons = await _context.Persons.Include(p => p.RiverAddresses).ToListAsync();
            return Ok(persons);
        }

        // POST: api/person
        [HttpPost]
        public async Task<IActionResult> CreatePerson([FromBody] Person person)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _context.Persons.Add(person);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPersons), new { id = person.PersonId }, person);
        }

        // PUT: api/person/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePerson(int id, [FromBody] Person person)
        {
            if (id != person.PersonId)
                return BadRequest();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _context.Entry(person).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Persons.Any(e => e.PersonId == id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        // DELETE: api/person/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePerson(int id)
        {
            var person = await _context.Persons.FindAsync(id);
            if (person == null)
                return NotFound();

            _context.Persons.Remove(person);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
