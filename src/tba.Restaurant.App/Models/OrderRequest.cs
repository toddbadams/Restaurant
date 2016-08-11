using System.ComponentModel.DataAnnotations;

namespace tba.Restaurant.App.Models
{
    public class OrderRequest
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public long MenuId { get; set; }
    }
}