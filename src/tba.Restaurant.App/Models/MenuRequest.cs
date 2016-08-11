namespace tba.Restaurant.App.Models
{
    public class MenuRequest
    {
        public string Name { get; set; }
        public MenuItemRequest[] Items { get; set; }
    }
}