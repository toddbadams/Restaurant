using System.Threading.Tasks;
using log4net;
using tba.Core.Exceptions;
using tba.Core.Utilities;
using tba.Restaurant.App.Models;
using tba.Restaurant.Entities;
using tba.Restaurant.Data;
using System.Collections.Generic;
using tba.Core.Persistence;
using System;

namespace tba.Restaurant.App.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ITimeProvider _timeProvider;
        private readonly IMenuService _menuService;
        private readonly IRestaurantFactory _restaurantFactory;
        private readonly ILog _log;
        private readonly IUnitOfWork _unitOfWork;
        private const string FriendlyName = "Order Service";

        public OrderService(IOrderRepository orderRepository,
            ITimeProvider timeProvider,
            IMenuService menuService,
            IRestaurantFactory restaurantFactory,
            ILog log,
            IUnitOfWork unitOfWork)
        {
            _orderRepository = orderRepository;
            _timeProvider = timeProvider;
            _menuService = menuService;
            _restaurantFactory = restaurantFactory;
            _log = log;
            _unitOfWork = unitOfWork;
        }

        public async Task<OrderModel> GetAsync(long userId, long orderId)
        {
            var msg = string.Format("Get. userId={0}, order={1}", userId, orderId);
            _log.Info(msg);
            var e = await GetEntityAsync(userId, orderId);
            if (e.State == Order.States.Closed)
            {
                throw new EntityDoesNotExistException("this order is closed");
            }
            var menu = await _menuService.GetAsync(userId, e.MenuId);
            return _restaurantFactory.Create(e, menu);
        }

        public async Task<OrderModel> InsertOrderAsync(long userId, OrderRequest order)
        {
            var msg = string.Format("Insert. userId={0}, order={1}", userId, Serialization.Serialize(order));

            _log.Info(msg);
            var menu = await _menuService.GetAsync(userId, order.MenuId);
            var e = _restaurantFactory.Create(order);
            await _orderRepository.OpenAsync(userId, e);
            var m = _restaurantFactory.Create(e, menu);
            return m;
        }

        public async Task<OrderModel> InsertOrderItemsAsync(long userId, long orderId, OrderItemRequest[] items)
        {
            var msg = string.Format("Insert. userId={0}, orderId={1}, items={2}", userId, orderId, Serialization.Serialize(items));
            _log.Info(msg);
            var entities = new List<OrderItem>();
            // todo if exists add to qnty
            // todo check of menu item is still avialable
            foreach (var item in items)
            {
                entities.Add(_restaurantFactory.Create(item));
            }
            using (var t = _unitOfWork.BeginTransaction())
            {
                try
                {
                    // todo seperate threads for update and get menu
                    var order = await _orderRepository.AddItemsAsync(userId, orderId, entities.ToArray());
                    var menu = await _menuService.GetAsync(userId, order.MenuId);
                    t.Commit();
                    return _restaurantFactory.Create(order, menu);
                }
                catch (Exception ex)
                {
                    _log.Error("Failed to insert order, rolling back", ex);
                    t.Rollback();
                    throw;
                }
            }
        }

        public async Task<bool> CloseOrderAsync(long userId, long orderId)
        {
            var msg = string.Format("Close. userId={0}, orderId={1}", userId, orderId);
            _log.Info(msg);
            var e = await GetEntityAsync(userId, orderId);
            e.State = Order.States.Closed;
            await _orderRepository.CloseAsync(userId, orderId);
            return true;
        }


        /// <summary>
        /// Get a single entity
        /// </summary>
        /// <param name="userId">a user</param>
        /// <param name="entityId">Id of the entity to delete or undelete</param>
        /// <returns>the entity</returns>
        private async Task<Order> GetEntityAsync(long userId, long entityId)
        {
            var msg = string.Format("GetEntityAsync. userId={0}, entityId={1}", userId, entityId);

            _log.Info(msg);
            var e = await _orderRepository.GetAsync(entityId);
            return await Task.FromResult(e);
        }
    }
}
