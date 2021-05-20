using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HaragApp.Component.Interfaces;
using HaragApp.Component.Services;
using HaragApp.Data;
using HaragApp.PathUrl;
using HaragApp.ViewModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace HaragApp.Controllers.api
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]


    public class ClientAppController : BaseController
    {
        private readonly UserManager<ApplicationDbUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly SignInManager<ApplicationDbUser> _signInManager;
        private readonly ApplicationDbContext db;
        private readonly IHostingEnvironment HostingEnvironment;
        public ClientAppController(ApplicationDbContext _db, IHostingEnvironment HostingEnvironment, UserManager<ApplicationDbUser> userManager, SignInManager<ApplicationDbUser> signInManager, IConfiguration configuration)
            : base(_db, HostingEnvironment, userManager, signInManager, configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
            _signInManager = signInManager;
            db = _db;
            this.HostingEnvironment = HostingEnvironment;
        }
        public IActionResult Index()
        {
            return View();
        }
        //[AllowAnonymous]
        //[HttpPost(ApiRoutes.setting.PostAdvertisment)]
        //public async Task<ActionResult<AdsImagesVm>> PostAdvertisment(AdsImagesVm advertisment)
        //{
         

        //    return CreatedAtAction("GetAdvertisment", new { id = 0 }, advertisment);
        //}
        [AllowAnonymous]
        [HttpPost(ApiRoutes.setting.ContactUs)]
        public ActionResult ContactUs()
        {
            try
            {
                var data = db.Configs.Select(x => new
                {
                    mobile1=x.Mobile1,
                    mobile2=x.Mobile2,
                    email=x.Email

                }).FirstOrDefault();
                return Json(new
                {
                    data
                });



            }
            catch (Exception ex)
            {
                return Json(new
                {
                    key = 0,
                    msg = ex.Message
                });
            }

        }


        [AllowAnonymous]
        [HttpPost(ApiRoutes.setting.AboutUs)]
        public ActionResult AboutUs()
        {
            try
            {
                var data = db.Configs.Select(x => new
                {
                    about = x.about

                }).FirstOrDefault();


                return Json(new
                {
                    data
                });



            }
            catch (Exception ex)
            {
                return Json(new
                {
                    key = 0,
                    msg = ex.Message
                });
            }

        }


        [AllowAnonymous]
        [HttpPost(ApiRoutes.setting.GetAllPaidAdv)]
        public IActionResult GetAllPaidAdv()
        {
            IAdverstisment dd = new AdvertisementServices(db, HostingEnvironment);
            var d = dd.GetAllPaidAdv();
            return Json(new { data = d });
        }

        [AllowAnonymous]
        [HttpPost(ApiRoutes.setting.Shop)]
        public async Task<IActionResult> Shop(ShopViewModel model)
        {
            string ID;
            if (string.IsNullOrEmpty(model.userID))
            {
               ID = HttpContext.Session.GetString("ID");

            }
            else
            {
                ID = model.userID;
            }

            model.PageNo = model.PageNo ?? 1;
            IAdverstisment dd = new AdvertisementServices(db, HostingEnvironment);
            //var user = await _userManager.GetUserAsync(User);
            //string UserID = string.Empty;
            //if (user != null)
            //{
            //    UserID = user.Id;
            //}
            //model.userID = UserID;
            model.userID = ID;
            var d = dd.Shop(model);
            
            return Json(new
            {
                advertisments = d.Advertisments,
                Cities = d.Cities,
                Categories = d.Categories,
                search = d.search,
                pageNo = d.PageNo,
                AllAdsCount = d.AllAdsCount,
                CategoryId = d.CategoryId,
                CityId = d.CityId,
                Km = d.Km,
                Lang = d.Lang,
                Lat = d.Lat,
                NumberOFDisplayedAds = d.Advertisments.Count()
            });
        }


        [AllowAnonymous]
        [HttpPost(ApiRoutes.setting.addComment)]
        public IActionResult addComment(CommentVM comment)
        {
            if (!String.IsNullOrEmpty(comment.UserId))
            {
                IAdverstisment ads = new AdvertisementServices(db, HostingEnvironment);
                ads.AddComment(comment);

                var allComments = db.Comments.Select(x => new CommentVM
                {
                    advID = x.advID,
                    Name = x.Name,
                    CommentText = x.CommentText,
                    UserId = x.UserId,
                    Date = x.Date,
                    CommentID = x.CommentID                    
                }).Where(x => x.advID == comment.advID).ToList();
                return Json(new
                {
                    data = allComments
                }
               );

            }
            else
            {
                return Json(new
                {
                    data = "برجاء تسجيل الدخول"
                });
            }          
        }


        [AllowAnonymous]
        [HttpGet(ApiRoutes.setting.GetCities)]
        public IActionResult GetCities()
        {
            IAdverstisment dd = new AdvertisementServices(db, HostingEnvironment);
            var Cities = dd.GetAllCities();
            return Json(new { Cities = Cities });
        }


        //[AllowAnonymous]
        //[HttpPost(ApiRoutes.setting.Condtions)]
        //public ActionResult Condtions(string lang = "ar")
        //{
        //    try
        //    {


        //        //var data = db.Setting.Select(x => new
        //        //{

        //        //    text = lang == "ar" ? x.Condtions_ar_client : x.Condtions_en_client

        //        //}).FirstOrDefault();


        //        return Json(new
        //        {
        //            key = 1,
        //            //  data
        //        });



        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new
        //        {
        //            key = 0,
        //            msg = ex.Message
        //        });
        //    }

        //}

        //[AllowAnonymous]
        //[HttpPost(ApiRoutes.setting.GetSetting)]
        //public ActionResult GetSetting(string phone, string lang = "ar")
        //{
        //    try
        //    {


        //        //var data = db.Setting.Select(x => new
        //        //{

        //        //    aboutUs_client = lang == "ar" ? x.aboutUs_ar_client : x.aboutUs_en_client,
        //        //    aboutUs_delegt = lang == "ar" ? x.aboutUs_ar_delegt : x.aboutUs_en_delegt,

        //        //    Condtions_client = lang == "ar" ? x.Condtions_ar_client : x.Condtions_en_client,
        //        //    Condtions_delegt = lang == "ar" ? x.Condtions_ar_delegt : x.Condtions_en_delegt,


        //        //    text1_client = lang == "ar" ? x.text1_ar_client : x.text1_en_client,
        //        //    text2_client = lang == "ar" ? x.text2_ar_client : x.text2_en_client,
        //        //    text3_client = lang == "ar" ? x.text3_ar_client : x.text3_en_client,
        //        //    // text_client = lang == "ar" ? x.text_ar_client : x.text_en_client,

        //        //    text1_delegt = lang == "ar" ? x.text1_ar_delegt : x.text1_en_delegt,
        //        //    text2_delegt = lang == "ar" ? x.text2_ar_delegt : x.text2_en_delegt,
        //        //    text3_delegt = lang == "ar" ? x.text3_ar_delegt : x.text3_en_delegt,
        //        //    //  text_delegt = lang == "ar" ? x.text_ar_delegt : x.text_en_delegt,
        //        //    x.twitter,
        //        //    x.phone,
        //        //    x.location,
        //        //    x.key_map,
        //        //    x.instgram,
        //        //    x.facebook,
        //        //    x.bank_account,
        //        //    x.bank_account2,
        //        //    x.bank_account_name,
        //        //    x.bank_account_name2

        //        //}).FirstOrDefault();


        //        return Json(new
        //        {
        //            key = 1,
        //            // data
        //        });



        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new
        //        {
        //            key = 0,
        //            msg = ex.Message
        //        });
        //    }

        //}
        //[HttpPost(ApiRoutes.setting.GetQAndAnswer)]
        //public ActionResult GetQAndAnswer(string lang = "ar")
        //{
        //    try
        //    {

        //        //var data = (from st in db.QAndAnswer
        //        //            where st.type == 1
        //        //            select new
        //        //            {
        //        //                st.id,
        //        //                question = lang == "ar" ? st.question : st.questionEn,
        //        //                answer = lang == "ar" ? st.answer : st.answerEn,
        //        //                opend = false
        //        //            }).ToList();

        //        return Json(new
        //        {
        //            key = 1,
        //            // data
        //        });



        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new
        //        {
        //            key = 0,
        //            msg = ex.Message
        //        });
        //    }


        //}
        ////[HttpPost(ApiRoutes.setting.Addcomplaints)]
        ////public ActionResult Addcomplaints(Complaints_model complaints, string lang = "")
        ////{
        ////    try
        ////    {
        ////        Complaints complaints1 = new Complaints();
        ////        complaints1.email = complaints.email;
        ////        complaints1.name = complaints.name;
        ////        complaints1.text = complaints.text;
        ////        db.Complaints.Add(complaints1);
        ////        db.SaveChanges();

        ////        return Json(new { key = 1, msg = creatMessage(lang, "تم الارسال بنجاح", "Send successfully") });



        ////    }
        ////    catch (Exception ex)
        ////    {
        ////        return Json(new
        ////        {
        ////            key = 0,
        ////            msg = ex.Message
        ////        });
        ////    }

        ////}



    }
}
