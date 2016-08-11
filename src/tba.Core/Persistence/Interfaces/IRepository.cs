﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using tba.Core.Entities;

namespace tba.Core.Persistence.Interfaces
{
    /// <summary>
    ///     Represents a read/write repository of IRna objects.
    /// </summary>
    public interface IRepository<T> : IReadOnlyRepository<T> where T : Entity
    {
        /// <summary>
        ///     Create an entity in the data store
        /// </summary>
        /// <param name="userId">user id used to capture in audit</param>
        /// <param name="entity">entity to create</param>
        Task<T> InsertAsync(long userId, T entity);

        /// <summary>
        ///     Update an entity in the data store
        /// </summary>
        /// <param name="userId">user id used to capture in audit</param>
        /// <param name="entity">entity to update</param>
        Task<T> UpdateAsync(long userId, T entity);

        /// <summary>
        ///     Update entities in the data store
        /// </summary>
        /// <param name="userId">user id used to capture in audit</param>
        /// <param name="entities">enumerable collection of entities to update</param>
        Task<IEnumerable<T>> UpdateAsync(long userId, IQueryable<T> entities);

        /// <summary>
        ///     Delete an entities that match the domainFilter from the data store
        /// </summary>
        /// <param name="userId">user id used to capture in audit</param>
        /// <param name="domainFilter">domainFilter of entities to delete</param>
        Task DeleteAsync(long userId, IQueryable<T> domainFilter);

        /// <summary>
        ///     Delete an entities that match the domainFilter from the data store
        /// </summary>
        /// <param name="userId">user id used to capture in audit</param>
        /// <param name="id">id of the record</param>
        Task DeleteAsync(long userId, long id);
    }
}