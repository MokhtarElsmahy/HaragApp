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

namespace HaragApp.Controllers
{
   // [Authorize]
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
        public async Task<IActionResult> Details(int? id)
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

        // GET: Advertisments/Create
        public IActionResult Create()
        {
            
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
        public async Task<IActionResult> Create(Advertisment advertisment , string ImageUrl1, string ImageUrl2, string ImageUrl3, string ImageUrl4, string ImageUrl5)
        {
            if (ModelState.IsValid)
            {

               // var user = await _userManager.GetUserAsync(User);
                advertisment.UserId = "307d13f5-3199-495b-88dd-fcf23d145726";
                _context.Add(advertisment);
                await _context.SaveChangesAsync();
                if (ImageUrl1 != null)
                {
                  AdImage img = new AdImage() { AdID = advertisment.AdID, img = ImageUrl1 };
                    _context.Add(img);
                    _context.SaveChanges();
                }
                if (ImageUrl2 != null)
                {
                    AdImage img = new AdImage() { AdID = advertisment.AdID, img = ImageUrl2 };
                    _context.Add(img);
                    _context.SaveChanges();
                }
                if (ImageUrl3 != null)
                {
                    AdImage img = new AdImage() { AdID = advertisment.AdID, img = ImageUrl3 };
                    _context.Add(img);
                    _context.SaveChanges();
                }
                if (ImageUrl4 != null)
                {
                    AdImage img = new AdImage() { AdID = advertisment.AdID, img = ImageUrl4 };
                    _context.Add(img);
                    _context.SaveChanges();
                }
                if (ImageUrl5 != null)
                {
                    AdImage img = new AdImage() { AdID = advertisment.AdID, img = ImageUrl5 };
                    _context.Add(img);
                    _context.SaveChanges();
                }
                //if (ImageUrl1 != null)
                //{
                //    iSellerReciept.ReceiptImage = ImageUrl;
                //}
                //else
                //{
                //    iSellerReciept.ReceiptImage = "/assets/img/defaultRecImage.png";
                //}
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryID"] = new SelectList(_context.AnimalCategories, "CategoryID", "CategoryName", advertisment.CategoryID);
            ViewData["CityID"] = new SelectList(_context.Cities, "CityID", "CityID", advertisment.CityID);
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
