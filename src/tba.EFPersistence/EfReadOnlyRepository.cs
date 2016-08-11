using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using tba.Core.Entities;
using tba.Core.Exceptions;
using tba.Core.Persistence.Interfaces;

namespace tba.EFPersistence
{
    /// <summary>
    ///     Entity Framework based read-only repository.
    ///  Conventionv - Get for a single, Fetch for a collection
    /// </summary>
    public class EfReadOnlyRepository<T> : IReadOnlyRepository<T> where T : Entity
    {
        internal EfReadOnlyRepository(DbContext context)
        {
            // set repository context (holds entity framework info)
            Context = context;
            Table = Context.Set<T>();
        }

        #region Private Vars

        internal readonly DbContext Context; // the database context
        internal readonly IDbSet<T> Table; // the table we are working with
        private bool _disposed; // track if disposed

        #endregion

        #region public CRUD Methods

        /// <summary>
        ///     Query from the data store
        /// </summary>
        /// <returns>List of entites</returns>
        public IQueryable<T> Query()
        {

            // return table as a query
            return Table.OfType<T>();
        }


        /// <summary>
        ///     check for the existance a single record from the data store
        /// </summary>
        /// <param name="id">id of the record</param>
        /// <returns>returns first or default entity</returns>
        public bool Exists(long id)
        {
            return Table.Any(i => i.Id == id);
        }

        /// <summary>
        ///     Get a single record from the data store
        /// </summary>
        /// <param name="id">id of the record</param>
        /// <returns>returns first or default entity</returns>
        public async Task<T> GetAsync(long id)
        {
            // get our query 
            var item = await Table.OfType<T>().FirstAsync(i => i.Id == id);

            if (item == null)
            {
                throw new EntityDoesNotExistException(string.Format("entity {0} does not exist.", id));
            }

            // return list
            return item;
        }

        #endregion

        #region Dispose

        /// <summary>
        ///     dispose
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        ///     dispose
        /// </summary>
        /// <param name="disposing">are we currently disposing</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    Context.Dispose();
                }
            }
            _disposed = true;
        }

        #endregion
    }
}