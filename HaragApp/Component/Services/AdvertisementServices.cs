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
                IsPaid= advertisment.IsPaid,
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
         
            adsVM.ImageUrl1 = adImages[0].img ;
            adsVM.ImageUrl2 = adImages[1].img ;
            adsVM.ImageUrl3 = adImages[2].img ;
            adsVM.ImageUrl4 = adImages[3].img ;
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
        public List<PaidAddViewModel> GetAllPaidAdv()
        {
            
            return _context.Advertisments.Include(x=>x.AdImages).ToList().Where(c => c.IsPaid == true)
                .Select(x=>new PaidAddViewModel { Title=x.Title , AdID=x.AdID ,ImageUrl1=x.AdImages.ToList()[0].img}).ToList();
        }

        public void DeletePaidAdd(int id)
        {
            var add = _context.Advertisments.Find(id);
            _context.Remove(add);
            _context.SaveChanges();
        }

        public AdsImagesVm GetPaidAdv(int id)
        {
            var ad = _context.Advertisments.Include(c => c.AdImages).FirstOrDefault(c => c.AdID == id);
            AdsImagesVm model = new AdsImagesVm()
            {
                AdID = ad.AdID,
                CategoryID = ad.CategoryID,
                CityID = ad.CityID,
                Description = ad.Description,
                Title = ad.Title,
                UserId = ad.UserId,
                ImageUrl1 = ad.AdImages.ToList()[0].img,
                ImageUrl2 = ad.AdImages.ToList()[1].img,
                ImageUrl3 = ad.AdImages.ToList()[2].img,
                ImageUrl4 = ad.AdImages.ToList()[3].img,
                ImageUrl5 = ad.AdImages.ToList()[4].img

            };
            return model;
        }

        public void EditPaidAdv(AdsImagesVm advertisment)
        {
            var ad = _context.Advertisments.Find(advertisment.AdID);
            ad.CityID = advertisment.CityID;
            ad.CategoryID = advertisment.CategoryID;
            ad.Description = advertisment.Description;
            ad.IsPact = advertisment.IsPact;
            ad.IsPaid = advertisment.IsPaid;
            ad.Title = advertisment.Title;
            ad.UserId = advertisment.UserId;

            var AdImages = _context.AdImages.Where(c => c.AdID == advertisment.AdID).ToList();
            AdImages[0].img = advertisment.ImageUrl1;
            AdImages[1].img = advertisment.ImageUrl2;
            AdImages[2].img = advertisment.ImageUrl3;
            AdImages[3].img = advertisment.ImageUrl4;
            AdImages[4].img = advertisment.ImageUrl5;

            _context.SaveChanges();
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

        public ShopViewModel Shop(ShopViewModel model)
        {

            model.Advertisments = _context.Advertisments.Include(c => c.AdImages).Include(c=>c.AnimalCategory).Include(c=>c.City)
                .Select(x=>new AdsImagesVm  { AdID=x.AdID, Title=x.Title ,CategoryID=x.CategoryID,CityID=x.CityID, CategoryName = x.AnimalCategory.CategoryName, CityName=x.City.CityName ,Lang=Convert.ToDouble(x.City.Langtude),
                  Lat=  Convert.ToDouble(x.City.Lantitude)
                , ImageUrl1=x.AdImages.ToList()[0].img,
                    ImageUrl2= x.AdImages.ToList()[1].img
                ,
                    ImageUrl3 = x.AdImages.ToList()[2].img
                ,
                    ImageUrl4 = x.AdImages.ToList()[3].img
                ,
                    ImageUrl5 = x.AdImages.ToList()[4].img
             
                 
                }).ToList();
            model.Cities = _context.Cities.ToList();
            model.Categories = _context.AnimalCategories.ToList();
            model.PageNo = model.PageNo;
            model.search = model.search;
            model.Km = model.Km;
            if (model.search != null)
            {

                if (model.CategoryId != 0)
                {

                    if (model.CityId != 0)
                    {
                        model.Advertisments = model.Advertisments.Where(c => c.CategoryID == model.CategoryId && c.CityID == model.CityId && (c.Title.Contains(model.search) || c.CategoryName.Contains(model.search))).ToList();
                    }
                    else
                    {
                        model.Advertisments = model.Advertisments.Where(c => c.CategoryID == model.CategoryId && (c.Title.Contains(model.search) || c.CategoryName.Contains(model.search))).ToList();
                    }

                }
                else
                {
                    if (model.CityId != 0)
                    {
                        model.Advertisments = model.Advertisments.Where(c => c.CityID == model.CityId && (c.Title.Contains(model.search) || c.CategoryName.Contains(model.search))).ToList();
                    }
                    else
                    {
                        model.Advertisments = model.Advertisments.Where(c => (c.Title.Contains(model.search) || c.CategoryName.Contains(model.search))).ToList();
                    }


                }

            }
            else
            {
                if (model.CategoryId != 0)
                {

                    if (model.CityId != 0)
                    {

                        model.Advertisments = model.Advertisments.Where(c => c.CategoryID == model.CategoryId && c.CityID == model.CityId).ToList();
                    }
                    else
                    {
                        model.Advertisments = model.Advertisments.Where(c => c.CategoryID == model.CategoryId).ToList();
                    }




                }
                else
                {
                    if (model.CityId != 0)
                    {
                        model.Advertisments = model.Advertisments.Where(c => c.CityID == model.CityId).ToList();
                    }


                }
            }
            if (model.Lang != 0)
            {

                model.Advertisments = GetNearestAdvertisments(model.Lat, model.Lang, model.Km, model.Advertisments);
            }

            model.AllAdsCount = model.Advertisments.Count();
            model.Advertisments = model.Advertisments.OrderByDescending(c => c.AdID).Skip((model.PageNo - 1) * 6).Take(6).ToList();
            return model;
        }
        public List<AdsImagesVm> GetNearestAdvertisments(double currentLatitude, double currentLongitude,
         int km, List<AdsImagesVm> data)
        {

            List<AdsImagesVm> advertsments = new List<AdsImagesVm>();
            var query = (from c in data

                         select c).ToList();
            foreach (var ad in data)
            {
                double distance = Distance(currentLatitude, currentLongitude, Convert.ToDouble(ad.Lat), Convert.ToDouble(ad.Lang));
                if (distance <= km)         //nearbyplaces which are within 25 kms  50 w 70
                {
                    AdsImagesVm dist = new AdsImagesVm();
                   
                    dist.Title = ad.Title;
                    dist.CityID = ad.CityID;
                    dist.CityName = ad.CityName;
                    dist.CategoryID = ad.CategoryID;
                    dist.CategoryName = ad.CategoryName;
                    dist.Description = ad.Description;
                    dist.Lang = ad.Lang;
                    dist.Lat = ad.Lat;
                    dist.ImageUrl1 = ad.ImageUrl1;
                    dist.ImageUrl2 = ad.ImageUrl2;
                    dist.ImageUrl3 = ad.ImageUrl3;
                    dist.ImageUrl4 = ad.ImageUrl4;
                    dist.ImageUrl5 = ad.ImageUrl5;
                    advertsments.Add(dist);

                }

            }


            return advertsments;
        }

        private double Distance(double lat1, double lon1, double lat2, double lon2)
        {
            double theta = lon1 - lon2;
            double dist = Math.Sin(deg2rad(lat1)) * Math.Sin(deg2rad(lat2)) + Math.Cos(deg2rad(lat1)) * Math.Cos(deg2rad(lat2)) * Math.Cos(deg2rad(theta));
            dist = Math.Acos(dist);
            dist = rad2deg(dist);
            dist = (dist * 60 * 1.1515) / 0.6213711922;
            // dist = (dist  * 1.609344);   //miles to kms
            return (dist);
        }
        private double deg2rad(double deg)
        {
            return (deg * Math.PI / 180.0);
        }

        private double rad2deg(double rad)
        {
            return (rad * 180.0 / Math.PI);
        }
    }
}
