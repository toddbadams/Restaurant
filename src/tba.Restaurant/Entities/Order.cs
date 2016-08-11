using System.Collections.Generic;
using tba.Core.Entities;

namespace tba.Restaurant.Entities
{
    public class Order : Entity
    {
        public long MenuId { get; set; }
        public string Name { get; set; }
        public States State { get; set; }
        public virtual ICollection<OrderItem> Items { get; set; }

        public enum States
        {
            Open = 1,
            Closed = 2
        }
    }
}