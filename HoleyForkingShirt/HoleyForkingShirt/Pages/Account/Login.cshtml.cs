using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HoleyForkingShirt.Models;
using HoleyForkingShirt.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HoleyForkingShirt.Pages.Account
{
    public class LoginModel : PageModel
    {
        private SignInManager<ApplicationUser> _signInManager;

        [BindProperty]
        public LoginViewModel Input { get; set; }
        public LoginModel(SignInManager<ApplicationUser> signInManager)
        {
            _signInManager = signInManager;
        }
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPost()
        {
            if(ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(Input.Email, Input.Password, isPersistent:false, false);
                if(result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Invalid Login Attempt, Yo.");
                    return Page();
                }
            }
            return Page();
        }
    }
}
