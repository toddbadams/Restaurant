using System.Data.Entity;
using tba.Core.Entities;
using tba.Restaurant.Entities;

namespace tba.Restaurant.WebApi.Context
{
    public class RestaurantDbContext : DbContext
    {
        public RestaurantDbContext(string connectionStringName)
            : base(connectionStringName)
        {
        }

        public RestaurantDbContext()
            : this("DefaultConnection")
        {
            // todo: add this as a web config setting
            Database.SetInitializer(new DropCreateDatabaseAlways<RestaurantDbContext>());
        }

        public IDbSet<Order> Diners { get; set; }
        public IDbSet<Menu> Menus { get; set; }
        public IDbSet<Entity.Audit> Audits { get; set; }
    }
}
