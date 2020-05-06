using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using HoleyForkingShirt.Data;
using HoleyForkingShirt.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HoleyForkingShirt.Pages.Admin
{
    public class OrderDetailsModel : PageModel
    {
        private StoreDbContext _context;

        public Order Order { get; private set; }
        public string Html { get; set; }

        public OrderDetailsModel(StoreDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Order = await _context.Orders.FindAsync(id);
            Html = Order.SB;
            return Page();
        }
    }
}
