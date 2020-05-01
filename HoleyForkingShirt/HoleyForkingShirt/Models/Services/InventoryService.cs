using HoleyForkingShirt.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoleyForkingShirt.Models.Services
{
    public class InventoryService : IInventory
    {
        private readonly StoreDbContext _context;

        /// <summary>
        /// Inventory Servic Constructor
        /// </summary>
        /// <param name="context">Database context to be used</param>
        public InventoryService(StoreDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Adds a given product to the saved database
        /// </summary>
        /// <param name="product">The product to be added</param>
        /// <returns>The same product</returns>
        public async Task<Product> CreateProduct(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return product;
        }

        /// <summary>
        /// Delete a product of a given ID form the database
        /// </summary>
        /// <param name="id">ID of product to be deleted</param>
        /// <returns>No Content</returns>
        public async Task DeleteProduct(int id)
        {
            var toDelete = await _context.Products.FindAsync(id);
            _context.Products.Remove(toDelete);

            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Returns a single product by its ID
        /// </summary>
        /// <param name="id">ID of product to find</param>
        /// <returns>Product found in database</returns>
        public async Task<Product> GetProductById(int id)
        {
            return await _context.Products.FindAsync(id);
        }

        /// <summary>
        /// Returns all products currently in the database
        /// </summary>
        /// <returns>A list of all products</returns>
        public async Task<List<Product>> GetProducts()
        {
            return await _context.Products.ToListAsync();
        }

        /// <summary>
        /// Update the properties of a product of a given ID
        /// </summary>
        /// <param name="product">Altered version of original property</param>
        /// <returns>No Content</returns>
        public async Task UpdateProduct(Product product)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
        }
    }
}
