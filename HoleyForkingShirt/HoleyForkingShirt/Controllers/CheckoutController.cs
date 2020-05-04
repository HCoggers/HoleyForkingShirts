using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using HoleyForkingShirt.Models;
using HoleyForkingShirt.Models.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HoleyForkingShirt.Controllers
{
    public class CheckoutController : Controller
    {
        private ICartManager _cartManager;
        private UserManager<ApplicationUser> _userManager;
        private IEmailSender _emailSender;
        private IPayment _payment;

        public CheckoutController(ICartManager cartManager, UserManager<ApplicationUser> userManager, IEmailSender emailSender, IPayment payment)
        {
            _cartManager = cartManager;
            _userManager = userManager;
            _emailSender = emailSender;
            _payment = payment;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// This method sends an email based on what was in your cart at checkout. 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Payment(string cardtype)
        {
            var response = _payment.Run(cardtype);

            if(response == "success")
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("<h1> Order Details:</h1>");
                sb.AppendLine($"<p> {User.Claims.First(c => c.Type == ClaimTypes.GivenName).Value} </ p >");
                sb.AppendLine("<h2> Cart:</h2>");
                sb.AppendLine("<table><tr><th> Product </th>");
                sb.AppendLine("<th> Qty </th>");
                sb.AppendLine("<th> Price </th></tr>");

                int cartID = _cartManager.GetCart(_userManager.GetUserId(User)).Result.ID;
                decimal total = 0m;

                foreach (var item in await _cartManager.GetAllItems(cartID))
                {
                    sb.AppendLine($" <tr> <td> {item.Product.Name} </td> <td> {item.Qty} </td> <td>${item.Product.Price} </td> </tr>");
                    total += item.Product.Price;
                }
                sb.AppendLine($"</table><p> Total: ${total} </p>");
                await _emailSender.SendEmailAsync(User.Claims.First(c => c.Type == ClaimTypes.Email).Value, "Here is your receipt.", sb.ToString());
                return RedirectToPage("/Checkout/Receipt");
            }

            return RedirectToAction("Index");
        }
    }
}