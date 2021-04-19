using HaragApp.Component.Interfaces;
using HaragApp.Data;
using HaragApp.Models;
using HaragApp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace HaragApp.Component.Services
{

    public class AdvertisementServices : IAdverstisment
    {
        private readonly ApplicationDbContext _context;
        private IHostingEnvironment _hosting { get; set; }

        public AdvertisementServices(ApplicationDbContext context, IHostingEnvironment hosting)
        {
            _context = context; _hosting = hosting;

        }

        public AdsImagesVm CreateAsync(AdsImagesVm advertisment)
        {
            Advertisment ad;
            if (advertisment.CategoryID == 0)
            {
                ad = new Advertisment()
                {
                    CategoryID = _context.AnimalCategories.ToList().Where(x => x.CategoryID >= 1).FirstOrDefault().CategoryID,
                    CityID = _context.Cities.ToList().Where(x => x.CityID >= 1).FirstOrDefault().CityID,
                    IsPaid = true,
                    Title = advertisment.Title,
                    Description = advertisment.Description,
                    UserId = advertisment.UserId
                };
            }
            else
            {
                ad = new Advertisment()
                {
                    CategoryID = advertisment.CategoryID,
                    CityID = advertisment.CityID,
                    IsPaid = advertisment.IsPaid,
                    IsPact = advertisment.IsPact,
                    Title = advertisment.Title,
                    Description = advertisment.Description,
                    UserId = advertisment.UserId

                };
            }

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

        public AdsImagesVm UpdateAsync(AdsImagesVm advertisment, IFormFileCollection files)
        {
            Advertisment ad = _context.Advertisments.Include(c => c.AdImages).FirstOrDefault(c => c.AdID == advertisment.AdID);
            if (advertisment.CategoryID == 0)
            {

                ad.CategoryID = _context.AnimalCategories.ToList().Where(x => x.CategoryID >= 1).FirstOrDefault().CategoryID;
                ad.CityID = _context.Cities.ToList().Where(x => x.CityID >= 1).FirstOrDefault().CityID;
                ad.IsPaid = true;
                ad.Title = advertisment.Title;
                ad.Description = advertisment.Description;
                ad.UserId = advertisment.UserId;
                ad.AdImages.ToList()[0].img = advertisment.ImageUrl1;
                //ad.AdImages.ToList()[1].img = advertisment.ImageUrl2;
                //ad.AdImages.ToList()[2].img = advertisment.ImageUrl3;
                //ad.AdImages.ToList()[3].img = advertisment.ImageUrl4;
                //ad.AdImages.ToList()[4].img = advertisment.ImageUrl5;

            }
            else
            {

                ad.CategoryID = advertisment.CategoryID;
                ad.CityID = advertisment.CityID;
                ad.IsPact = true;
                ad.Title = advertisment.Title;
                ad.Description = advertisment.Description;
                ad.UserId = advertisment.UserId;
                int j = 0;
                for (int i = 0; i < advertisment.IsImageChanged.Count; i++)
                {

                    if (advertisment.IsImageChanged[i] == true)
                    {
                        string uploads = Path.Combine(_hosting.WebRootPath, @"uploads");
                        var filename = ContentDispositionHeaderValue.Parse(files[j].ContentDisposition).FileName.Trim('"');
                        var newFileName = Guid.NewGuid() + filename;
                        string fullPath = Path.Combine(uploads, newFileName);
                        ad.AdImages.ToList()[i].img = $"/uploads/{newFileName}";
                        files[j].CopyTo(new FileStream(fullPath, FileMode.Create));
                        j++;
                    }
                }


            }


            _context.SaveChanges();
            advertisment.ImageUrl1 = ad.AdImages.ToList()[0].img;
            advertisment.ImageUrl2 = ad.AdImages.ToList()[1].img;
            advertisment.ImageUrl3 = ad.AdImages.ToList()[2].img;
            advertisment.ImageUrl4 = ad.AdImages.ToList()[3].img;
            advertisment.ImageUrl5 = ad.AdImages.ToList()[4].img;

            return advertisment;
        }


        public AdsImagesVM2 Details(int? id)
        {
            if (id == null)
            {
                return null;
            }

            var adv = _context.Advertisments
                .Include(a => a.AnimalCategory)
                .Include(a => a.City)
                .Include(a => a.User).Include(c => c.AdImages).Include(x => x.Comments)
                .FirstOrDefault(m => m.AdID == id);
            var adsVM = new AdsImagesVM2();
            adsVM.AdID = adv.AdID;
            adsVM.Title = adv.Title;
            adsVM.UserId = adv.UserId;
            adsVM.CategoryID = adv.CategoryID;
            adsVM.CategoryName = adv.AnimalCategory.CategoryName;
            adsVM.CityID = adv.CityID;
            adsVM.CityName = adv.City.CityName;
            adsVM.Description = adv.Description;
            adsVM.userPhone = adv.User.Phone;

            adsVM.comments = adv.Comments.Select(x => new CommentVM { advID = x.advID, Name = x.Name, CommentText = x.CommentText, UserId = x.UserId, Date = x.Date, CommentID = x.CommentID }).ToList();

            var adImages = adv.AdImages.ToList();

            adsVM.ImageUrl1 = adImages[0].img;
            if (adv.IsPaid == false)
            {
                adsVM.ImageUrl2 = adImages[1].img;
                adsVM.ImageUrl3 = adImages[2].img;
                adsVM.ImageUrl4 = adImages[3].img;
                adsVM.ImageUrl5 = adImages[4].img;
            }


            if (adv == null)
            {
                return null;
            }

            return adsVM;
        }


        public List<AdsImagesVM2> GetUserAdvertisementsAsync(string id)
        {
            var Advs = _context.Advertisments.Include(xx => xx.AdImages).Include(xx => xx.AnimalCategory).Include(xx => xx.City).Where(xx => xx.UserId == id).Where(xx => xx.IsPaid == false).ToList();
            List<AdsImagesVM2> adsImagesVms = new List<AdsImagesVM2>();
            foreach (var item in Advs)
            {
                AdsImagesVM2 ads = new AdsImagesVM2();
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

            return _context.Advertisments.Include(x => x.AdImages).ToList().Where(c => c.IsPaid == true)
                .Select(x => new PaidAddViewModel { Title = x.Title, AdID = x.AdID, ImageUrl1 = x.AdImages.ToList()[0].img, URL = x.Description }).ToList();
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
                Description = ad.Description,
                Title = ad.Title,
                UserId = ad.UserId,
                ImageUrl1 = ad.AdImages.ToList()[0].img,


            };
            return model;
        }

        public void EditPaidAdv(AdsImagesVm advertisment)
        {
            var ad = _context.Advertisments.Find(advertisment.AdID);
            ad.Description = advertisment.Description;
            ad.IsPact = advertisment.IsPact;
            ad.IsPaid = advertisment.IsPaid;
            ad.Title = advertisment.Title;
            ad.UserId = advertisment.UserId;

            var AdImages = _context.AdImages.Where(c => c.AdID == advertisment.AdID).ToList();
            AdImages[0].img = advertisment.ImageUrl1;


            _context.SaveChanges();
        }
        public bool userDeleteADV(int id)
        {
            var imgNames = _context.AdImages.Where(x => x.AdID == id).ToList();

            var advertisment = _context.Advertisments.Find(id);
            _context.Advertisments.Remove(advertisment);
            _context.SaveChanges();

            string uploadsFolder = Path.Combine(_hosting.WebRootPath, @"uploads");
            foreach (var x in imgNames)
            {
                var arr = x.img.Split('/');
                var path = System.IO.Path.Combine(Directory.GetCurrentDirectory(), uploadsFolder, arr[arr.Length-1]);

                System.GC.Collect();
                System.GC.WaitForPendingFinalizers();

                if (System.IO.File.Exists(path))
                {
                    System.GC.Collect();
                    System.GC.WaitForPendingFinalizers();
                    System.IO.File.Delete(path);
                }
            }           
            


            var advertismentCheck = _context.Advertisments.Find(id);
            if (advertismentCheck == null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public AdsImagesVm userUpdateADV(int id, AdsImagesVm newADV)
        {
            var oldADV = _context.Advertisments
                   .Include(xx => xx.AdImages)
                   .Include(xx => xx.AnimalCategory)
                   .Include(xx => xx.City)
                   .Where(xx => xx.AdID == newADV.AdID).FirstOrDefault();

            oldADV.Title = newADV.Title;
            if (oldADV.AdImages.ToList()[0].img != newADV.ImageUrl1)
            {

                string[] arr = oldADV.AdImages.ToList()[0].img.Split('/');
                //string uploads = Path.Combine(_hosting.WebRootPath, @"uploads");

                //string fullPath = Path.Combine(uploads, arr[arr.Length - 1]);//   ----------/uploads/filename

                //oldADV.AdImages.ToList()[0].img = newADV.ImageUrl1;

                //File.Delete(fullPath);

                string uploadsFolder = Path.Combine(_hosting.WebRootPath, @"uploads");
                var path = System.IO.Path.Combine(Directory.GetCurrentDirectory(), uploadsFolder, arr[arr.Length - 1]);

                System.GC.Collect();
                System.GC.WaitForPendingFinalizers();

                if (System.IO.File.Exists(path))
                {
                    System.GC.Collect();
                    System.GC.WaitForPendingFinalizers();
                    System.IO.File.Delete(path);
                    oldADV.AdImages.ToList()[0].img = newADV.ImageUrl1;
                }



            }
            else
            {
                oldADV.AdImages.ToList()[0].img = newADV.ImageUrl1;
            }

            if (oldADV.AdImages.ToList()[1].img != newADV.ImageUrl2)
            {

                string[] arr = oldADV.AdImages.ToList()[1].img.Split('/');
                //string uploads = Path.Combine(_hosting.WebRootPath, @"uploads");

                //string fullPath = Path.Combine(uploads, arr[arr.Length - 1]);//   ----------/uploads/filename

                //oldADV.AdImages.ToList()[1].img = newADV.ImageUrl2;
                //File.Delete(fullPath);

                string uploadsFolder = Path.Combine(_hosting.WebRootPath, @"uploads");
                var path = System.IO.Path.Combine(Directory.GetCurrentDirectory(), uploadsFolder, arr[arr.Length - 1]);

                System.GC.Collect();
                System.GC.WaitForPendingFinalizers();

                if (System.IO.File.Exists(path))
                {
                    System.GC.Collect();
                    System.GC.WaitForPendingFinalizers();
                    System.IO.File.Delete(path);
                    oldADV.AdImages.ToList()[1].img = newADV.ImageUrl2;
                }


            }
            else
            {
                oldADV.AdImages.ToList()[1].img = newADV.ImageUrl2;
            }

            if (oldADV.AdImages.ToList()[2].img != newADV.ImageUrl3)
            {

                string[] arr = oldADV.AdImages.ToList()[2].img.Split('/');
                //string uploads = Path.Combine(_hosting.WebRootPath, @"uploads");

                //string fullPath = Path.Combine(uploads, arr[arr.Length - 1]);//   ----------/uploads/filename

                //oldADV.AdImages.ToList()[2].img = newADV.ImageUrl3;
                //File.Delete(fullPath);
                string uploadsFolder = Path.Combine(_hosting.WebRootPath, @"uploads");
                var path = System.IO.Path.Combine(Directory.GetCurrentDirectory(), uploadsFolder, arr[arr.Length - 1]);

                System.GC.Collect();
                System.GC.WaitForPendingFinalizers();

                if (System.IO.File.Exists(path))
                {
                    System.GC.Collect();
                    System.GC.WaitForPendingFinalizers();
                    System.IO.File.Delete(path);
                    oldADV.AdImages.ToList()[2].img = newADV.ImageUrl3;
                }


            }
            else
            {
                oldADV.AdImages.ToList()[2].img = newADV.ImageUrl3;
            }

            if (oldADV.AdImages.ToList()[3].img != newADV.ImageUrl4)
            {

                string[] arr = oldADV.AdImages.ToList()[3].img.Split('/');
                //string uploads = Path.Combine(_hosting.WebRootPath, @"uploads");

                //string fullPath = Path.Combine(uploads, arr[arr.Length - 1]);//   ----------/uploads/filename

                //oldADV.AdImages.ToList()[3].img = newADV.ImageUrl4;
                //File.Delete(fullPath);
                string uploadsFolder = Path.Combine(_hosting.WebRootPath, @"uploads");
                var path = System.IO.Path.Combine(Directory.GetCurrentDirectory(), uploadsFolder, arr[arr.Length - 1]);

                System.GC.Collect();
                System.GC.WaitForPendingFinalizers();

                if (System.IO.File.Exists(path))
                {
                    System.GC.Collect();
                    System.GC.WaitForPendingFinalizers();
                    System.IO.File.Delete(path);
                    oldADV.AdImages.ToList()[3].img = newADV.ImageUrl4;
                }


            }
            else
            {
                oldADV.AdImages.ToList()[3].img = newADV.ImageUrl4;
            }

            if (oldADV.AdImages.ToList()[4].img != newADV.ImageUrl5)
            {

                string[] arr = oldADV.AdImages.ToList()[4].img.Split('/');
                //string uploads = Path.Combine(_hosting.WebRootPath, @"uploads");

                //string fullPath = Path.Combine(uploads, arr[arr.Length - 1]);//   ----------/uploads/filename

                //oldADV.AdImages.ToList()[4].img = newADV.ImageUrl5;
                //File.Delete(fullPath);
                string uploadsFolder = Path.Combine(_hosting.WebRootPath, @"uploads");
                var path = System.IO.Path.Combine(Directory.GetCurrentDirectory(), uploadsFolder, arr[arr.Length - 1]);

                System.GC.Collect();
                System.GC.WaitForPendingFinalizers();

                if (System.IO.File.Exists(path))
                {
                    System.GC.Collect();
                    System.GC.WaitForPendingFinalizers();
                    System.IO.File.Delete(path);
                    oldADV.AdImages.ToList()[4].img = newADV.ImageUrl5;
                }



            }
            else
            {
                oldADV.AdImages.ToList()[4].img = newADV.ImageUrl5;
            }




            //oldADV.AdImages.ToList()[0].img = newADV.ImageUrl1;
            //oldADV.AdImages.ToList()[1].img = newADV.ImageUrl2;
            //oldADV.AdImages.ToList()[2].img = newADV.ImageUrl3;
            //oldADV.AdImages.ToList()[3].img = newADV.ImageUrl4;
            //oldADV.AdImages.ToList()[4].img = newADV.ImageUrl5;
            oldADV.CityID = newADV.CityID;
            oldADV.CategoryID = newADV.CategoryID;
            oldADV.Description = newADV.Description;








            _context.SaveChanges();

            return newADV;
        }
 
        public ShopViewModel Shop(ShopViewModel model)
        {
            if (model != null)
            {
                if (string.IsNullOrEmpty(model.userID) == false)
                {
                    model.Advertisments = _context.Advertisments.Where(c => c.IsPaid == false).Include(c => c.AdImages).Include(c => c.AnimalCategory).Include(c => c.City)
                   .Select(x => new AdsImagesVM2
                   {
                       AdID = x.AdID,
                       Title = x.Title,
                       CategoryID = x.CategoryID,
                       CityID = x.CityID,
                       CategoryName = x.AnimalCategory.CategoryName,
                       CityName = x.City.CityName,
                       Lang = Convert.ToDouble(x.City.Langtude),
                       Lat = Convert.ToDouble(x.City.Lantitude),
                       ImageUrl1 = x.AdImages.ToList()[0].img,
                       ImageUrl2 = x.AdImages.ToList()[1].img,
                       ImageUrl3 = x.AdImages.ToList()[2].img,
                       ImageUrl4 = x.AdImages.ToList()[3].img,
                       ImageUrl5 = x.AdImages.ToList()[4].img,
                       IsFav = _context.Favorites.Where(c => c.AdID == x.AdID && c.UserId == x.UserId).FirstOrDefault() == null ? false : true



                   }).ToList();
                }
                else
                {
                    model.Advertisments = _context.Advertisments.Where(c => c.IsPaid == false).Include(c => c.AdImages).Include(c => c.AnimalCategory).Include(c => c.City)
                  .Select(x => new AdsImagesVM2
                  {
                      AdID = x.AdID,
                      Title = x.Title,
                      CategoryID = x.CategoryID,
                      CityID = x.CityID,
                      CategoryName = x.AnimalCategory.CategoryName,
                      CityName = x.City.CityName,
                      Lang = Convert.ToDouble(x.City.Langtude),
                      Lat = Convert.ToDouble(x.City.Lantitude),
                      ImageUrl1 = x.AdImages.ToList()[0].img,
                      ImageUrl2 = x.AdImages.ToList()[1].img,
                      ImageUrl3 = x.AdImages.ToList()[2].img,
                      ImageUrl4 = x.AdImages.ToList()[3].img,
                      ImageUrl5 = x.AdImages.ToList()[4].img,
                      IsFav = false



                  }).ToList();
                }

            }

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
                if (model.CategoryId != 0)//categoryId=5 , CityId=-1
                {

                    if (model.CityId != 0 && model.CityId != -1)
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
                    if (model.CityId != 0 && model.CityId == -1)
                    {
                        model.Advertisments = model.Advertisments.ToList();
                    }
                    else if (model.CityId != 0)
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








        public List<AdsImagesVM2> GetNearestAdvertisments(double currentLatitude, double currentLongitude,
         int km, List<AdsImagesVM2> data)
        {

            List<AdsImagesVM2> advertsments = new List<AdsImagesVM2>();
            var query = (from c in data

                         select c).ToList();
            foreach (var ad in data)
            {
                double distance = Distance(currentLatitude, currentLongitude, Convert.ToDouble(ad.Lat), Convert.ToDouble(ad.Lang));
                if (distance <= km)         //nearbyplaces which are within 25 kms  50 w 70
                {
                    AdsImagesVM2 dist = new AdsImagesVM2();

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


        public bool AddToFav(int AdID, string userID)
        {
            var fav = _context.Favorites.SingleOrDefault(x => x.AdID == AdID && x.UserId == userID);
            Favorite favADv = new Favorite();
            if (fav == null)
            {
                favADv.AdID = AdID;
                favADv.UserId = userID;
                _context.Favorites.Add(favADv);
                _context.SaveChanges();
            }

            var advertismentCheck = _context.Favorites.Where(xx => xx.AdID == AdID && xx.UserId == userID).SingleOrDefault();
            if (advertismentCheck == null)
            {
                return false;
            }


            return true;

        }

        public List<favoriteViewModel> GetUserFavorites(string userID)
        {

            var ADS = _context.Favorites.Include(x => x.Advertisment).Include(x => x.Advertisment.AdImages).Include(x => x.Advertisment.AnimalCategory).Include(x => x.Advertisment.City).Where(x => x.UserId == userID).ToList();
            List<favoriteViewModel> adVMs = new List<favoriteViewModel>();
            foreach (var item in ADS)
            {
                adVMs.Add(new favoriteViewModel
                {
                    AdID = item.AdID,
                    CategoryName = item.Advertisment.AnimalCategory.CategoryName,
                    CityName = item.Advertisment.City.CityName,
                    Title = item.Advertisment.Title,
                    ImageUrl1 = item.Advertisment.AdImages.ToList()[0].img,
                    Description = item.Advertisment.Description
                });
            }

            return (adVMs);
        }

        public bool DeleteFav(string userID, int ADVid)
        {
            var favAD = _context.Favorites.FirstOrDefault(x => x.AdID == ADVid && x.UserId == userID);
            _context.Favorites.Remove(favAD);
            _context.SaveChanges();

            var check = _context.Favorites.FirstOrDefault(x => x.AdID == ADVid && x.UserId == userID);
            if (check == null)
            {
                return true;
            }
            return false;
        }

        public List<favoriteViewModel> GetAdvertismentsForIndex()
        {


            int pageNo = 1;
            int pageSize = 6;
            var advs = _context.Advertisments.OrderBy(p => p.AdID).Skip((pageNo - 1) * pageSize).Take(pageSize).Include(p => p.AnimalCategory).Include(xx => xx.AdImages).Where(x => x.IsPaid != true)
                .Select(xx => new favoriteViewModel
                {
                    CategoryName = xx.AnimalCategory.CategoryName,
                    Title = xx.Title,
                    ImageUrl1 = xx.AdImages.ToList()[0].img
                }).ToList();
            return advs;

        }

        public List<AdsImagesVm> GetAllAdvertisemtsData(string userID)
        {
            var ads = _context.Advertisments.Include(xx => xx.AdImages).Include(xx => xx.City).Include(xx => xx.AnimalCategory).Include(xx => xx.User).ToList();
            List<AdsImagesVm> adsImagesVms = new List<AdsImagesVm>();

            if (string.IsNullOrEmpty(userID) == false)
            {
                foreach (var item in ads)
                {
                    adsImagesVms.Add(new AdsImagesVm()
                    {
                        AdID = item.AdID,
                        CategoryID = item.CategoryID,
                        CityName = item.City.CityName,
                        Description = item.Description,
                        CategoryName = item.AnimalCategory.CategoryName,
                        CityID = item.CityID,
                        ImageUrl1 = item.AdImages.ToList()[0].img,
                        ImageUrl2 = item.AdImages.ToList()[1].img,
                        ImageUrl3 = item.AdImages.ToList()[2].img,
                        ImageUrl4 = item.AdImages.ToList()[3].img,
                        ImageUrl5 = item.AdImages.ToList()[4].img,
                        Title = item.Title,
                        UserId = item.UserId,
                        userPhone = item.User.Phone

                    });
                }
            }
            else
            {
                foreach (var item in ads)
                {
                    adsImagesVms.Add(new AdsImagesVm()
                    {
                        AdID = item.AdID,
                        CategoryID = item.CategoryID,
                        CityName = item.City.CityName,
                        Description = item.Description,
                        CategoryName = item.AnimalCategory.CategoryName,
                        CityID = item.CityID,
                        ImageUrl1 = item.AdImages.ToList()[0].img,
                        ImageUrl2 = item.AdImages.ToList()[1].img,
                        ImageUrl3 = item.AdImages.ToList()[2].img,
                        ImageUrl4 = item.AdImages.ToList()[3].img,
                        ImageUrl5 = item.AdImages.ToList()[4].img,
                        Title = item.Title,
                        UserId = item.UserId,
                        userPhone = item.User.Phone

                    });
                }
            }



            return adsImagesVms;
        }

        public favoriteViewModel GetAdvertisementByID(int ADid)
        {
            var ad = _context.Advertisments.Include(c => c.AdImages).Include(xx => xx.City).Include(xx => xx.AnimalCategory).FirstOrDefault(c => c.AdID == ADid);
            favoriteViewModel model = new favoriteViewModel()
            {
                AdID = ad.AdID,

                Description = ad.Description,
                Title = ad.Title,

                ImageUrl1 = ad.AdImages.ToList()[0].img,
                CategoryName = ad.AnimalCategory.CategoryName,
                CityName = ad.City.CityName

            };
            return model;
        }

        public List<favoriteViewModel> GetTopFiveFavs()
        {
            var results = from p in _context.Favorites
                          group p by p.AdID into g
                          select new { id = g.Key, total = g.Count() };
            var TOP = results.OrderByDescending(xx => xx.total).Take(5);
            List<favoriteViewModel> favoriteViewModel = new List<favoriteViewModel>();

            IAdverstisment aa = new AdvertisementServices(_context, _hosting);
            foreach (var item in TOP)
            {
                var adv = aa.GetAdvertisementByID(item.id);
                favoriteViewModel.Add(new favoriteViewModel
                {
                    AdID = adv.AdID,
                    CategoryName = adv.CategoryName,
                    CityName = adv.CityName,
                    ImageUrl1 = adv.ImageUrl1,
                    Title = adv.Title,
                    Description = adv.Description
                });


            }
            return favoriteViewModel;
        }

        //--------------------------------------------------------------------------------------------------------------

        public AdsImagesVM2 AddComment(CommentVM comment)
        {
            var com = new Comment();

            com.advID = comment.advID;
            com.CommentText = comment.CommentText;
            com.Date = DateTime.Now;
            com.Name = comment.Name;
            com.UserId = comment.UserId;

            _context.Comments.Add(com);
            _context.SaveChanges();

            return Details(comment.advID);
        }
    }
}
