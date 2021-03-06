using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Text;
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
        private IEmailSender _emailSender;

        [BindProperty]
        public RegisterInput RegisterData { get; set; }

        public RegisterModel(UserManager<ApplicationUser> usermanager, SignInManager<ApplicationUser> signIn, ICartManager cartManager, IEmailSender emailSender)
        {
            _userManager = usermanager;
            _signInManager = signIn;
            _cartManager = cartManager;
            _emailSender = emailSender;
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
                    LastName = RegisterData.LastName,
                    BirthDate = RegisterData.Birthday
                };

                var result = await _userManager.CreateAsync(user, RegisterData.Password);

                if (result.Succeeded)
                {
                    // COLLECT CLAIMS
                    Claim fullName = new Claim(ClaimTypes.GivenName, $"{user.FirstName} {user.LastName}", ClaimValueTypes.String);
                    Claim birthday = new Claim(
                        ClaimTypes.DateOfBirth, 
                        new DateTime(user.BirthDate.Year, user.BirthDate.Month, user.BirthDate.Day).ToString("u"),
                        ClaimValueTypes.DateTime);
                    Claim email = new Claim(ClaimTypes.Email, user.Email, ClaimValueTypes.Email);

                    List<Claim> claims = new List<Claim> { fullName, birthday, email };

                    await _userManager.AddClaimsAsync(user, claims);

                    // ASSIGN ROLES
                    await _userManager.AddToRoleAsync(user, ApplicationRoles.Member);
                    switch(user.Email)
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

                    // Sign in user
                    await _signInManager.SignInAsync(user, isPersistent: false);

                    // SEND REGISTRATION EMAIL
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine($"<h1>Welcome to Holey Forking Shirts, {user.FirstName}!</h1>");
                    sb.AppendLine("<h2>You're account was registered successfully</h2>");
                    if (await _userManager.IsInRoleAsync(user, ApplicationRoles.Admin))
                    {
                        sb.AppendLine("<p>You have been given administrative privileges and access to admin-only pages</p>");
                        sb.AppendLine("<p>To access your Admin Dashboard, click <a href='https://holeyforkingshirts.azurewebsites.net/Admin'>HERE</a>");
                    } else
                    {
                        sb.AppendLine("<p>To start your E-thrifting adventure, click <a href='https://holeyforkingshirts.azurewebsites.net'>HERE</a>");
                    }

                    await _emailSender.SendEmailAsync(user.Email, "Welcome", sb.ToString());

                    // CREATE PERSONAL SHOPPING CART
                    Models.Cart cart = new Models.Cart
                    {
                        UserId = user.Id,
                        CartItems = new List<CartItems>()
                    };
                    await _cartManager.CreateCart(cart);

                    if (await _userManager.IsInRoleAsync(user, ApplicationRoles.Admin))
                        return RedirectToPage("/Admin/Dashboard");

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
