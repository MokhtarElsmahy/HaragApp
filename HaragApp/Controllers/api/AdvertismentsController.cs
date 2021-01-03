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

namespace HaragApp.Controllers.api
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdvertismentsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationDbUser> _userManager;

        public AdvertismentsController(ApplicationDbContext context, UserManager<ApplicationDbUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: api/Advertisments
        //[HttpGet]
        //public  ActionResult<List<AdsImagesVm>> GetAdvertisments(string userID)
        //{
        //    IAdverstisment ads = new AdvertisementServices(_context);
           
        //    return ads.GetAllAdvertisemtsData(userID) ;
        //}

        // GET: api/Advertisments/5
        [HttpGet("{id}")]
        public ActionResult<AdsImagesVm> GetAdvertisment(int id)
        {
           
            //var advertisment = await _context.Advertisments.FindAsync(id);
            IAdverstisment ads = new AdvertisementServices(_context);
            var advertisment = ads.Details(id);
            if (advertisment == null)
            {
                return NotFound();
            }

            return advertisment;
        }

       
      

        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut]
        public async Task<IActionResult> PutAdvertisment(AdsImagesVm advertisment)
        {
          
            IAdverstisment ads = new AdvertisementServices(_context);
            ads.UpdateAsync(advertisment);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                
            }

            return NoContent();
        }

    
        [HttpPost]
        public async Task<ActionResult<Advertisment>> PostAdvertisment(AdsImagesVm advertisment)
        {
            IAdverstisment ads = new AdvertisementServices(_context);
            ads.CreateAsync(advertisment);
            
            return CreatedAtAction("GetAdvertisment", new { id = advertisment.AdID }, advertisment);
        }

        // DELETE: api/Advertisments/5
        [HttpDelete("{id}")]
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
