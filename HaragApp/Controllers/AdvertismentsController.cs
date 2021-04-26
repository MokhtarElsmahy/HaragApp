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
using System.Net;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;


namespace HaragApp.Controllers
{


    public class AdvertismentsController : Controller
    {

        private readonly ApplicationDbContext _context;
        private IHostingEnvironment _hosting { get; set; }
        private readonly UserManager<ApplicationDbUser> _userManager;

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _config;

        public AdvertismentsController(ApplicationDbContext context, IHostingEnvironment hosting, UserManager<ApplicationDbUser> userManager , IHttpContextAccessor httpContextAccessor, IConfiguration config)
        {
            _hosting = hosting;
            _context = context;
            _userManager = userManager;

            _config = config;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<IActionResult> SendResendAsync(string numbers)
        {
            var client = new RestClient($"http://api.yamamah.com/SendSMS");
            var request = new RestRequest(Method.POST);
            var code = GetFormNumber().ToString();
            request.AddJsonBody(new
            {
                Username = "966532866666",
                Password = "Ht5pTY26",
                Tagname = "Haraajm",
                RecepientNumber = numbers,
                Message = code

            });
            IRestResponse response = await client.ExecuteAsync(request);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var user = await _userManager.GetUserAsync(User);
                user.code = int.Parse(code);
                await _userManager.UpdateAsync(user);
                return LocalRedirect("/identity/account/RegisterConfirmation");

            }
            return LocalRedirect("/identity/account/RegisterConfirmation");
        }
        public static int GetFormNumber()
        {
            Random rnd = new Random();
            return rnd.Next();
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
            IAdverstisment ads = new AdvertisementServices(_context,_hosting);
            var addedADV = ads.Details(id);

            ViewData["CityID"] = new SelectList(_context.Cities, "CityID", "CityName", addedADV.CityID);
            ViewData["CategoryID"] = new SelectList(_context.AnimalCategories, "CategoryID", "CategoryName", addedADV.CategoryID);
            ViewData["SliderImage"] = _context.Configs.ToList()[0].shopSliderImage;

            return View(addedADV);
        }

        [HttpGet]
        public async Task<IActionResult> GetUserAds()
        {
            var user = await _userManager.GetUserAsync(User);

            IAdverstisment ads = new AdvertisementServices(_context, _hosting);
            var advList = ads.GetUserAdvertisementsAsync(user.Id);

            return View(advList);
        }
        // POST: Advertisments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        
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
            ViewData["SliderImage"] = _context.Configs.ToList()[0].shopSliderImage;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(AdsImagesVm advertisment)
        {

            if (ModelState.IsValid)
            {
                IAdverstisment ads = new AdvertisementServices(_context, _hosting);
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
            ViewData["SliderImage"] = _context.Configs.ToList()[0].shopSliderImage;

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
        
        
        public IActionResult userDeleteADV(int id)
        {
            IAdverstisment ads = new AdvertisementServices(_context, _hosting);
            ads.userDeleteADV(id);
            return RedirectToAction("GetUserAds", "Advertisments");
        }

        [HttpGet]
        public IActionResult userUpdateADV(int id)
        {
            var adv = _context.Advertisments
                .Include(a => a.AnimalCategory)
                .Include(a => a.City)
                .Include(a => a.User).Include(c => c.AdImages)
                .FirstOrDefault(m => m.AdID == id);
            var adsVM = new AdsImagesVm();
            adsVM.AdID = adv.AdID;
            adsVM.Title = adv.Title;
            adsVM.UserId = adv.UserId;
            adsVM.CategoryID = adv.CategoryID;
            adsVM.CategoryName = adv.AnimalCategory.CategoryName;
            adsVM.CityID = adv.CityID;
            adsVM.CityName = adv.City.CityName;
            adsVM.Description = adv.Description;
            var adImages = adv.AdImages.ToList();
            adsVM.ImageUrl1 = adImages[0].img;
            adsVM.ImageUrl2 = adImages[1].img;
            adsVM.ImageUrl3 = adImages[2].img;
            adsVM.ImageUrl4 = adImages[3].img;
            adsVM.ImageUrl5 = adImages[4].img;
            adsVM.SliderImage = _context.Configs.ToList()[0].shopSliderImage;
            ViewData["CategoryID"] = new SelectList(_context.AnimalCategories, "CategoryID", "CategoryName", adsVM.CategoryID);
            ViewData["CityID"] = new SelectList(_context.Cities, "CityID", "CityName", adsVM.CityID);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", adsVM.UserId);
            return View(adsVM);
        }

        [HttpPost]
        public IActionResult userUpdateADV(AdsImagesVm newADV)
        {
            if (ModelState.IsValid)
            {
                IAdverstisment adverstisment = new AdvertisementServices(_context, _hosting);
                adverstisment.userUpdateADV(newADV.AdID , newADV);

            }

            return RedirectToAction("GetUserAds", "Advertisments");

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


                System.Threading.Thread.Sleep(5000);
                return Json(new { image = $"/uploads/{newFileName}" });
            }
            catch (Exception ex)
            {
                result = ex.Message;
                return Json(new { image = $"/uploads/{result}" });
            }


        }

        public IActionResult IndexPaidAdv()
        {
            IAdverstisment dd = new AdvertisementServices(_context, _hosting);
            return View(dd.GetAllPaidAdv());
        }

        [Authorize(Roles = "admin")]
        public IActionResult CreatePaidAdd()
        {

            //_context.Roles.Add(new IdentityRole() { Name = "admin" });
            //_context.Roles.Add(new IdentityRole() { Name = "user" });
            //_context.SaveChanges();

            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

       

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePaidAddAsync(AdsImagesVm advertisment)
        {
            /*_context.Roles.Add(new IdentityRole { Name = "admin" });
            _context.Roles.Add(new IdentityRole { Name = "user" });
            _context.SaveChanges();*/
            if (ModelState.IsValid)
            {
                IAdverstisment ads = new AdvertisementServices(_context, _hosting);
                var user = await _userManager.GetUserAsync(User);

                var addedADV = ads.CreateAsync(advertisment);
                var add = _context.Advertisments.Find(addedADV.AdID);
                add.UserId = user.Id;
                _context.SaveChanges();


                return RedirectToAction("IndexPaidAdv");

            }
            ViewData["CategoryID"] = new SelectList(_context.AnimalCategories, "CategoryID", "CategoryName", advertisment.CategoryID);
            ViewData["CityID"] = new SelectList(_context.Cities, "CityID", "CityName", advertisment.CityID);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", advertisment.UserId);
            return View(advertisment);
        }

        [HttpGet]
        public IActionResult EditPaidAdd(int id)
        {

            IAdverstisment ads = new AdvertisementServices(_context, _hosting);
            var model = ads.GetPaidAdv(id);

            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            return View(model);
        }

        [HttpPost]
        public IActionResult EditPaidAdd(AdsImagesVm advertisment)
        {
            if (ModelState.IsValid)
            {

                IAdverstisment ads = new AdvertisementServices(_context, _hosting);
                ads.EditPaidAdv(advertisment);
                return RedirectToAction("IndexPaidAdv");
            }


            ViewData["CityID"] = new SelectList(_context.Cities, "CityID", "CityName", advertisment.CityID);
            ViewData["CategoryID"] = new SelectList(_context.AnimalCategories, "CategoryID", "CategoryName", advertisment.CategoryID);

            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            return View(advertisment);
        }
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        public IActionResult DeletePaidAdd(int Id)
        {
            IAdverstisment dd = new AdvertisementServices(_context, _hosting);
            dd.DeletePaidAdd(Id);
            return RedirectToAction(nameof(IndexPaidAdv));
        }


        public async Task<IActionResult> Shop(ShopViewModel model)
        {
            model.PageNo =  model.PageNo ?? 1;
            IAdverstisment ss = new AdvertisementServices(_context, _hosting);
           // var shop = ss.Shop(model);

            var user = await _userManager.GetUserAsync(User);
            string UserID = string.Empty;
            if (user != null)
            {
                UserID = user.Id;
            }
            model.userID = UserID;
            var shop = ss.Shop(model);

            model.Advertisments = shop.Advertisments.OrderByDescending(p => p.AdID).ToList();
            model.Cities = shop.Cities;
            model.Categories = shop.Categories;
            model.PageNo = shop.PageNo;
            model.search = shop.search;
            model.Km = 26;
            model.AllAdsCount = shop.AllAdsCount;
            model.Lang = shop.Lang;
            model.Lat = shop.Lat;
            model.CityId = shop.CityId;
            model.CategoryId = shop.CategoryId;
            model.ActiveCity = model.CityId;
            model.ActiveCategory = model.CategoryId;
            model.shopSliderImage = _context.Configs.ToList()[0].shopSliderImage;
            return View(model);
        }
      

        public async Task<IActionResult> AddToFav(int adID,string userID)
        {
          
            var user = await _userManager.GetUserAsync(User);
            if(user == null)
            {
                return Json(new { message = "Please Login First" });

            }
            IAdverstisment adv = new AdvertisementServices(_context, _hosting);

            var check = adv.AddToFav(adID, user.Id.ToString());
            if (check)
            {
                return Json(new { message = "success" });

            }
            return Json(new { message = "Failed" });


        }


        [Authorize]
        public async Task<IActionResult> ShowFavouriteAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            IAdverstisment adverstisment = new AdvertisementServices(_context, _hosting);
            var adVMs = adverstisment.GetUserFavorites(user.Id.ToString());
            return View(adVMs);
        }

        [Authorize]
        public async Task<IActionResult> DeleteFavAsync(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            IAdverstisment adverstisment = new AdvertisementServices(_context, _hosting);
            adverstisment.DeleteFav(user.Id, id);

            return RedirectToAction("ShowFavourite", "Advertisments");
        }

        //--------------------------------------------------------------------------------------

        [Authorize]
        public async Task<IActionResult> AddComment(Comment comment)
        {
            var user = await _userManager.GetUserAsync(User);
            CommentVM comVM = new CommentVM() { advID = comment.advID , UserId = user.Id , CommentText = comment.CommentText , Name = comment.Name , CommentID = comment.CommentID};
            if (ModelState.IsValid)
            {
                IAdverstisment adverstisment = new AdvertisementServices(_context, _hosting);
                
                adverstisment.AddComment(comVM);
            }
            return RedirectToAction("Details", new { id = comment.advID });

        }


        public IActionResult RemoveComment(int CommentID , int advID)
        {
            var com = _context.Comments.SingleOrDefault(x => x.CommentID == CommentID);
            _context.Comments.Remove(com);
            _context.SaveChanges();
            return RedirectToAction("Details", new { id = advID });
        }


    }
}
