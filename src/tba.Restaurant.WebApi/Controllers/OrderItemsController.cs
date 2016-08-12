using System.Threading.Tasks;
using System.Web.Http;
using log4net;
using tba.Core.Persistence.Interfaces;
using tba.Core.Utilities;
using tba.EFPersistence;
using tba.Restaurant.App.Models;
using tba.Restaurant.App.Services;
using tba.Restaurant.Entities;
using tba.Restaurant.WebApi.Context;
using tba.Restaurant.Data;
using tba.Restaurant.DataEf;

namespace tba.Restaurant.WebApi.Controllers
{
    public class OrderItemsController : ApiController
    {
        // todo: get user id from user token in header
        private const long UserId = 1;
        private readonly IOrderService _service;

        public OrderItemsController()
        {
            // Poor man's IOC
            var context = new RestaurantDbContext();
            IOrderRepository repository = new OrderRepository(context);
            IRepository<Menu> menuRepository = new EfRepository<Menu>(context, TimeProvider.Current);
            IMenuService menuService = new MenuService(menuRepository, TimeProvider.Current, new ResturantFactory(), LogManager.GetLogger("MenuService"));
            _service = new OrderService(repository, TimeProvider.Current, menuService, new ResturantFactory(), LogManager.GetLogger("OrderService"));
        }

        [HttpPost]
        [Route("orders/{id:long}/items")]
        public async Task<IHttpActionResult> Post(long id, OrderItemRequest[] payload)
        {
            var vm = await _service.InsertOrderItemsAsync(UserId, id, payload);
            return Ok(vm);
        }
    }
}
