using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoleyForkingShirt.Models
{
    public interface IInventory
    {
        // CREATE
        public Task<Product> CreateProduct(Product product);

        // READ
        public Task<List<Product>> GetProducts();
        public Task<Product> GetProductById(int id);

        // UPDATE
        public Task UpdateProduct(Product product);

        // DELETE
        public Task DeleteProduct(int id);
    }
}
