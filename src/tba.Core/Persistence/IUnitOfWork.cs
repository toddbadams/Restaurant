namespace tba.Core.Persistence
{
    public interface IUnitOfWork
    {
        ITransaction BeginTransaction();
    }
}
