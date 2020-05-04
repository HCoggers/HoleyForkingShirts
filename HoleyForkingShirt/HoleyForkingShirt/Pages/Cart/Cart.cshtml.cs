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
        private IEmailSender _emailSender;
        private IPayment _payment;

        public List<CartItems> InCart;
        public decimal Total;

        public CartModel(ICartManager cartManager, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, IEmailSender emailSender, IPayment payment)
        {
            _cartManager = cartManager;
            _signInManager = signInManager;
            _userManager = userManager;
            _emailSender = emailSender;
            _payment = payment;
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
        /// <summary>
        /// This method sends an email based on what was in your cart at checkout. 
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> OnPostAsync()
        {/*
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<h1> Order Details:</h1>");
            sb.AppendLine($"<p> {User.Claims.First(c => c.Type == ClaimTypes.GivenName).Value} </ p >");
            sb.AppendLine("<h2> Cart:</h2>");
            sb.AppendLine("<table><tr><th> Product </th>");
            sb.AppendLine("<th> Qty </th>");
            sb.AppendLine("<th> Price </th></tr>");

            int cartID = _cartManager.GetCart(_userManager.GetUserId(User)).Result.ID;
            foreach (var item in await _cartManager.GetAllItems(cartID))
            {
                sb.AppendLine($" <tr> <td> {item.Product.Name} </td> <td> {item.Qty} </td> <td>${item.Product.Price} </td> </tr>");
                Total += item.Product.Price;
            }
            sb.AppendLine($"</table><p> Total: ${Total} </p>");
            await _emailSender.SendEmailAsync(User.Claims.First(c => c.Type == ClaimTypes.Email).Value, "Here is your receipt.", sb.ToString());*/
            return RedirectToAction("Checkout");
        }
    }
}
