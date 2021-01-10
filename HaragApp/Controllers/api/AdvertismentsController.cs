using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HaragApp.Data;
using HaragApp.Models;
using HaragApp.ViewModels;
using HaragApp.Component.Interfaces;
using HaragApp.Component.Services;
using Microsoft.AspNetCore.Identity;
using RestSharp;
using System.Net;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.FileProviders;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace HaragApp.Controllers.api
{
   
    [Route("api/[controller]")]
    //[ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class AdvertismentsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationDbUser> _userManager;
        private IHostingEnvironment _hosting { get; set; }
        public AdvertismentsController(ApplicationDbContext context, IHostingEnvironment hosting, UserManager<ApplicationDbUser> userManager)
        {
            _context = context;
            _userManager = userManager;
            _hosting = hosting;
        }

        // GET: api/Advertisments
        //[HttpGet]
        //public  ActionResult<List<AdsImagesVm>> GetAdvertisments(string userID)
        //{
        //    IAdverstisment ads = new AdvertisementServices(_context);

        //    return ads.GetAllAdvertisemtsData(userID) ;
        //}

        // GET: api/Advertisments/5
        [AllowAnonymous]
        [HttpGet("{id}")]
        public ActionResult<AdsImagesVM2> GetAdvertisment(int id)
        {
           
            //var advertisment = await _context.Advertisments.FindAsync(id);
            IAdverstisment ads = new AdvertisementServices(_context, _hosting);
            var advertisment = ads.Details(id);
            if (advertisment == null)
            {
                return NotFound();
            }

            return advertisment;
        }



        [AllowAnonymous]
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut]
        public async Task<IActionResult> PutAdvertisment(AdsImagesVm advertisment)
        {
           

            IAdverstisment ads = new AdvertisementServices(_context, _hosting);
            ads.UpdateAsync(advertisment);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                
            }
            AdsImagesVM2 model = new AdsImagesVM2()
            {
                AdID = advertisment.AdID,
                CategoryID = advertisment.CategoryID,
                Title = advertisment.Title,
                CityID = advertisment.CityID,
                IsPaid = advertisment.IsPaid,
                IsPact = advertisment.IsPact,
                IsFav = advertisment.IsFav,
                UserId = advertisment.UserId,
                Description = advertisment.Description,
                ImageUrl1 = advertisment.ImageUrl1,
                ImageUrl2 = advertisment.ImageUrl2,
                ImageUrl3 = advertisment.ImageUrl3,
                ImageUrl4 = advertisment.ImageUrl4,
                ImageUrl5 = advertisment.ImageUrl5
            };

            return CreatedAtAction("GetAdvertisment", new { id = advertisment.AdID }, model);
           // return NoContent();
        }

    
        [AllowAnonymous]
        [HttpPost, DisableRequestSizeLimit]
        public async Task<ActionResult<AdsImagesVm>> PostAdvertisment(AdsImagesVm advertisment)
        {
            if (advertisment.Files != null)
            {
                for (int i = 0; i < advertisment.Files.Count; i++)
                {
                    //string result = "defaultRecImage.png";

                    try
                    {

                        string uploads = Path.Combine(_hosting.WebRootPath, @"uploads");
                        var filename = ContentDispositionHeaderValue.Parse(advertisment.Files[i].ContentDisposition).FileName.Trim('"');
                        var newFileName = Guid.NewGuid() + filename;
                        string fullPath = Path.Combine(uploads, newFileName);
                        advertisment.Files[i].CopyTo(new FileStream(fullPath, FileMode.Create));
                        if (i==0)
                        {
                            advertisment.ImageUrl1 = $"/uploads/{newFileName}";
                        }
                        if (i == 1)
                        {
                            advertisment.ImageUrl2 = $"/uploads/{newFileName}";
                        }
                        if (i == 2)
                        {
                            advertisment.ImageUrl3 = $"/uploads/{newFileName}";
                        }
                        if (i == 3)
                        {
                            advertisment.ImageUrl4 = $"/uploads/{newFileName}";
                        }
                        if (i == 4)
                        {
                            advertisment.ImageUrl5 = $"/uploads/{newFileName}";
                        }

                    }
                    catch (Exception ex)
                    {
                       

                    }
                }
              
            }
           

            IAdverstisment ads = new AdvertisementServices(_context, _hosting);
            ads.CreateAsync(advertisment);
            AdsImagesVM2 model = new AdsImagesVM2()
            {
                AdID = advertisment.AdID,
                CategoryID = advertisment.CategoryID,
                Title = advertisment.Title,
                CityID = advertisment.CityID,
                IsPaid = advertisment.IsPaid,
                IsPact = advertisment.IsPact,
                IsFav = advertisment.IsFav,
                Description = advertisment.Description,
                ImageUrl1 = advertisment.ImageUrl1,
                ImageUrl2 = advertisment.ImageUrl2,
                ImageUrl3 = advertisment.ImageUrl3,
                ImageUrl4 = advertisment.ImageUrl4,
                ImageUrl5 = advertisment.ImageUrl5, 
                UserId=advertisment.UserId
            };
          
            return CreatedAtAction("GetAdvertisment", new { id = advertisment.AdID }, model);
        }

        // DELETE: api/Advertisments/5
        [HttpDelete("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<Advertisment>> DeleteAdvertisment(int id)
        {
            var advertisment = await _context.Advertisments.FindAsync(id);
            if (advertisment == null)
            {
                return NotFound();
            }

            _context.Advertisments.Remove(advertisment);
            await _context.SaveChangesAsync();

            return advertisment;
        }

        private bool AdvertismentExists(int id)
        {
            return _context.Advertisments.Any(e => e.AdID == id);
        }
    }
}
