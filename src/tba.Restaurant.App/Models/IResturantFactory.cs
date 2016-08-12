using tba.Restaurant.Entities;

namespace tba.Restaurant.App.Models
{
    public interface IRestaurantFactory
    {
        Order Create(OrderRequest request);
        OrderItem Create(OrderItemRequest request);
        OrderModel Create(Order entity, MenuModel menu);
        MenuItem Create(MenuItemRequest request);
        Menu Create(MenuRequest request);
        MenuModel Create(Menu entity);
    }
}