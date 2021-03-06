﻿using System.Threading.Tasks;
using System.Web.Http;
using log4net;
using tba.Core.Persistence.Interfaces;
using tba.Core.Utilities;
using tba.EFPersistence;
using tba.Restaurant.App.Models;
using tba.Restaurant.App.Services;
using tba.Restaurant.Entities;
using tba.Restaurant.WebApi.Context;
using tba.Restaurant.DataEf;
using tba.Restaurant.Data;

namespace tba.Restaurant.WebApi.Controllers
{
    public class OrdersController : ApiController
    {
        // todo (tba:17/05/15): get user id from user token in header
        private const long UserId = 1;
        private readonly IOrderService _service;

        public OrdersController()
        {
            // todo: implement IOC
            var context = new RestaurantDbContext();
            var uow = new UnitOfWork(context);
            IOrderRepository repository = new OrderRepository(context);
            IRepository<Menu> menuRepository = new EfRepository<Menu>(context, TimeProvider.Current);
            IMenuService menuService = new MenuService(menuRepository, TimeProvider.Current, new ResturantFactory(), LogManager.GetLogger("MenuService"));
            _service = new OrderService(repository, TimeProvider.Current, menuService, new ResturantFactory(), LogManager.GetLogger("OrderService"), uow);
        }

        [HttpGet]
        [Route("orders/{id:long}")]
        public async Task<IHttpActionResult> Get(long id)
        {
            var vm = await _service.GetAsync(UserId, id);
            return Ok(vm);
        }

        [HttpPost]
        [Route("orders")]
        public async Task<IHttpActionResult> Open(OrderRequest payload)
        {
            var vm = await _service.InsertOrderAsync(UserId, payload);
            return Ok(vm);
        }

        [HttpPost]
        [Route("orders/{id:long}/close")]
        public async Task<IHttpActionResult> Close(long id)
        {
            var vm = await _service.CloseOrderAsync(UserId, id);
            return Ok(vm);
        }
    }
}
