using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HaragApp.Data;
using HaragApp.Models;
using RestSharp;

namespace HaragApp.Controllers
{
    public class AnimalCategoriesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AnimalCategoriesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: AnimalCategories
        public async Task<IActionResult> Index()
        {
            return View(await _context.AnimalCategories.ToListAsync());
        }

        // GET: AnimalCategories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var animalCategory = await _context.AnimalCategories
                .FirstOrDefaultAsync(m => m.CategoryID == id);
            if (animalCategory == null)
            {
                return NotFound();
            }

            return View(animalCategory);
        }

        // GET: AnimalCategories/Create
        public IActionResult Create()
        {
            ViewBag.CategoryList = _context.AnimalCategories.ToList();
            return View();
        }

        // POST: AnimalCategories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CategoryID,CategoryName")] AnimalCategory animalCategory)
        {
            if (ModelState.IsValid)
            {

                var client = new RestClient($"https://localhost:44396/api/AnimalCategories");
                var request = new RestRequest(Method.POST);
                request.AddJsonBody(animalCategory);
                IRestResponse response = await client.ExecuteAsync(request);
                return RedirectToAction(nameof(Create));

            }
            return View(animalCategory);
        }

        // GET: AnimalCategories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var animalCategory = await _context.AnimalCategories.FindAsync(id);
            if (animalCategory == null)
            {
                return NotFound();
            }
            return PartialView(animalCategory);
        }

        // POST: AnimalCategories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("CategoryID,CategoryName")] AnimalCategory animalCategory)
        {
            if (ModelState.IsValid)
            {
                try
                {
                   

                    var client = new RestClient($"https://localhost:44396/api/AnimalCategories/"+animalCategory.CategoryID);
                    var request = new RestRequest(Method.PUT);
                    request.AddJsonBody(animalCategory);
                    IRestResponse response = await client.ExecuteAsync(request);
                   
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AnimalCategoryExists(animalCategory.CategoryID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Create));
            }
            return View(animalCategory);
        }

        // GET: AnimalCategories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
          
            var client = new RestClient($"https://localhost:44396/api/AnimalCategories/"+id);
            var request = new RestRequest(Method.DELETE);
           // request.AddJsonBody(id);
            IRestResponse response = await client.ExecuteAsync(request);
            return RedirectToAction(nameof(Create));
        }

        private bool AnimalCategoryExists(int id)
        {
            return _context.AnimalCategories.Any(e => e.CategoryID == id);
        }
    }
}
