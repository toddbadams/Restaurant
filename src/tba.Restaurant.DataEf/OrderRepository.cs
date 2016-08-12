using System;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using tba.Core.Exceptions;
using tba.Restaurant.Data;
using tba.Restaurant.Entities;

namespace tba.Restaurant.DataEf
{

    public class OrderRepository : IOrderRepository
    {
        private DbContext _dbContext;
        private readonly IDbSet<Order> _table;

        public OrderRepository(DbContext context)
        {
            _dbContext = context;
            _table = _dbContext.Set<Order>();
        }

        public async Task<Order> AddItemsAsync(long userId, long orderId, OrderItem[] items)
        {
            // todo if exists add to qnty
            // todo check of menu item is still avialable
            var order = await GetAsync(orderId);
            order.Items = (order.Items == null)
                ? new Collection<OrderItem>(items)
                : new Collection<OrderItem>(order.Items.Concat(items).ToList());
            _table.Attach(order);
            _dbContext.Entry(order).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
            return order;
        }

        public async Task CloseAsync(long userId, long orderId)
        {
            var order = await GetAsync(orderId);
            order.State = Order.States.Closed;
            _table.Attach(order);
            _dbContext.Entry(order).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }

        public async Task<Order> GetAsync(long id)
        {
            // get our query 
            var item = await _table.FirstAsync(i => i.Id == id);

            if (item == null)
            {
                throw new EntityDoesNotExistException(string.Format("entity {0} does not exist.", id));
            }

            // return list
            return item;
        }

        public async Task<Order> OpenAsync(long userId, Order order)
        {
            if (order == null)
                throw new ArgumentNullException("entity");

            // add entity to data store
            _table.Add(order);

            // todo - move to transaction UOW
            await _dbContext.SaveChangesAsync();

            return order;
        }
    }
}
