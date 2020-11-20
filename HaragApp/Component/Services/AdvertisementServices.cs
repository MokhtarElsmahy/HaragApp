using HaragApp.Component.Interfaces;
using HaragApp.Data;
using HaragApp.Models;
using HaragApp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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

        public AdsImagesVm CreateAsync(AdsImagesVm advertisment)
        {
           
            Advertisment ad = new Advertisment()
            {
                CategoryID = advertisment.CategoryID,
                CityID = advertisment.CityID,
                IsPact = advertisment.IsPact,
                Title = advertisment.Title,
                Description = advertisment.Description,
              
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

        public List<AdsImagesVm> GetUserAdvertisementsAsync(string id)
        {
            var Advs = _context.Advertisments.Include(xx => xx.AdImages).Include(xx=>xx.AnimalCategory).Include(xx=>xx.City).Where(xx => xx.UserId == id).ToList();
            List<AdsImagesVm> adsImagesVms = new List<AdsImagesVm>();
            foreach (var item in Advs)
            {
                AdsImagesVm ads = new AdsImagesVm();
                ads.AdID = item.AdID;
                ads.UserId = item.UserId;
                ads.Title = item.Title;
                ads.CategoryID = item.CategoryID;
                ads.CategoryName = item.AnimalCategory.CategoryName;
                ads.CityID = item.CityID;
                ads.CityName = item.City.CityName;
                ads.Description = item.Description;
                ads.ImageUrl1 = item.AdImages.ToList()[0].img;
                ads.ImageUrl2 = item.AdImages.ToList()[1].img;
                ads.ImageUrl3 = item.AdImages.ToList()[2].img;
                ads.ImageUrl4 = item.AdImages.ToList()[3].img;
                ads.ImageUrl5 = item.AdImages.ToList()[4].img;

                adsImagesVms.Add(ads);

            }

            return adsImagesVms;
        }

        public bool userDeleteADV(int id)
        {
            var advertisment = _context.Advertisments.Find(id);
            _context.Advertisments.Remove(advertisment);
            _context.SaveChanges();
            var advertismentCheck = _context.Advertisments.Find(id);
            if (advertismentCheck == null)
            {
                return true;
            }else
            {
                return false;
            }
        }

        public AdsImagesVm userUpdateADV(int id , AdsImagesVm newADV)
        {
            var oldADV = _context.Advertisments
                   .Include(xx => xx.AdImages)
                   .Include(xx => xx.AnimalCategory)
                   .Include(xx => xx.City)
                   .Where(xx => xx.AdID == newADV.AdID).FirstOrDefault();

            oldADV.Title = newADV.Title;
            oldADV.AdImages.ToList()[0].img = newADV.ImageUrl1;
            oldADV.AdImages.ToList()[1].img = newADV.ImageUrl2;
            oldADV.AdImages.ToList()[2].img = newADV.ImageUrl3;
            oldADV.AdImages.ToList()[3].img = newADV.ImageUrl4;
            oldADV.AdImages.ToList()[4].img = newADV.ImageUrl5;
            oldADV.CityID = newADV.CityID;
            oldADV.CategoryID = newADV.CategoryID;
            oldADV.Description = newADV.Description;

            _context.SaveChanges();

            return newADV;
        }
    }
}
