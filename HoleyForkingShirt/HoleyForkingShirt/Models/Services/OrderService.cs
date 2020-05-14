using HoleyForkingShirt.Data;
using HoleyForkingShirt.Models.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoleyForkingShirt.Models.Services
{
    public class OrderService : IOrderManager
    {
        private StoreDbContext _context;

        public OrderService(StoreDbContext context)
        {
            _context = context;

        }
        public async Task<Order> CreateOrder(Order order)
        {
            _context.Add(order);
            await _context.SaveChangesAsync();
            return order;
        }

        public Task DeleteOrder(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Order>> GetOrders()
        {
            return await _context.Orders.ToListAsync();
        }

        public Order GetOrderById(int id)
        {
            var order = _context.Orders.Where(i => i.Id == id)
                .FirstOrDefault();
            return order;
        }

        public Task UpdateOrder(Order order)
        {
            throw new NotImplementedException();
        }
    }
}
