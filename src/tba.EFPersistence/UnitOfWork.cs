using System.Data.Entity;
using tba.Core.Persistence;

namespace tba.EFPersistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DbContext _dbContext;

        public UnitOfWork(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public ITransaction BeginTransaction()
        {
            return new Transaction(_dbContext);
        }
    }   
}
