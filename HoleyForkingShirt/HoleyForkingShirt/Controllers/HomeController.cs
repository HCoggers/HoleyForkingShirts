using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace HoleyForkingShirt.Controllers
{
    public class HomeController : Controller
    {
        private IPayment _payment;
        public IActionResult Index()
        {
            return View();
        }
        public HomeController(IPayment payment)
        {
            _payment = payment;
        }

        [HttpPost]
        public string Payment()
        {
            var result = _payment.Run();
            return result;
        }
    }
 
}