using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FollowRivers.Data;
using FollowRivers.Models;
using FollowRivers.Models;
using FollowRivers.DTO;

namespace FollowRivers.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RiverAddressController : ControllerBase
    {
        private readonly FollowRiversContext _context;

        public RiverAddressController(FollowRiversContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetRiverAddresses()
        {
            var riverAddresses = await _context.RiverAddresses.Include(r => r.Person).ToListAsync();
            return Ok(riverAddresses);
        }

        [HttpPost]
        public async Task<IActionResult> CreateRiverAddress([FromBody] RiverAddressDTO riverAddressDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            Person person = await _context.Persons.FindAsync(riverAddressDTO.PersonId);
            if (person == null) return BadRequest(ModelState);
            var riverAddress = new RiverAddress { Address = riverAddressDTO.Address, CanCauseFlood = riverAddressDTO.CanCauseFlood, Person = person };
            _context.RiverAddresses.Add(riverAddress);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetRiverAddresses), new { id = riverAddress.RiverAddressId }, riverAddress);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRiverAddress(int id, [FromBody] RiverAddressDTO riverAddressDTO)
        {
            var person = await _context.Persons.FindAsync(riverAddressDTO.PersonId);
            if (person == null) return BadRequest();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var riverAddress = await _context.RiverAddresses.FindAsync(id);
            riverAddress.Address = riverAddressDTO.Address;
            riverAddress.Person = person;
            riverAddress.CanCauseFlood = riverAddressDTO.CanCauseFlood;
            _context.Entry(riverAddress).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.RiverAddresses.Any(e => e.RiverAddressId == id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRiverAddress(int id)
        {
            var riverAddress = await _context.RiverAddresses.FindAsync(id);
            if (riverAddress == null)
                return NotFound();

            _context.RiverAddresses.Remove(riverAddress);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
