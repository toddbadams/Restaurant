using System.ComponentModel.DataAnnotations;

namespace tba.Restaurant.App.Models
{
    public class OrderItemRequest
    {
        [Required]
        public long MenuItemId { get; set; }
        [Required]
        public int Quantity { get; set; }       
    }
}
