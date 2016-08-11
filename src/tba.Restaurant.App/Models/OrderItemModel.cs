namespace tba.Restaurant.App.Models
{
    public class OrderItemModel
    {
        public long MenuItemId { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
    }
}