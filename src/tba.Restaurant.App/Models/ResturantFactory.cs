using System.Linq;
using tba.Restaurant.Entities;

namespace tba.Restaurant.App.Models
{
    public class ResturantFactory : IResturantFactory
    {
        public Order Create(OrderRequest request)
        {
            var e = new Order
            {
                Name = request.Name,
                MenuId = request.MenuId,
                State = Order.States.Open
            };
            return e;
        }

        public OrderItem Create(OrderItemRequest request)
        {
            var e = new OrderItem
            {
                MenuItemId = request.MenuItemId,
                Quantity = request.Quantity
            };
            return e;
        }


        public OrderModel Create(Order entity, MenuModel menu)
        {
            if (entity == null) return null;
            if (menu == null) return null;
            var items = entity.Items != null
                ? entity.Items.Select(Create).ToArray()
                : new OrderItemModel[0];
            foreach (var item in items)
            {
                var x = (from mi in menu.Items
                         where mi.Id == item.MenuItemId
                         select mi).FirstOrDefault();
                if (x == null) continue;
                item.Name = x.Name;
                item.Price = x.Price;
            }
            var m = new OrderModel
            {
                DinerId = entity.Id,
                Name = entity.Name,
                Items = items,
                Menu = menu
            };
            return m;
        }

        public OrderItemModel Create(OrderItem entity)
        {
            var m = new OrderItemModel
            {
                Quantity = entity.Quantity,
                MenuItemId = entity.Id
            };
            return m;
        }

        public MenuItem Create(MenuItemRequest request)
        {
            var e = new MenuItem
            {
                Name = request.Name,
                Price = request.Price
            };
            return e;
        }

        public Menu Create(MenuRequest request)
        {
            var e = new Menu
            {
                Name = request.Name
            };
            return e;
        }

        public MenuModel Create(Menu entity)
        {
            var m = new MenuModel
            {
                MenuId = entity.Id,
                Name = entity.Name,
                Items = (entity.Items != null
                ? entity.Items.Select(MenuItemModel.Create).ToArray()
                : new MenuItemModel[0])
            };
            return m;
        }
    }
}