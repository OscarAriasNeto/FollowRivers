using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FollowRivers.Data;
using FollowRivers.Models;
using FollowRivers.Models;

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

        // GET: api/riveraddress
        [HttpGet]
        public async Task<IActionResult> GetRiverAddresses()
        {
            var riverAddresses = await _context.RiverAddresses.Include(r => r.Person).ToListAsync();
            return Ok(riverAddresses);
        }

        // POST: api/riveraddress
        [HttpPost]
        public async Task<IActionResult> CreateRiverAddress([FromBody] RiverAddress riverAddress)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _context.RiverAddresses.Add(riverAddress);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetRiverAddresses), new { id = riverAddress.RiverAddressId }, riverAddress);
        }

        // PUT: api/riveraddress/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRiverAddress(int id, [FromBody] RiverAddress riverAddress)
        {
            if (id != riverAddress.RiverAddressId)
                return BadRequest();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

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

        // DELETE: api/riveraddress/{id}
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
