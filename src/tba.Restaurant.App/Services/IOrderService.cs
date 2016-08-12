using System.Threading.Tasks;
using tba.Restaurant.App.Models;

namespace tba.Restaurant.App.Services
{
    public interface IOrderService
    {
        Task<OrderModel> GetAsync(long userId, long dinerId);
        Task<OrderModel> InsertOrderAsync(long userId, OrderRequest order);
        Task<OrderModel> InsertOrderItemsAsync(long userId, long dinerId, OrderItemRequest[] items);
        Task<bool> CloseOrderAsync(long userId, long dinerId);
    }
}