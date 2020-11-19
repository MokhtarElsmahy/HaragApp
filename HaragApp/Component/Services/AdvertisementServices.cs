using HaragApp.Component.Interfaces;
using HaragApp.Data;
using HaragApp.Models;
using HaragApp.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaragApp.Component.Services
{
    public class AdvertisementServices : IAdverstisment
    {
        private readonly ApplicationDbContext _context;

        public AdvertisementServices(ApplicationDbContext context)
        {
            _context = context;
        }

        public AdsImagesVm Create(AdsImagesVm advertisment)
        {
                       
            Advertisment ad = new Advertisment()
            {
                CategoryID = advertisment.CategoryID,
                CityID = advertisment.CityID,
                IsPact = advertisment.IsPact,
                Title = advertisment.Title,
                Description = advertisment.Description,
                UserId = "626f057d-409a-49d0-a38f-8bb22f6634a7"
            };
            _context.Add(ad);
            _context.SaveChanges();

            if (advertisment.ImageUrl1 != null)
            {
                AdImage img = new AdImage() { AdID = ad.AdID, img = advertisment.ImageUrl1 };
                _context.Add(img);
                _context.SaveChanges();

            }
            if (advertisment.ImageUrl2 != null)
            {
                AdImage img = new AdImage() { AdID = ad.AdID, img = advertisment.ImageUrl2 };
                _context.Add(img);
                _context.SaveChanges();

            }
            if (advertisment.ImageUrl3 != null)
            {
                AdImage img = new AdImage() { AdID = ad.AdID, img = advertisment.ImageUrl3 };
                _context.Add(img);
                _context.SaveChanges();

            }
            if (advertisment.ImageUrl4 != null)
            {
                AdImage img = new AdImage() { AdID = ad.AdID, img = advertisment.ImageUrl4 };
                _context.Add(img);
                _context.SaveChanges();

            }
            if (advertisment.ImageUrl5 != null)
            {
                AdImage img = new AdImage() { AdID = ad.AdID, img = advertisment.ImageUrl5 };
                _context.Add(img);
                _context.SaveChanges();
            }
            advertisment.AdID = ad.AdID;
            return advertisment;
        }


        public AdsImagesVm Details(int? id)
        {
            if (id == null)
            {
                return null;
            }

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
            if (adv == null)
            {
                return null;
            }
            return adsVM;
        }
    }
}
