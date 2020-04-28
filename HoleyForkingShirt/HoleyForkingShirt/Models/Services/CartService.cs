using HoleyForkingShirt.Data;
using HoleyForkingShirt.Models.Interfaces;
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

        public Cart GetCart(string userId)
        {
            var cart = _context.Carts.First(c => c.UserId == userId);
            return cart;
        }

        public async Task UpdateCart(int id, Cart cart)
        {
            _context.Carts.Update(cart);
            await _context.SaveChangesAsync();
        }
    }
}
