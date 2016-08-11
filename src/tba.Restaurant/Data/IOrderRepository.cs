using System.Threading.Tasks;
using tba.Restaurant.Entities;

namespace tba.Restaurant.Data
{
    public interface IOrderRepository
    {
        Task<Order> GetAsync(long id);
        Task CloseOrder(long userId, long dinerId);
    }
}
