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
    public class ProductInstancesController : ControllerBase
    {
        private readonly V2DataContext _context;

        public ProductInstancesController(V2DataContext context)
        {
            _context = context;
        }

        // GET: api/ProductInstances
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductInstance>>> GetProductInstance()
        {
            return await _context.ProductInstance.Include(p => p.Product.SubCategory.Category).ToListAsync();
        }

        // GET: api/ProductInstances/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductInstance>> GetProductInstance(int id)
        {
            var productInstance = await _context.ProductInstance.FindAsync(id);
            List<Product> products = await _context.Product.Include(p => p.SubCategory.Category).ToListAsync();


            if (productInstance == null)
            {
                return NotFound();
            }

            productInstance.Product = products.Find(p => p.Barcode == productInstance.Barcode);

            return productInstance;
        }

        // PUT: api/ProductInstances/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProductInstance(int id, ProductInstance productInstance)
        {
            if (id != productInstance.ProductInstanceId)
            {
                return BadRequest();
            }

            _context.Entry(productInstance).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductInstanceExists(id))
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

        // POST: api/ProductInstances
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ProductInstance>> PostProductInstance(ProductInstance productInstance)
        {
            _context.ProductInstance.Add(productInstance);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProductInstance", new { id = productInstance.ProductInstanceId }, productInstance);
        }

        // DELETE: api/ProductInstances/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProductInstance(int id)
        {
            var productInstance = await _context.ProductInstance.FindAsync(id);
            if (productInstance == null)
            {
                return NotFound();
            }

            _context.ProductInstance.Remove(productInstance);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProductInstanceExists(int id)
        {
            return _context.ProductInstance.Any(e => e.ProductInstanceId == id);
        }
    }
}
