using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HoleyForkingShirt.Data;
using HoleyForkingShirt.Models;
using HoleyForkingShirt.Models.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HoleyForkingShirt.Pages.Products
{
    public class DetailsModel : PageModel
    {
        private StoreDbContext _context;
        private UserManager<ApplicationUser> _userManager;
        private ICartManager _cartManager;

        [BindProperty(SupportsGet = true)]
        public Product Product { get; set; }

        public DetailsModel(ICartManager cartManager, StoreDbContext context, UserManager<ApplicationUser> userManager)
        {
            _cartManager = cartManager;
            _context = context;
            _userManager = userManager;
        }
        public async Task<IActionResult> OnGetAsync(int id)
        {
            Product = await _context.Products.FindAsync(id);
            return Page();
        }
        
        /// <summary>
        /// This method is what adds things to the detail page. 
        /// </summary>
        /// <param name="inputId"></param>
        /// <returns></returns>
        public async Task<IActionResult> OnPost(string inputId)
        {
            int productId = Int32.Parse(inputId);
            Models.Cart cart = await _cartManager.GetCart(_userManager.GetUserId(User));
            Product product = await _context.Products.FindAsync(productId);

            CartItems newItem = new CartItems
            {
                CartID = cart.ID,
                ProductID = product.ID,
                Qty = 1,
                Cart = cart,
                Product = product
            };

            if (cart.CartItems == null)
            {
                cart.CartItems = new List<CartItems>();
                cart.CartItems.Add(newItem);
            }
            else
            {
                CartItems item = await _context.CartItems.FindAsync(newItem.CartID, newItem.ProductID);
                if (item != null)
                    item.Qty++;
                else
                {
                    cart.CartItems.Add(newItem);
                }
            }

            await _cartManager.UpdateCart(cart.ID, cart);
            await _context.SaveChangesAsync();

            return RedirectToPage("/Cart/Cart");
        }
    }
}
