using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using HoleyForkingShirt.Models;
using HoleyForkingShirt.Models.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HoleyForkingShirt.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private UserManager<ApplicationUser> _userManager;
        private SignInManager<ApplicationUser> _signInManager;
        private ICartManager _cartManager;

        [BindProperty]
        public RegisterInput RegisterData { get; set; }

        public RegisterModel(UserManager<ApplicationUser> usermanager, SignInManager<ApplicationUser> signIn, ICartManager cartManager)
        {
            _userManager = usermanager;
            _signInManager = signIn;
            _cartManager = cartManager;
        }

        public void OnGet()
        {
        }
        /// <summary>
        /// This is our post method for register. It makes a user and makes the claims for the user. 
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> OnPost()
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = RegisterData.Email,
                    Email = RegisterData.Email,
                    FirstName = RegisterData.FirstName,
                    LatName = RegisterData.LastName,
                    BirthDate = RegisterData.Birthday
                };

                var result = await _userManager.CreateAsync(user, RegisterData.Password);

                if (result.Succeeded)
                {

                    Claim fullName = new Claim(ClaimTypes.GivenName, $"{user.FirstName} {user.LatName}", ClaimValueTypes.String);
                    Claim birthday = new Claim(
                        ClaimTypes.DateOfBirth, 
                        new DateTime(user.BirthDate.Year, user.BirthDate.Month, user.BirthDate.Day).ToString("u"),
                        ClaimValueTypes.DateTime);
                    Claim email = new Claim(ClaimTypes.Email, user.Email, ClaimValueTypes.Email);

                    List<Claim> claims = new List<Claim> { fullName, birthday, email };

                    await _userManager.AddClaimsAsync(user, claims);
                    await _signInManager.SignInAsync(user, isPersistent: false);


                    Models.Cart cart = new Models.Cart
                    {
                        UserId = user.Id,
                        CartItems = new List<CartItems>()
                    };
                    await _cartManager.CreateCart(cart);

                     return RedirectToAction("Index", "Home");
                }

                foreach(var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return Page();
        }
        /// <summary>
        /// This is what is required to register on our page as a user. 
        /// </summary>
        public class RegisterInput
        {
            [Required]
            [EmailAddress]
            [Display(Name = "Email Address")]
            public string Email { get; set; }

            [Required]
            [Display(Name = "First Name")]
            public string FirstName { get; set; }

            [Required]
            [Display(Name = "Last Name")]
            public string LastName { get; set; }

            [Required]
            [DataType(DataType.Date)]
            public DateTime Birthday { get; set; }

            [Required]
            [StringLength(15, ErrorMessage ="The {0} must be at least {2} and a maximum {1} characters long", MinimumLength = 7)]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Required]
            [DataType(DataType.Password)]
            [Compare("Password", ErrorMessage = "The passwords don't match. Just like the products we sell.")]
            public string ConfirmPassword { get; set; }

        }
    }
}
