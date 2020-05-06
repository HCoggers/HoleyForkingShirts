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
        private IOrderManager _orderManager;

        public CheckoutController(ICartManager cartManager, UserManager<ApplicationUser> userManager, IEmailSender emailSender, IPayment payment, IOrderManager orderManager)
        {
            _cartManager = cartManager;
            _userManager = userManager;
            _emailSender = emailSender;
            _payment = payment;
            _orderManager = orderManager;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Receipt(string firstname, string lastname, string address, string city, string state, string country)
        {

            var details = _payment.GetAddress(firstname, lastname, address, city, state, country);
            return View(details);
        }

        /// <summary>
        /// This method sends a payment request, and if it is successful, sends an email based on what was in your cart at checkout. 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Payment(string cardtype, string firstname, string lastname, string address, string city, string state, string country)
        {

            var details = _payment.GetAddress(firstname, lastname, address, city, state, country);
            var response = _payment.Run(cardtype, details);
            var userId = _userManager.GetUserId(User);

            if (response == "success")
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("<h1> Order Details:</h1>");
                sb.AppendLine($"<p>Name: {details.firstName} {details.lastName}</ p >");
                sb.AppendLine($"<p>Address: {details.address}</ p >");
                sb.AppendLine($"<p>{details.city}, {details.state}, {details.country}</ p >");
                sb.AppendLine("<h2> Cart:</h2>");
                sb.AppendLine("<table><tr><th> Product </th>");
                sb.AppendLine("<th> Qty </th>");
                sb.AppendLine("<th> Price </th></tr>");

                int cartID = _cartManager.GetCart(userId).Result.ID;
                decimal total = 0m;

                foreach (var item in await _cartManager.GetAllItems(cartID))
                {
                    sb.AppendLine($" <tr> <td> {item.Product.Name} </td> <td> {item.Qty} </td> <td>${item.Product.Price} </td> </tr>");
                    total += item.Product.Price;
                }
                sb.AppendLine($"</table><p> Total: ${total} </p>");
                await _emailSender.SendEmailAsync(User.Claims.First(c => c.Type == ClaimTypes.Email).Value, "Here is your receipt.", sb.ToString());

                Order order = new Order
                {
                    UserId = userId,
                    Date = DateTime.Now,
                    Total = total,
                    SB = sb.ToString()
                    
                };

                await _orderManager.CreateOrder(order);



                return RedirectToAction("Receipt", new { firstname, lastname, address, city, state, country });
            }

            return RedirectToAction("Index");
        }
    }
}