﻿using System;
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

namespace HaragApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<ApplicationDbUser> _userManager;
        private readonly ApplicationDbContext _context;
        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context,UserManager<ApplicationDbUser> userManager)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            HomeViewModel model = new HomeViewModel();
            IAdverstisment dd = new AdvertisementServices(_context);
            IAnimalCategory aa = new AnimalServices(_context);
            model.PaidAdvs = dd.GetAllPaidAdv();
            model.animalCategories = aa.GetAll();
            model.advertisments = dd.GetAdvertismentsForIndex();
            model.TOPadvertisments = dd.GetTopFiveFavs();
            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        
    }
}
