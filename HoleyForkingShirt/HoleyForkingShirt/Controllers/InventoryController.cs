using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using HoleyForkingShirt.Data;
using HoleyForkingShirt.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HoleyForkingShirt.Controllers
{
    [Authorize(Policy = "AdminOnly")]
    public class InventoryController : Controller
    {
        private StoreDbContext _context;
        private IInventory _inventory;

        public Blob Blob { get; set; }

        [BindProperty]
        public IFormFile Image { get; set; }

        public InventoryController(StoreDbContext context, IInventory inventory, IConfiguration configuration)
        {
            Blob = new Blob(configuration);
            _context = context;
            _inventory = inventory;
        }
        // GET: /<controller>/
        [HttpGet]
        public async Task<IActionResult> IndexAsync()
        {
            var products = await _context.Products.ToListAsync();
            return View(products);
        }
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var product = await _context.Products.FindAsync(id);
            return View(product);
        }

        [HttpPost, ActionName("UpdateProduct")]
        public async Task<IActionResult> UpdateProduct(string name, string description, int price, string sku, int id, string size, IFormFile image)
        {
            var path = Path.GetTempFileName();

            using (var stream = System.IO.File.Create(path))
            {
               
                await image.CopyToAsync(stream);
            }

            await Blob.UploadFileAsync("products", name, path);

            var blob = await Blob.GetBlobAsync(name, "products");
            var uri = blob.Uri;

            Product updatedProduct = await _context.Products.FindAsync(id);
            updatedProduct.Name = name;
            updatedProduct.Description = description;
            updatedProduct.Sku = sku;
            updatedProduct.Image = uri.ToString();
            updatedProduct.Price = price;
            updatedProduct.Size = Enum.Parse<Sizes>(size);

            await _inventory.UpdateProduct(updatedProduct);
            var product = await _inventory.GetProductById(id);
            return View("Details", product);
        }

        [HttpPost, ActionName("DeleteProduct")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            await _inventory.DeleteProduct(id);
            var products = await _inventory.GetProducts();
            return View("Index", products);
        }

        [HttpPost, ActionName("CreateProduct")]
        public async Task<IActionResult> CreateProduct(string name, string description, int price, string sku, int id, string size, string image)
        {
            Product newProduct = new Product
            {
                Name = name,
                Sku = sku,
                Description = description,
                Image = image,
                Price = price,
                Size = Enum.Parse<Sizes>(size),
            };
            await _inventory.CreateProduct(newProduct);
            var product = _context.Products.Where(p => p.Name == newProduct.Name).FirstOrDefault();
            return View("Details", product);
        }
    }
   
}
