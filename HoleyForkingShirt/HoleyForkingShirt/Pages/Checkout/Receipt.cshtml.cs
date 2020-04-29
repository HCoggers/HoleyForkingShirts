using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HoleyForkingShirt.Models;
using HoleyForkingShirt.Models.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HoleyForkingShirt
{
    public class ReceiptModel : PageModel
    {
        private UserManager<ApplicationUser> _userManager;
        private ICartManager _cartManager;

        public List<CartItems> InCart;
        public decimal Total;

        public ReceiptModel(UserManager<ApplicationUser> userManager, ICartManager cartManager)
        {
            _userManager = userManager;
            _cartManager = cartManager;
        }
        public async Task<IActionResult> OnGet()
        {
            var userId = _userManager.GetUserId(User);
            Cart cart = await _cartManager.GetCart(userId);
            List<CartItems> items = await _cartManager.GetAllItems(cart.ID);

            InCart = items;

            foreach (var item in items)
            {
                Total += (item.Product.Price * item.Qty);
            }

            return Page();
        }
    }
}