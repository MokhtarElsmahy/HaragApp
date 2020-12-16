using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using HaragApp.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;

namespace HaragApp.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ResetPasswordModel : PageModel
    {
        private readonly UserManager<ApplicationDbUser> _userManager;
        private readonly ApplicationDbContext _context;
        public ResetPasswordModel(UserManager<ApplicationDbUser> userManager , ApplicationDbContext context)
        {
            _context = context;
            _userManager = userManager;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {


            [Required]
            [DataType(DataType.Password)]
            [Display(Name = "كلمة السر الحالية")]
            public string oldPass { get; set; }

            [Required]
            [DataType(DataType.Password)]
            [Display(Name = "كلمة السر الجديدة")]
            public string newPass { get; set; }


        }

        public IActionResult OnGet(string code = null)
        {
           
               return Page();
            
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Index","Home");
            }

            if(user.showpassword == Input.oldPass)
            {
                var changePasswordResult = await _userManager.ChangePasswordAsync(user, Input.oldPass, Input.newPass);
                user.showpassword = Input.newPass;
                await _userManager.UpdateAsync(user);
                await _context.SaveChangesAsync();
                
            }



            return RedirectToAction("Index", "Home");

            /*
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.FindByEmailAsync(Input.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToPage("./ResetPasswordConfirmation");
            }

            var result = await _userManager.ResetPasswordAsync(user, Input.Code, Input.Password);
            if (result.Succeeded)
            {
                return RedirectToPage("./ResetPasswordConfirmation");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return Page();*/
        }
    }
}
