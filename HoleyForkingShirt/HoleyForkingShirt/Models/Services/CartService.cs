using HoleyForkingShirt.Data;
using HoleyForkingShirt.Models.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoleyForkingShirt.Models.Services
{
    public class CartService : ICartManager
    {
        private readonly StoreDbContext _context;
        /// <summary>
        /// Constructor for carts
        /// </summary>
        /// <param name="storeDbContext"></param>
        public CartService(StoreDbContext storeDbContext)
        {
            _context = storeDbContext;
        }
        /// <summary>
        /// Creates carts for the databse
        /// </summary>
        /// <param name="cart"></param>
        /// <returns></returns>
        public async Task<Cart> CreateCart(Cart cart)
        {
            _context.Add(cart);
            await _context.SaveChangesAsync();
            return cart;
        }
        /// <summary>
        /// Deletes carts in the database
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteCart(int id)
        {
            var toDelete = await _context.Carts.FindAsync(id);
            _context.Carts.Remove(toDelete);

            await _context.SaveChangesAsync();
        }
        /// <summary>
        /// Grabs the reference to a cart based on userid 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<Cart> GetCart(string userId)
        {
            var carts = await _context.Carts.Where(c => c.UserId == userId)
                .Include(c => c.CartItems)
                .ToListAsync();

            if (carts.Count > 0)
                return carts.First();
            else
                return null;
        }
        /// <summary>
        /// grabs all items in a cart based on cartid
        /// </summary>
        /// <param name="cartId"></param>
        /// <returns></returns>
        public async Task<List<CartItems>> GetAllItems(int cartId)
        {
            var items = await _context.CartItems.Where(i => i.CartID == cartId)
                .Include(i => i.Product)
                .ToListAsync();
            return items;
        }
        /// <summary>
        /// Updates the cart in the database.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cart"></param>
        /// <returns></returns>
        public async Task UpdateCart(int id, Cart cart)
        {
            _context.Carts.Update(cart);
            await _context.SaveChangesAsync();
        }

        public async Task<decimal> GetTotal(int cartId)
        {
            List<CartItems> items = await GetAllItems(cartId);
            decimal total = 0;
            foreach (var item in items)
                total += item.Product.Price;
            return total;
        }
    }
}
