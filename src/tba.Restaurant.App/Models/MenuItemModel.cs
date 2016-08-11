using tba.Restaurant.Entities;

namespace tba.Restaurant.App.Models
{
    public class MenuItemModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }

        public static MenuItemModel Create(MenuItem entity)
        {
            var m = new MenuItemModel
            {
                Id = entity.Id,
                Name = entity.Name,
                Price = entity.Price
            };
            return m;
        }
    }
}