using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HaragApp.Data;
using HaragApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using HaragApp.ViewModels;
using RestSharp;
using HaragApp.Component.Interfaces;
using HaragApp.Component.Services;

namespace HaragApp.Controllers
{
   
    
    public class AdvertismentsController : Controller
    {

        private readonly ApplicationDbContext _context;
        private IHostingEnvironment _hosting { get; set; }
        private readonly UserManager<ApplicationDbUser> _userManager;

        public AdvertismentsController(ApplicationDbContext context , IHostingEnvironment hosting ,UserManager<ApplicationDbUser> userManager)
        {
            _context = context;
            _hosting = hosting;
            _userManager = userManager;
        }

        // GET: Advertisments
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Advertisments.Include(a => a.AnimalCategory).Include(a => a.City).Include(a => a.User);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Advertisments/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
                IAdverstisment ads = new AdvertisementServices(_context);
                var addedADV = ads.Details(id);

            ViewData["CityID"] = new SelectList(_context.Cities, "CityID", "CityName",addedADV.CityID);
            ViewData["CategoryID"] = new SelectList(_context.AnimalCategories, "CategoryID", "CategoryName", addedADV.CategoryID);

            return View(addedADV);
        }


        [Authorize]
        // GET: Advertisments/Create
        public IActionResult Create()
        {
            
            //_context.Roles.Add(new IdentityRole() { Name = "admin" });
            //_context.Roles.Add(new IdentityRole() { Name = "user" });
            //_context.SaveChanges();
            ViewData["CityID"] = new SelectList(_context.Cities, "CityID", "CityName");
            ViewData["CategoryID"] = new SelectList(_context.AnimalCategories, "CategoryID", "CategoryName");

            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: Advertisments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAsync(AdsImagesVm advertisment)
        {
            /*_context.Roles.Add(new IdentityRole { Name = "admin" });
            _context.Roles.Add(new IdentityRole { Name = "user" });
            _context.SaveChanges();*/
            if (ModelState.IsValid)
            {
                IAdverstisment ads = new AdvertisementServices(_context);
                var user = await _userManager.GetUserAsync(User);
              
                var addedADV = ads.CreateAsync(advertisment);
                var add = _context.Advertisments.Find(addedADV.AdID);
                add.UserId = user.Id;
                _context.SaveChanges();


                return RedirectToAction("Details", new { id = addedADV.AdID });
            }
            ViewData["CategoryID"] = new SelectList(_context.AnimalCategories, "CategoryID", "CategoryName", advertisment.CategoryID);
            ViewData["CityID"] = new SelectList(_context.Cities, "CityID", "CityName", advertisment.CityID);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", advertisment.UserId);
            return View(advertisment);
        }

        // GET: Advertisments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var advertisment = await _context.Advertisments.FindAsync(id);
            if (advertisment == null)
            {
                return NotFound();
            }
            ViewData["CategoryID"] = new SelectList(_context.AnimalCategories, "CategoryID", "CategoryName", advertisment.CategoryID);
            ViewData["CityID"] = new SelectList(_context.Cities, "CityID", "CityID", advertisment.CityID);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", advertisment.UserId);
            return View(advertisment);
        }

        // POST: Advertisments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AdID,Title,IsPact,UserId,CityID,CategoryID")] Advertisment advertisment)
        {
            if (id != advertisment.AdID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(advertisment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AdvertismentExists(advertisment.AdID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryID"] = new SelectList(_context.AnimalCategories, "CategoryID", "CategoryName", advertisment.CategoryID);
            ViewData["CityID"] = new SelectList(_context.Cities, "CityID", "CityID", advertisment.CityID);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", advertisment.UserId);
            return View(advertisment);
        }

        // GET: Advertisments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var advertisment = await _context.Advertisments
                .Include(a => a.AnimalCategory)
                .Include(a => a.City)
                .Include(a => a.User)
                .FirstOrDefaultAsync(m => m.AdID == id);
            if (advertisment == null)
            {
                return NotFound();
            }

            return View(advertisment);
        }

        // POST: Advertisments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var advertisment = await _context.Advertisments.FindAsync(id);
            _context.Advertisments.Remove(advertisment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AdvertismentExists(int id)
        {
            return _context.Advertisments.Any(e => e.AdID == id);
        }

        public IActionResult UploadImage()
        {

            string result = "defaultRecImage.png";

            try
            {
                var file = Request.Form.Files;
                string uploads = Path.Combine(_hosting.WebRootPath, @"uploads");
                var filename = ContentDispositionHeaderValue.Parse(file[0].ContentDisposition).FileName.Trim('"');
                var newFileName = Guid.NewGuid() + filename;
                string fullPath = Path.Combine(uploads, newFileName);
                file[0].CopyTo(new FileStream(fullPath, FileMode.Create));


                return Json(new { image = $"/uploads/{newFileName}" });
            }
            catch (Exception ex)
            {
                result = ex.Message;
                return Json(new { image = $"/uploads/{result}" });
            }


        }
    }
}
