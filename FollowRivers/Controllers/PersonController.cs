using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FollowRivers.Data;
using FollowRivers.Models;
using FollowRivers.DTO;

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

        [HttpGet]
        public async Task<IActionResult> GetPersons()
        {
            var persons = await _context.Persons.Include(p => p.RiverAddresses).ToListAsync();
            return Ok(persons);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePerson([FromBody] PersonDTO personDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var person = new Person { Email = personDTO.Email, Senha = personDTO.Senha, Name = personDTO.Name };
            _context.Persons.Add(person);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPersons), new { id = person.PersonId }, person);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePerson(int id, [FromBody] PersonDTO personDTO)
        {

            var person = await _context.Persons.FindAsync(id);
            if (person == null) { return BadRequest(ModelState); };
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            person.Senha = personDTO.Senha;
            person.Name = personDTO.Name;
            person.Email = personDTO.Email;
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
