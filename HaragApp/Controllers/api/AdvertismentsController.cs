using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HaragApp.Data;
using HaragApp.Models;

namespace HaragApp.Controllers.api
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdvertismentsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AdvertismentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Advertisments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Advertisment>>> GetAdvertisments()
        {
            return await _context.Advertisments.ToListAsync();
        }

        // GET: api/Advertisments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Advertisment>> GetAdvertisment(int id)
        {
            var advertisment = await _context.Advertisments.FindAsync(id);

            if (advertisment == null)
            {
                return NotFound();
            }

            return advertisment;
        }

        // PUT: api/Advertisments/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAdvertisment(int id, Advertisment advertisment)
        {
            if (id != advertisment.AdID)
            {
                return BadRequest();
            }

            _context.Entry(advertisment).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AdvertismentExists(id))
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

        // POST: api/Advertisments
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Advertisment>> PostAdvertisment(Advertisment advertisment)
        {
            _context.Advertisments.Add(advertisment);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAdvertisment", new { id = advertisment.AdID }, advertisment);
        }

        // DELETE: api/Advertisments/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Advertisment>> DeleteAdvertisment(int id)
        {
            var advertisment = await _context.Advertisments.FindAsync(id);
            if (advertisment == null)
            {
                return NotFound();
            }

            _context.Advertisments.Remove(advertisment);
            await _context.SaveChangesAsync();

            return advertisment;
        }

        private bool AdvertismentExists(int id)
        {
            return _context.Advertisments.Any(e => e.AdID == id);
        }
    }
}
