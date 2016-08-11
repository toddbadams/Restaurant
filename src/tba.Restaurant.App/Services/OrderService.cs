using System;
using System.Threading.Tasks;
using log4net;
using tba.Core.Exceptions;
using tba.Core.Persistence.Interfaces;
using tba.Core.Utilities;
using tba.Restaurant.App.Models;
using tba.Restaurant.Entities;

namespace tba.Restaurant.App.Services
{
    public class OrderService : IOrderService
    {
        private readonly IRepository<Order> _dinerRepository;
        private readonly ITimeProvider _timeProvider;
        private readonly IMenuService _menuService;
        private readonly IResturantFactory _resturantFactory;
        private readonly ILog _log;
        private const string FriendlyName = "Order Service";

        public OrderService(IRepository<Order> dinerRepository,
            ITimeProvider timeProvider,
            IMenuService menuService,
            IResturantFactory resturantFactory,
            ILog log)
        {
            _dinerRepository = dinerRepository;
            _timeProvider = timeProvider;
            _menuService = menuService;
            _resturantFactory = resturantFactory;
            _log = log;
        }

        public async Task<OrderModel> GetAsync(long userId, long dinerId)
        {
            var msg = string.Format("Get. userId={0}, order={1}", userId, dinerId);
            try
            {
                _log.Info(msg);
                var e = await GetEntityAsync(userId, dinerId);
                if (e.State == Order.States.Closed)
                {
                    throw new EntityDoesNotExistException("this order is closed");
                }
                var menu = await _menuService.GetAsync(userId, e.MenuId);
                return _resturantFactory.Create(e, menu);
            }
            catch (EntityDoesNotExistException exception)
            {
                _log.Error(msg, exception);
                throw;
            }
            catch (Exception exception)
            {
                _log.Error(msg, exception);
                throw new ApplicationException(string.Format("Failed to get {0} with id of {1}", FriendlyName, dinerId));
            }
        }

        public async Task<OrderModel> InsertDinerAsync(long userId, OrderRequest order)
        {
            var msg = string.Format("Insert. userId={0}, order={1}", userId, Serialization.Serialize(order));
            try
            {
                _log.Info(msg);
                var menu = await _menuService.GetAsync(userId, order.MenuId);
                var e = _resturantFactory.Create(order);
                await _dinerRepository.InsertAsync(userId, e);
                var m = _resturantFactory.Create(e, menu);
                return m;
            }
            catch (EntityDoesNotExistException exception)
            {
                _log.Error(msg, exception);
                throw;
            }
            catch (Exception exception)
            {
                _log.Error(msg, exception);
                throw new ApplicationException("Failed to insert " + order.Name + " " + FriendlyName);
            }
        }

        public async Task<OrderModel> InsertOrderItemsAsync(long userId, long dinerId, OrderItemRequest[] items)
        {
            var msg = string.Format("Insert. userId={0}, dinerId={1}, items={2}", userId, dinerId, Serialization.Serialize(items));
            try
            {
                _log.Info(msg);
                var e = await GetEntityAsync(userId, dinerId);
                // todo if exists add to qnty
                // todo check of menu item is still avialable
                foreach (var item in items)
                {
                    e.Items.Add(_resturantFactory.Create(item));
                }
                // todo seperate threads for update and get menu
                e = await _dinerRepository.UpdateAsync(userId, e);
                var menu = await _menuService.GetAsync(userId, e.MenuId);
                return _resturantFactory.Create(e, menu);
            }
            catch (EntityDoesNotExistException exception)
            {
                _log.Error(msg, exception);
                throw;
            }
            catch (Exception exception)
            {
                _log.Error(msg, exception);
                throw new ApplicationException("Failed to insert " + dinerId + " " + FriendlyName);
            }
        }

        public async Task<bool> CloseDinerAsync(long userId, long dinerId)
        {
            var msg = string.Format("Close. userId={0}, dinerId={1}", userId, dinerId);
            try
            {
                _log.Info(msg);
                var e = await GetEntityAsync(userId, dinerId);
                e.State = Order.States.Closed;
                await _dinerRepository.UpdateAsync(userId, e);
                return true;
            }
            catch (EntityDoesNotExistException exception)
            {
                _log.Error(msg, exception);
                throw;
            }
            catch (Exception exception)
            {
                _log.Error(msg, exception);
                throw new ApplicationException("Failed to insert " + dinerId + " " + FriendlyName);
            }
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
            try
            {
                _log.Info(msg);
                var e = await _dinerRepository.GetAsync(entityId);
                return await Task.FromResult(e);
            }
            catch (Exception exception)
            {
                _log.Error(msg, exception);
                return null;
            }
        }
    }
}
