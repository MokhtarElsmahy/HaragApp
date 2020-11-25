using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HaragApp.Data;
using HaragApp.Models;
using HaragApp.Commonet;

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
        public ActionResult<IEnumerable<AnimalCategory>> GetAnimalCategories()
        {
            IAnimalCategory aa = new AnimalServices(_context);
            var categories = aa.GetAll();
            return categories;
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
            IAnimalCategory animal = new AnimalServices(_context);
            animal.Create(animalCategory);

            return CreatedAtAction("GetAnimalCategory", new { id = animalCategory.CategoryID }, animalCategory);
        }

      
        private bool AnimalCategoryExists(int id)
        {
            return _context.AnimalCategories.Any(e => e.CategoryID == id);
        }
    }
}
