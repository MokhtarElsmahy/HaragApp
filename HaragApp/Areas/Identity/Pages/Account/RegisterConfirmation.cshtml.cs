using Microsoft.AspNetCore.Authorization;
using System.Text;
using System.Threading.Tasks;
using HaragApp.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using HaragApp.ViewModels;
using System.ComponentModel.DataAnnotations;

namespace HaragApp.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterConfirmationModel : PageModel
    {
        private readonly UserManager<ApplicationDbUser> _userManager;
        private readonly IEmailSender _sender;
        private readonly SignInManager<ApplicationDbUser> _signInManager;
        public RegisterConfirmationModel(UserManager<ApplicationDbUser> userManager, IEmailSender sender, SignInManager<ApplicationDbUser> signInManager)
        {
            _userManager = userManager;
            _sender = sender; 
            _signInManager = signInManager;

        }

        [Required(ErrorMessage ="ادخل الكود")]
        public string code { get; set; }
        public string UserID { get; set; }


        [BindProperty]
        public string PhoneNumber { get; set; }




        public bool DisplayConfirmAccountLink { get; set; }

        public string EmailConfirmationUrl { get; set; }


        public async Task<IActionResult> OnGetAsync(RegisterConModel model)
        {
            //var user = await _userManager.GetUserAsync(User);
            
            PhoneNumber = model.Phone;
            UserID = model.UserID;
           
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string code , string UserID)
        {

            //var user = await _userManager.GetUserAsync(User);
            var user = await _userManager.FindByIdAsync(UserID);
        
            if (user.code == int.Parse(code) && user.IsActive == false)
            {
                user.IsActive = true;
                await _userManager.UpdateAsync(user);
                await _signInManager.SignInAsync(user, isPersistent: false);
                return LocalRedirect("/Home/Index");
            }
            if (user.code == int.Parse(code) && user.IsActive == true)
            {
                return RedirectToPage("./ResetPassword");
            }

            return Page();
        }


    }
}
