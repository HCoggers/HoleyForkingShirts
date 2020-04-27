using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using HoleyForkingShirt.Models;
using HoleyForkingShirt.Models.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HoleyForkingShirt.Pages.Cart
{
    public class CartModel : PageModel
    {
        private ICartManager _cartManager; 
        public List<CartItems> InCart;
        private SignInManager<ApplicationUser> _signInManager;
        private UserManager<ApplicationUser> _userManager;
        public CartModel(ICartManager cart, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
        {
            _cartManager = cart;
            _signInManager = signInManager;
            _userManager = userManager;
        }
        public async Task<IActionResult> OnGetAsync()
        {
            var signedIn = _signInManager.IsSignedIn(User);
            if (signedIn)
            {
                Models.Cart cart = await _cartManager.GetCart(User.Claims
                    .First(c => c.Type == ClaimTypes.Email)
                    .ToString());
                InCart = cart.cartItems;
            }

            return Page();
        }
    }
}
