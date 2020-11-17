using HaragApp.Data;
using HaragApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaragApp.Commonet
{
    public class AnimalServices : IAnimalCategory
    {
        private readonly ApplicationDbContext _context;

        public AnimalServices(ApplicationDbContext context)
        {
            _context = context;
        }
        public AnimalCategory Create(AnimalCategory animalCategory)
        {
            _context.AnimalCategories.Add(animalCategory);
             _context.SaveChanges();
            return animalCategory;
            //return CreatedAtAction("GetAnimalCategory", new { id = animalCategory.CategoryID }, animalCategory);
        }

        public AnimalCategory Get(int id)
        {
            var s = _context.AnimalCategories.Where(x => x.CategoryID == id).FirstOrDefault();
            return s;
        }
    }
}
