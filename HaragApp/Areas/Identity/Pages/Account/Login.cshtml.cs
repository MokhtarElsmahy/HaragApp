using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using HaragApp.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace HaragApp.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        private readonly UserManager<ApplicationDbUser> _userManager;
        private readonly SignInManager<ApplicationDbUser> _signInManager;
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<LoginModel> _logger;

        public LoginModel(SignInManager<ApplicationDbUser> signInManager,
            ILogger<LoginModel> logger, ApplicationDbContext dbContext,
            UserManager<ApplicationDbUser> userManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _dbContext = dbContext;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public class InputModel
        {
            [Required]
            [MaxLength(9,ErrorMessage ="من فضلك ادخل رقم مكون من 9 ارقام")]
            public string Phone { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            returnUrl = returnUrl ?? Url.Content("~/");

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");

            if (ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                string phone = $"966{Input.Phone}";
                var user = _dbContext.Users.Where(c => c.Phone == phone ).FirstOrDefault();
                if (user == null)
                {
                    return Page();
                }
                // var result = await _signInManager.CheckPasswordSignInAsync(user,Input.Password,false);
                var result = await _signInManager.PasswordSignInAsync(user, Input.Password, Input.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    if (result.Succeeded)
                    {
                        #region MyRegion
                        var roles = await _userManager.GetRolesAsync(user);
                        if (roles.Contains("admin"))
                        {
                            _logger.LogInformation("User logged in.");
                            return LocalRedirect("/configs/index");
                        }

                        #endregion
                        _logger.LogInformation("User logged in.");
                        return LocalRedirect(returnUrl);
                    }
                    if (result.RequiresTwoFactor)
                    {
                        return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe });
                    }
                    if (result.IsLockedOut)
                    {
                        _logger.LogWarning("User account locked out.");
                        return RedirectToPage("./Lockout");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                        return Page();
                    }
                    //#region MyRegion
                    //var roles = await _userManager.GetRolesAsync(user);
                    //if (roles.Contains("admin"))
                    //{
                    //    _logger.LogInformation("User logged in.");
                    //    return LocalRedirect("/AnimalCategories/Create");
                    //}
                    //else
                    //{
                    //    _logger.LogInformation("User logged in.");
                    //    return LocalRedirect("/Advertisments/Create");
                    //}
                    //#endregion




                }
                if (result.RequiresTwoFactor)
                {
                    return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe });
                }
                if (result.IsLockedOut)
                {
                    _logger.LogWarning("User account locked out.");
                    return RedirectToPage("./Lockout");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return Page();
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
