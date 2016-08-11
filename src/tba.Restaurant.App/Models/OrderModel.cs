namespace tba.Restaurant.App.Models
{
    public class OrderModel
    {
        public long DinerId { get; set; }
        public string Name { get; set; }
        public OrderItemModel[] Items { get; set; }
        public MenuModel Menu { get; set; }
    }
}