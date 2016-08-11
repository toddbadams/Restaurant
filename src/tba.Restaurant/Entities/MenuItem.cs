using tba.Core.Entities;

namespace tba.Restaurant.Entities
{
    public class MenuItem : Entity
    {
        public string Name { get; set; }
        public double Price { get; set; }
        public bool IsAvialable { get; set; }
    }
}
