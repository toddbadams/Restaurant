using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using log4net;
using tba.Core.Persistence.Interfaces;
using tba.Core.Utilities;
using tba.Restaurant.App.Models;
using tba.Restaurant.Entities;

namespace tba.Restaurant.App.Services
{
    public class MenuService : IMenuService
    {
        private readonly IRepository<Menu> _menuRepository;
        private readonly IResturantFactory _resturantFactory;
        private readonly ILog _log;
        private const string FriendlyName = "Menu Service";

        public MenuService(IRepository<Menu> menuRepository,
            ITimeProvider timeProvider,
            IResturantFactory resturantFactory,
            ILog log)
        {
            _menuRepository = menuRepository;
            _resturantFactory = resturantFactory;
            _log = log;
        }

        public async Task<MenuModel> GetAsync(long userId, long menuId)
        {
            var e = await GetEntityAsync(userId, menuId);
            return _resturantFactory.Create(e);
        }

        public async Task<MenuModel> InsertMenuAsync(long userId, MenuRequest menu)
        {
            var msg = string.Format("Insert. userId={0}, menu={1}", userId, Serialization.Serialize(menu));
            try
            {
                _log.Info(msg);
                var e = _resturantFactory.Create(menu);
                await _menuRepository.InsertAsync(userId, e);
                if (menu.Items != null)
                {
                    var results = await InsertMenuItemsAsync(userId, e.Id, menu.Items);
                }
                return _resturantFactory.Create(e);
            }
            catch (Exception exception)
            {
                _log.Error(msg, exception);
                throw new ApplicationException("Failed to insert " + menu.Name + " " + FriendlyName);
            }

        }

        private async Task<MenuModel> InsertMenuItemsAsync(long userId, long menuId, MenuItemRequest[] items)
        {
            var menuEntity = await GetEntityAsync(userId, menuId);
            if (menuEntity.Items == null) menuEntity.Items = new List<MenuItem>();
            foreach (var item in items)
            {
                menuEntity.Items.Add(_resturantFactory.Create(item));
            }
            menuEntity = await _menuRepository.UpdateAsync(userId, menuEntity);
            return _resturantFactory.Create(menuEntity);
        }

        /// <summary>
        /// Get a single entity
        /// </summary>
        /// <param name="userId">a user</param>
        /// <param name="entityId">Id of the entity to delete or undelete</param>
        /// <returns>the entity</returns>
        private async Task<Menu> GetEntityAsync(long userId, long entityId)
        {
            var msg = string.Format("GetEntityAsync. userId={0}, entityId={1}", userId, entityId);
            try
            {
                _log.Info(msg);
                var e = await _menuRepository.GetAsync(entityId);
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
