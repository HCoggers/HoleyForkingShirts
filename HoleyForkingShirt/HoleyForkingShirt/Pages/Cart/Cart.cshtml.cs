using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using HoleyForkingShirt.Data;
using HoleyForkingShirt.Models;
using HoleyForkingShirt.Models.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Internal;

namespace HoleyForkingShirt.Pages.Cart
{
    [Authorize]
    public class CartModel : PageModel
    {
        private ICartManager _cartManager; 
        private SignInManager<ApplicationUser> _signInManager;
        private UserManager<ApplicationUser> _userManager;

        public List<CartItems> InCart;
        public decimal Total;

        public CartModel(ICartManager cartManager, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
        {
            _cartManager = cartManager;
            _signInManager = signInManager;
            _userManager = userManager;
            Total = 0;
        }
        /// <summary>
        /// This is our method to check the cart of a user. 
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> OnGetAsync()
        {
            var signedIn = _signInManager.IsSignedIn(User);
            if (signedIn)
            {
                var userId = _userManager.GetUserId(User);
                Models.Cart cart = await _cartManager.GetCart(userId);
                List<CartItems> items = await _cartManager.GetAllItems(cart.ID);
                if(items == null)
                {
                    ModelState.AddModelError("", "You have no items in your cart.");
                    return RedirectToPage("/Products/Shop");
                }

                foreach (var item in items)
                {
                    Total += (item.Product.Price * item.Qty);
                }

                InCart = items;
                return Page();
            }
            else
            {
                ModelState.AddModelError("", "Please Login to view your Cart.");
                return RedirectToPage("/Account/Login");
            }

        }
    }
}
