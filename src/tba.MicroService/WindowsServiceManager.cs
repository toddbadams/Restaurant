using System;
using System.Collections;
using System.Configuration.Install;
using System.Reflection;
using System.ServiceProcess;

namespace tba.MicroService
{
    public class WindowsServiceManager : IWindowsServiceManager
    {
        private readonly string _serviceDisplayName;

        public WindowsServiceManager(string serviceDisplayName)
        {
            _serviceDisplayName = serviceDisplayName;
        }

        public void Install()
        {
            if (IsInstalled()) return;
            using (AssemblyInstaller installer = GetInstaller())
            {
                IDictionary state = new Hashtable();
                try
                {
                    installer.Install(state);
                    installer.Commit(state);
                }
                catch
                {
                    try
                    {
                        installer.Rollback(state);
                    }
                    catch { }
                    throw;
                }
            }
        }

        public void UnInstall()
        {
            if (!IsInstalled()) return;
            using (var installer = GetInstaller())
            {
                IDictionary state = new Hashtable();
                installer.Uninstall(state);
            }
        }

        public void Start()
        {
            if (!IsInstalled()) return;
            using (var controller = new ServiceController(_serviceDisplayName))
            {
                if (controller.Status == ServiceControllerStatus.Running) return;
                controller.Start();

                controller.WaitForStatus(ServiceControllerStatus.Running, TimeSpan.FromSeconds(10));
            }
        }

        public void Stop()
        {
            if (!IsInstalled()) return;
            using (var controller = new ServiceController(_serviceDisplayName))
            {
                if (controller.Status == ServiceControllerStatus.Stopped) return;
                controller.Stop();
                controller.WaitForStatus(ServiceControllerStatus.Stopped, TimeSpan.FromSeconds(10));
            }
        }


        private static AssemblyInstaller GetInstaller()
        {
            var serviceExeName = Assembly.GetEntryAssembly().ManifestModule.Name;
            return new AssemblyInstaller(serviceExeName, null) { UseNewContext = true };
        }

        private bool IsInstalled()
        {
            using (var controller = new ServiceController(_serviceDisplayName))
            {
                try
                {
                    var status = controller.Status;
                }
                catch (Exception)
                {
                    return false;
                }
                return true;
            }
        }
    }
}