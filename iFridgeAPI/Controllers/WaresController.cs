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
    public class WaresController : ControllerBase
    {
        private readonly ContextWare _context;

        public WaresController(ContextWare context)
        {
            _context = context;
        }

        // GET: api/Wares
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Ware>>> GetWares()
        {
            return await _context.Wares.ToListAsync();
        }

        // GET: api/Wares/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Ware>> GetWare(int id)
        {
            var ware = await _context.Wares.FindAsync(id);

            if (ware == null)
            {
                return NotFound();
            }

            return ware;
        }

        // PUT: api/Wares/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutWare(int id, Ware ware)
        {
            if (id != ware.Id)
            {
                return BadRequest();
            }

            _context.Entry(ware).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!WareExists(id))
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

        // POST: api/Wares
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Ware>> PostWare(Ware ware)
        {
            _context.Wares.Add(ware);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetWare", new { id = ware.Id }, ware);
        }

        // DELETE: api/Wares/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Ware>> DeleteWare(int id)
        {
            var ware = await _context.Wares.FindAsync(id);
            if (ware == null)
            {
                return NotFound();
            }

            _context.Wares.Remove(ware);
            await _context.SaveChangesAsync();

            return ware;
        }

        private bool WareExists(int id)
        {
            return _context.Wares.Any(e => e.Id == id);
        }
    }
}
