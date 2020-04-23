using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HoleyForkingShirt.Data;
using HoleyForkingShirt.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HoleyForkingShirt.Pages.Products
{
    public class ShopModel : PageModel
    {
        private IInventory _inventory;

        [BindProperty(SupportsGet =true)]
        public List<Product> Inventory { get; set; }

        public ShopModel(IInventory inventory)
        {
            _inventory = inventory;
        }
        public async Task<IActionResult> OnGet()
        {
            Inventory = await _inventory.GetProducts();
            return Page(); 
        }
    }
}
