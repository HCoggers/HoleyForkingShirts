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
        public CartService(StoreDbContext storeDbContext)
        {
            _context = storeDbContext;
        }
        public async Task<Cart> CreateCart(Cart cart)
        {
            _context.Add(cart);
            await _context.SaveChangesAsync();
            return cart;
        }

        public async Task DeleteCart(int id)
        {
            var toDelete = await _context.Carts.FindAsync(id);
            _context.Carts.Remove(toDelete);

            await _context.SaveChangesAsync();
        }

        public async Task<Cart> GetCart(string userId)
        {
            var carts = await _context.Carts.Where(c => c.UserId == userId)
                .Include(c => c.CartItems)
                .ToListAsync();

            return carts.First();
        }

        public async Task<List<CartItems>> GetAllItems(int cartId)
        {
            var items = await _context.CartItems.Where(i => i.CartID == cartId)
                .Include(i => i.Product)
                .ToListAsync();
            return items;
        }

        public async Task UpdateCart(int id, Cart cart)
        {
            _context.Carts.Update(cart);
            await _context.SaveChangesAsync();
        }
    }
}
