using System.Threading.Tasks;
using System.Web.Http;
using log4net;
using log4net.Core;
using tba.Core.Persistence.Interfaces;
using tba.Core.Utilities;
using tba.EFPersistence;
using tba.Restaurant.App.Models;
using tba.Restaurant.App.Services;
using tba.Restaurant.Entities;
using tba.Restaurant.WebApi.Context;

namespace tba.Restaurant.WebApi.Controllers
{
    public class MenusController : ApiController
    {
        // todo: get user id from user token in header
        private const long UserId = 1;
        private readonly IMenuService _menuService;

        public MenusController()
        {
            // todo: implement IOC
            var context = new RestaurantDbContext();
            IRepository<Menu> menuRepository = new EfRepository<Menu>(context, TimeProvider.Current);
            _menuService = new MenuService(menuRepository, TimeProvider.Current, new ResturantFactory(), LogManager.GetLogger("MenuService"));
        }

        [HttpPost]
        [Route("menus")]
        public async Task<IHttpActionResult> Post(MenuRequest request)
        {
            var menu = await _menuService.InsertMenuAsync(UserId, request);
            return Created("menus",menu);
        }
    }
}