using System.Collections.Generic;
using System.Threading.Tasks;

namespace HoleyForkingShirt.Models.Interfaces
{
    public interface IOrderManager
    {
        // CREATE
        public Task<Order> CreateOrder(Order order);

        // READ
        public Task<List<Order>> GetOrders();
        public Order GetOrderById(int id);

        // UPDATE
        public Task UpdateOrder(Order order);

        // DELETE
        public Task DeleteOrder(int id);
    }
}