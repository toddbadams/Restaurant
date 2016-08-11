using System.IO;
using Microsoft.Owin.Hosting;
using System;
using NDesk.Options;

namespace tba.MicroService
{
    public class MicroService
    {
        public event Action OnServiceStarted;
        public event Action OnServiceStopped;

        private readonly IWindowsServiceManager _windowsServiceManager;
        private readonly MicroServiceOptions _options;
        private readonly OptionSet _optionSet;
        private SelfHostProvider _selfHostServer;

        public MicroService(IWindowsServiceManager windowsServiceManager, MicroServiceOptions options) 
        {
            _windowsServiceManager = windowsServiceManager;
            _options = options;
            _optionSet = ServiceOptions();

            InternalService.OsStarted += Start;
            InternalService.OsStopped += Stop;
            ProjectInstaller.InitInstaller(options.DisplayName, options.ServiceName);
        }

        public void Run(string[] args)
        {
            _optionSet.Parse(args);
        }

        private OptionSet ServiceOptions()
        {
            return new OptionSet()
            { 
                {
                    "run", 
                    "One time run of the windows service.", t =>
                    {
                        Start();
                        Console.WriteLine("Press any key to exit...");
                        Console.ReadLine();
                        Stop();
                    }
                },
                {
                    "install", 
                    "Install as a windows service.", t =>
                    {
                        _windowsServiceManager.Install();
                        _windowsServiceManager.Start();
                    }
                },
                {
                    "uninstall", 
                    "Uninstall windows service", o =>
                    {
                        _windowsServiceManager.Stop();
                        _windowsServiceManager.UnInstall();
                    }
                },
                {
                    "h|help",
                    "show this message and exit", v =>
                    {
                        var sw = new StringWriter();
                        sw.Write("Microservice options:" + Environment.NewLine);

                        _optionSet.WriteOptionDescriptions(sw);

                        Console.WriteLine(sw.ToString());
                        Console.ReadLine();
                    }
                },
            };
        }

        private void Stop()
        {
            _selfHostServer.Dispose();
            if (OnServiceStopped != null)
                OnServiceStopped.Invoke();
        }

        private void Start()
        {
            var options = new StartOptions(_options.Uri)
            {
                Port = _options.Port
            };
            _selfHostServer = new SelfHostProvider(options);
            _selfHostServer.Connect();
            if (OnServiceStarted != null)
                OnServiceStarted.Invoke();
        }
    }
}