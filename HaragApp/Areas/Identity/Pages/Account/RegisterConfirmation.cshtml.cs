using Microsoft.AspNetCore.Authorization;
using System.Text;
using System.Threading.Tasks;
using HaragApp.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;

namespace HaragApp.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterConfirmationModel : PageModel
    {
        private readonly UserManager<ApplicationDbUser> _userManager;
        private readonly IEmailSender _sender;

        public RegisterConfirmationModel(UserManager<ApplicationDbUser> userManager, IEmailSender sender)
        {
            _userManager = userManager;
            _sender = sender;
        }
     
        public string code { get; set; }


        [BindProperty]
        public string PhoneNumber { get; set; }

       


        public bool DisplayConfirmAccountLink { get; set; }

        public string EmailConfirmationUrl { get; set; }


        public async Task<IActionResult> OnGetAsync(string code)
        {
            var user = await _userManager.GetUserAsync(User); 
            PhoneNumber = user.Phone;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string code)
        {

            var user = await _userManager.GetUserAsync(User);
            if (user.code == int.Parse(code) && user.IsActive==false)
            {
                user.IsActive = true;
                await _userManager.UpdateAsync(user);
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
