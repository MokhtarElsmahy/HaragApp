using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using HaragApp.Models;
using HaragApp.ViewModels;
using Microsoft.AspNetCore.Identity;
using HaragApp.Data;
using HaragApp.Component.Interfaces;
using HaragApp.Component.Services;
using HaragApp.Commonet;
using Microsoft.AspNetCore.Hosting;

namespace HaragApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<ApplicationDbUser> _userManager;
        private readonly ApplicationDbContext _context;
        private IHostingEnvironment _hosting { get; set; }
        public HomeController(ILogger<HomeController> logger, IHostingEnvironment hosting, ApplicationDbContext context,UserManager<ApplicationDbUser> userManager)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager; _hosting = hosting;
        }

        public IActionResult Index()
        {
            HomeViewModel model = new HomeViewModel();
            IAdverstisment dd = new AdvertisementServices(_context, _hosting);
            IAnimalCategory aa = new AnimalServices(_context);
            model.PaidAdvs = dd.GetAllPaidAdv();
            model.animalCategories = aa.GetAll();
            model.advertisments = dd.GetAdvertismentsForIndex();
            model.TOPadvertisments = dd.GetTopFiveFavs();
            model.config = _context.Configs.FirstOrDefault();
            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult about()
        {
            Configs config = _context.Configs.FirstOrDefault();
            return View(config);
        }

        public IActionResult Contact()
        {
            Configs config = _context.Configs.FirstOrDefault();
            return View(config);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<IActionResult> ChangePasswordAsync(string oldPass , string newPass)
        {
            var user = await _userManager.GetUserAsync(User);
            if(user == null)
            {
                return RedirectToAction("Index");
            }


            var changePasswordResult = await _userManager.ChangePasswordAsync(user, oldPass, newPass);
            await _userManager.UpdateAsync(user);

            return RedirectToAction("Index");

        }


    }
}
