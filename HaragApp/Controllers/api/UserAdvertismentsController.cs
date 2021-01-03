using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HaragApp.Data;
using HaragApp.Models;
using HaragApp.Component.Interfaces;
using HaragApp.Component.Services;
using HaragApp.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace HaragApp.Controllers.api
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserAdvertismentsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationDbUser> _userManager;

        public UserAdvertismentsController(ApplicationDbContext context, UserManager<ApplicationDbUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: api/UserAdvertisments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AdsImagesVm>>> GetAdvertisments(string userID)
        {
          
            if (string.IsNullOrEmpty(userID))
            {
                var user = await _userManager.GetUserAsync(User);
                IAdverstisment ads = new AdvertisementServices(_context);
                var advList = ads.GetUserAdvertisementsAsync(user.Id);

                return advList;
            }
            else
            {
               
                IAdverstisment ads = new AdvertisementServices(_context);
                var advList = ads.GetUserAdvertisementsAsync(userID);

                return advList;
            }
           
        }

        // GET: api/UserAdvertisments/5
        //[HttpGet("{id}")]
        //public async Task<ActionResult<Advertisment>> GetAdvertisment(int id)
        //{
        //    var advertisment = await _context.Advertisments.FindAsync(id);

        //    if (advertisment == null)
        //    {
        //        return NotFound();
        //    }

        //    return advertisment;
        //}

        // PUT: api/UserAdvertisments/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        //[HttpPut]
        //public AdsImagesVm PutAdvertisment(AdsImagesVm advertisment)
        //{
           
        //    IAdverstisment adverstisment = new AdvertisementServices(_context);
        //    var adv = adverstisment.userUpdateADV(advertisment.AdID, advertisment);


        //    return adv;
        //}

  
        //[HttpPost]
        //public async Task<ActionResult<Advertisment>> PostAdvertisment(Advertisment advertisment)
        //{
        //    _context.Advertisments.Add(advertisment);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction("GetAdvertisment", new { id = advertisment.AdID }, advertisment);
        //}

        // DELETE: api/UserAdvertisments/5
        //[HttpDelete("{id}")]
        //public bool DeleteAdvertisment(int id)
        //{
        //    IAdverstisment ads = new AdvertisementServices(_context);
        //    var boolCheck = ads.userDeleteADV(id);

        //    return boolCheck;
        //}
   
        private bool AdvertismentExists(int id)
        {
            return _context.Advertisments.Any(e => e.AdID == id);
        }

       
    }
}
