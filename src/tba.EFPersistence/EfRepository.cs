using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using tba.Core.Entities;
using tba.Core.Persistence.Interfaces;
using tba.Core.Utilities;

namespace tba.EFPersistence
{
    /// <summary>
    ///     Entity Framework based read-write repository.
    /// </summary>
    public sealed class EfRepository<T> : EfReadOnlyRepository<T>, IRepository<T> where T : Entity
    {
        private readonly ITimeProvider _timeProvider;

        public EfRepository(DbContext context, ITimeProvider timeProvider)
            : base(context)
        {
            _timeProvider = timeProvider;
        }

        #region public CRUD Methods

        /// <summary>
        ///     Create an entity in the data store
        /// </summary>
        /// <param name="userId">user id used to capture in audit</param>
        /// <param name="entity">entity to create</param>
        public async Task<T> InsertAsync(long userId, T entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            // add entity to data store
            Table.Add(entity);
            
            // update the data store
            await Context.SaveChangesAsync();

            return entity;
        }

        /// <summary>
        ///     Update an entity in the data store
        /// </summary>
        /// <param name="userId">user id used to capture in audit</param>
        /// <param name="entity">entity to update</param>
        public async Task<T> UpdateAsync(long userId, T entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");
            
            // update to data store
            Table.Attach(entity);
            Context.Entry((Entity)entity).State = EntityState.Modified;
            await Context.SaveChangesAsync();
            return entity;
        }

        /// <summary>
        ///     Update entities in the data store
        /// </summary>
        /// <param name="userId">user id used to capture in audit</param>
        /// <param name="entities">entities to update</param>
        public async Task<IEnumerable<T>> UpdateAsync(long userId, IQueryable<T> entities)
        {
            if (entities == null)
                throw new ArgumentNullException("entities");

            // process unit of work prior to update in data store
            foreach (var entity in entities)
            {
                Table.Attach(entity);
                Context.Entry((Entity)entity).State = EntityState.Modified;
            }

            // update to data store  
            await Context.SaveChangesAsync();
            return entities.AsEnumerable();
        }

        /// <summary>
        ///     Delete an entities that match the domainFilter from the data store
        /// </summary>
        /// <param name="userId">user id used to capture in audit</param>
        /// <param name="entities">entities to delete</param>
        public async Task DeleteAsync(long userId, IQueryable<T> entities)
        {
            if (entities == null)
                throw new ArgumentNullException("entities");

            // loop and delete
            foreach (T entity in entities)
            {
                if (Context.Entry(entity).State == EntityState.Detached)
                    Table.Attach(entity);

                Table.Remove(entity);
            }

            // comit the changes
            await Context.SaveChangesAsync();
        }

        /// <summary>
        ///     Delete an entities that match the domainFilter from the data store
        /// </summary>
        /// <param name="userId">user id used to capture in audit</param>
        /// <param name="id">id of the record</param>
        public async Task DeleteAsync(long userId, long id)
        {
            //if (id == null || id == long.Empty)
            //    throw new ArgumentException("id");

            // get our entity
            T entity = Table.Find(id);
            
            //  delete
            if (Context.Entry(entity).State == EntityState.Detached)
                Table.Attach(entity);
            Table.Remove(entity);

            // comit the changes
            await Context.SaveChangesAsync();
        }
        #endregion


    }
}