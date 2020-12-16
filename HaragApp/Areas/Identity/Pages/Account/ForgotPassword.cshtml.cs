using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using HaragApp.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using RestSharp;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace HaragApp.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ForgotPasswordModel : PageModel
    {
        private readonly UserManager<ApplicationDbUser> _userManager;
        private readonly IEmailSender _emailSender;
        private readonly ApplicationDbContext _context;
        private readonly SignInManager<ApplicationDbUser> _signInManager;
        private readonly ILogger<LoginModel> _logger;

        public ForgotPasswordModel(ILogger<LoginModel> logger, SignInManager<ApplicationDbUser> signInManager, UserManager<ApplicationDbUser> userManager, IEmailSender emailSender , ApplicationDbContext context)
        {
            _userManager = userManager;
            _emailSender = emailSender;
            _context = context;
            _signInManager = signInManager;
            _logger = logger;

        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel { 
        
            [Required]
            public string Phone { get; set; }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var user = _context.Users.FirstOrDefault(c=>c.Phone==Input.Phone);
                if (user == null)
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return Page();
                }
                if (user.Phone != Input.Phone)
                {
                    return Page();
                }
                else
                {
                    var x = SendMessage(user.code.ToString(), user.Phone.ToString());
                    var result = await _signInManager.PasswordSignInAsync(user, user.showpassword, true, lockoutOnFailure: false);
                    if (result.Succeeded)
                    {
                        _logger.LogInformation("User logged in.");
                        return RedirectToPage("./RegisterConfirmation");
                    }
                }




            }

            return Page();
        }
        public static async Task<string> SendMessage(string msg, string numbers)
        {
            var client = new RestClient($"http://api.yamamah.com/SendSMS");
            var request = new RestRequest(Method.POST);
            request.AddJsonBody(new
            {
                Username = "966532866666",
                Password = "Ht5pTY26",
                Tagname = "Haraajm",
                RecepientNumber = numbers,
                Message = msg

            });
            IRestResponse response = await client.ExecuteAsync(request);

            return "";
        }
        public static int GetFormNumber()
        {
            Random rnd = new Random();
            return rnd.Next();
        }
    }
}
