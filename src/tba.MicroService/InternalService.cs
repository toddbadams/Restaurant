using System;
using System.ServiceProcess;

namespace tba.MicroService
{
    public class InternalService : ServiceBase
    {
        internal static event Action OsStarted;
        internal static event Action OsStopped;

        protected override void OnStart(string[] args)
        {
            OsStarted.Invoke();
        }

        protected override void OnStop()
        {
            OsStopped.Invoke();
        }
    }
}