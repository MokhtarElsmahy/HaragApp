﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using HaragApp.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using RestSharp;
using System.Net;
using System.Net.Http;
using HaragApp.ViewModels;

namespace HaragApp.Areas.Identity.Pages.Account
{
    //[Authorize(Roles ="admin")]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<ApplicationDbUser> _signInManager;
        private readonly UserManager<ApplicationDbUser> _userManager;
        private readonly ApplicationDbContext _ctx;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;

        public RegisterModel(
            UserManager<ApplicationDbUser> userManager,
            SignInManager<ApplicationDbUser> signInManager,
            ILogger<RegisterModel> logger, ApplicationDbContext ctx,
            IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _ctx = ctx;
        }


        public bool IsAdmin { get; set; } = false;

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }
        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
          
            [Display(Name = "Phone")]
            [Required]
            [MaxLength(9, ErrorMessage = "من فضلك ادخل رقم مكون من 9 ارقام")]
            public string Phone { get; set; }


            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
            public bool IsHelper { get; set; }

        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;


            //var user = await _userManager.GetUserAsync(User);
            //if (_userManager.IsInRoleAsync(user,"admin").Result)
            //{
            //    IsAdmin = true;
            //}
            var user = await _userManager.GetUserAsync(User);
            if (user!=null)
            {
                var roles = await _userManager.GetRolesAsync(user);

                if (roles.Contains("admin"))
                {
                    IsAdmin = true;

                }
            }
           
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                string phone = $"966{Input.Phone}";
                var user = new ApplicationDbUser { UserName = $"{phone}user.com", Email = $"{phone}user.com", Phone= phone, CityID=1,showpassword=Input.Password};
                var result = await _userManager.CreateAsync(user, Input.Password);

                var Loginguser = await _userManager.GetUserAsync(User);
                if (Loginguser != null)
                {
                    var roles = await _userManager.GetRolesAsync(Loginguser);
                    if (roles.Contains("admin") && Input.IsHelper == true)
                    {
                        await _userManager.AddToRoleAsync(user, "admin");
                        _ctx.SaveChanges();
                    }
                }
             
                if (result.Succeeded)
                {
                    var code = GetFormNumber().ToString();
                    string smallCode = code.Substring(0, 4);
                    _logger.LogInformation("User created a new account with password.");
                    //await _signInManager.SignInAsync(user, isPersistent: false);
                   var x = await  SendMessage(smallCode, user.Phone);
                    user.code =int.Parse(smallCode);
                    await _userManager.UpdateAsync(user);
                    _ctx.SaveChanges();
                  
                    RegisterConModel model = new RegisterConModel() { UserID = user.Id, Phone= user.Phone, IsCreated = result.Succeeded };
                    return RedirectToPage("RegisterConfirmation",model);




                    ////await _userManager.AddToRoleAsync(user, "admin");

                    ////var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    ////code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    ////var callbackUrl = Url.Page(
                    ////    "/Account/ConfirmEmail",
                    ////    pageHandler: null,
                    ////    values: new { area = "Identity", userId = user.Id, code = code, returnUrl = returnUrl },
                    ////    protocol: Request.Scheme);

                    ////await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                    ////    $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    ////if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    ////{
                    ////    return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    ////}
                    ////else
                    ////  {
                    //await _signInManager.SignInAsync(user, isPersistent: false);
                    ////return LocalRedirect(returnUrl);
                    //return RedirectToAction("Create", "AnimalCategories");
                    ////}
                    ///

                    //_logger.LogInformation("User created a new account with password.");

                    //var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    //code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    //var callbackUrl = Url.Page(
                    //    "/Account/ConfirmEmail",
                    //    pageHandler: null,
                    //    values: new { area = "Identity", userId = user.Id, code = code, returnUrl = returnUrl },
                    //    protocol: Request.Scheme);

                    //await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                    //    $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    //if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    //{
                    //    return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    //}
                    //else
                    //{
                    //    await _signInManager.SignInAsync(user, isPersistent: false);
                    //    return LocalRedirect(returnUrl);
                    //}


                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }

        public static async Task<string> SendMessage(string msg, string numbers)
        {
            var client = new RestClient($"http://api.yamamah.com/SendSMS");
            var request = new RestRequest(Method.POST);
            request.AddJsonBody(new
            {
                Username = "966532866666",
                Password = "Ahmed05328",
                Tagname = "Haraajm",
                RecepientNumber = numbers,
                Message = $"Your Verification Code for SouqElMawashi is : {msg}"

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
