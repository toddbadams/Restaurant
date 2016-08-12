using System;
using System.Data.Entity;
using tba.Core.Persistence;

namespace tba.EFPersistence
{
    public class Transaction : ITransaction
    {
        private readonly DbContextTransaction _transaction;

        public Transaction(DbContext dbContext)
        {
            _transaction = dbContext.Database.BeginTransaction();
        }

        public void Commit()
        {
            _transaction.Commit();
        }

        public void Rollback()
        {
            _transaction.Rollback();
        }

        public void Dispose()
        {
            _transaction.Dispose();
        }

    }
}
