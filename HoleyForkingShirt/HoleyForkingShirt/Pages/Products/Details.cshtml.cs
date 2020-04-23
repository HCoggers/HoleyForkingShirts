using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HoleyForkingShirt.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HoleyForkingShirt.Pages.Products
{
    public class DetailsModel : PageModel
    {
        private IInventory _inventory;

        [BindProperty(SupportsGet = true)]
        public Product Product { get; set; }

        public DetailsModel(IInventory inventory)
        {
            _inventory = inventory;
        }
        public async Task<IActionResult> OnGetAsync(int id)
        {
            Product = await _inventory.GetProductById(id);
            return Page();
        }
    }
}
