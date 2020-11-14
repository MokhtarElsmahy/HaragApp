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
    public class AnimalCategoriesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AnimalCategoriesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/AnimalCategories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AnimalCategory>>> GetAnimalCategories()
        {
            return await _context.AnimalCategories.ToListAsync();
        }

        // GET: api/AnimalCategories/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AnimalCategory>> GetAnimalCategory(int id)
        {
            var animalCategory = await _context.AnimalCategories.FindAsync(id);

            if (animalCategory == null)
            {
                return NotFound();
            }

            return animalCategory;
        }

        // PUT: api/AnimalCategories/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAnimalCategory(int id ,AnimalCategory animalCategory)
        {

           
            _context.Entry(animalCategory).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
               
            }

            return NoContent();
        }

        // POST: api/AnimalCategories
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<AnimalCategory>> PostAnimalCategory(AnimalCategory animalCategory)
        {
            _context.AnimalCategories.Add(animalCategory);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAnimalCategory", new { id = animalCategory.CategoryID }, animalCategory);
        }

        // DELETE: api/AnimalCategories/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<AnimalCategory>> DeleteAnimalCategory(int id)
        {
            var animalCategory = await _context.AnimalCategories.FindAsync(id);
            if (animalCategory == null)
            {
                return NotFound();
            }

            _context.AnimalCategories.Remove(animalCategory);
            await _context.SaveChangesAsync();

            return animalCategory;
        }

        private bool AnimalCategoryExists(int id)
        {
            return _context.AnimalCategories.Any(e => e.CategoryID == id);
        }
    }
}
