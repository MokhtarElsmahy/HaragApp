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
using Microsoft.AspNetCore.Identity;
using HaragApp.ViewModels;

namespace HaragApp.Controllers.api
{
    [Route("api/[controller]")]
    [ApiController]
    public class FavoritesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationDbUser> _userManager;

        public FavoritesController(ApplicationDbContext context, UserManager<ApplicationDbUser> userManager)
        {
            _context = context;
            _userManager = userManager;

        }

        // GET: api/Favorites
        [HttpGet]
        public async Task<ActionResult<List<favoriteViewModel>>> GetFavorites(string userID)
        {
            var user = await _userManager.GetUserAsync(User);
            IAdverstisment adverstisment = new AdvertisementServices(_context);
            if (string.IsNullOrEmpty(userID) == true)
            {
                var adVMs = adverstisment.GetUserFavorites(user.Id.ToString());

                return adVMs;
            }
            else
            {
                var adVMs = adverstisment.GetUserFavorites(userID);

                return adVMs;
            }
          
        }

        

        

        // POST: api/Favorites
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<bool> PostFavorite(int adID, string userID)
        {
            IAdverstisment adverstisment = new AdvertisementServices(_context);
          
            if (string.IsNullOrEmpty(userID) == true)
            {
                var user = await _userManager.GetUserAsync(User);
                var check = adverstisment.AddToFav(adID, user.Id);
                return check;
            }
            else
            {
               
                var check = adverstisment.AddToFav(adID, userID);
                return check;
            }
          
           
        }

        // DELETE: api/Favorites/5
        [HttpDelete("{id}")]
        public async Task<bool> DeleteFavorite(int id ,string userID)
        {
            if (string.IsNullOrEmpty(userID)==true)
            {
                var user = await _userManager.GetUserAsync(User);
                IAdverstisment adverstisment = new AdvertisementServices(_context);
                var check = adverstisment.DeleteFav(user.Id, id);
                return check;
            }
            else
            {
                
                IAdverstisment adverstisment = new AdvertisementServices(_context);
                var check = adverstisment.DeleteFav(userID, id);
                return check;
            }
          

            
        }

        
    }
}
