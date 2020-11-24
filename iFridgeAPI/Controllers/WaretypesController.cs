using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FridgeModels;
using iFridgeAPI.Data;

namespace iFridgeAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WaretypesController : ControllerBase
    {
        private readonly ContextWaretype _context;

        public WaretypesController(ContextWaretype context)
        {
            _context = context;
        }

        // GET: api/Waretypes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Waretype>>> GetWaretypes()
        {
            return await _context.Waretypes.ToListAsync();
        }

        // GET: api/Waretypes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Waretype>> GetWaretype(int id)
        {
            var waretype = await _context.Waretypes.FindAsync(id);

            if (waretype == null)
            {
                return NotFound();
            }

            return waretype;
        }

        // PUT: api/Waretypes/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutWaretype(int id, Waretype waretype)
        {
            if (id != waretype.Barcode)
            {
                return BadRequest();
            }

            _context.Entry(waretype).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!WaretypeExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Waretypes
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Waretype>> PostWaretype(Waretype waretype)
        {
            _context.Waretypes.Add(waretype);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetWaretype", new { id = waretype.Barcode }, waretype);
        }

        // DELETE: api/Waretypes/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Waretype>> DeleteWaretype(int id)
        {
            var waretype = await _context.Waretypes.FindAsync(id);
            if (waretype == null)
            {
                return NotFound();
            }

            _context.Waretypes.Remove(waretype);
            await _context.SaveChangesAsync();

            return waretype;
        }

        private bool WaretypeExists(int id)
        {
            return _context.Waretypes.Any(e => e.Barcode == id);
        }
    }
}
