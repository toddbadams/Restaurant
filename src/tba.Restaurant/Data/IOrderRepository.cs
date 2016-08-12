using System;
using System.Threading.Tasks;
using tba.Restaurant.Entities;

namespace tba.Restaurant.Data
{
    public interface IOrderRepository
    {
        Task<Order> GetAsync(long id);
        Task<Order> OpenAsync(long userId, Order order);
        Task<Order> AddItemsAsync(long userId, long orderId, OrderItem[] items);
        Task CloseAsync(long userId, long orderId);
    }
}
