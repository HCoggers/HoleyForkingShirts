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
        private UserManager<ApplicationUser> _userManager;

        [BindProperty]
        public LoginViewModel Input { get; set; }
        public LoginModel(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }
        public void OnGet()
        {
        }
        /// <summary>
        /// This is what we use to set the admin roles to specific user emails. 
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> OnPost()
        {
            if(ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(Input.Email, Input.Password, isPersistent:false, false);
                if(result.Succeeded)
                {
                    var user = _userManager.Users.Where(u => u.Email == Input.Email).FirstOrDefault();
                    if (!await _userManager.IsInRoleAsync(user, ApplicationRoles.Member))
                    {
                        await _userManager.AddToRoleAsync(user, ApplicationRoles.Member);
                        if(!await _userManager.IsInRoleAsync(user, ApplicationRoles.Admin))
                        {
                            switch (user.Email)
                            {
                                case "harry.cogswell@gmail.com":
                                case "splintercel3000@gmail.com":
                                case "amanda@codefellows.com":
                                case "rice.jonathanm@gmail.com":
                                case "revyolution1120@gmail.com":
                                    await _userManager.AddToRoleAsync(user, ApplicationRoles.Admin);
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                    if (!await _userManager.IsInRoleAsync(user, ApplicationRoles.Admin))
                        return RedirectToPage("/Admin/Dashboard");
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
