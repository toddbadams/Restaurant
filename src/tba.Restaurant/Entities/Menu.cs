using System.Collections.Generic;
using tba.Core.Entities;

namespace tba.Restaurant.Entities
{
    public class Menu : Entity
    {
        public string Name { get; set; }
        public virtual ICollection<MenuItem> Items { get; set; }
    }
}