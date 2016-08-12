using System;

namespace tba.Core.Persistence
{
    public interface ITransaction : IDisposable
    {
        void Commit();
        void Rollback();
    }
}
