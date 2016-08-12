using System.Data.Entity;
using System.Threading;
using System.Threading.Tasks;
using tba.Core.Entities;
using tba.Restaurant.Entities;

namespace tba.Restaurant.WebApi.Context
{
    public class RestaurantDbContext : DbContext
    {
        public RestaurantDbContext(string connectionStringName)
    : base(connectionStringName)
        {
            // todo: add this as a web config setting
            Database.SetInitializer(new DropCreateDatabaseAlways<RestaurantDbContext>());

        }

        public RestaurantDbContext()
            : this("DefaultConnection")
        {
        }

        public IDbSet<Order> Diners { get; set; }
        public IDbSet<Menu> Menus { get; set; }

    }
}
