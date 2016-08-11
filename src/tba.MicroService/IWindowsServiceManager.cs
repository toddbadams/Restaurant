namespace tba.MicroService
{
    public interface IWindowsServiceManager
    {
        void Install();
        void UnInstall();
        void Start();
        void Stop();
    }
}