using tba.Core.Entities;

namespace tba.Restaurant.Entities
{
    public class OrderItem : Entity
    {
        public int Quantity { get; set; }
        public bool IsAvialable { get; set; }
        public long MenuItemId { get; set; }
    }
}
