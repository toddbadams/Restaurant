namespace tba.Restaurant.App.Models
{
    public class MenuModel
    {
        public long MenuId { get; set; }
        public string Name { get; set; }
        public MenuItemModel[] Items { get; set; }        
    }
}