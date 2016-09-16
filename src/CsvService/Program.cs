using log4net.Config;

namespace CsvService
{
    static class Program
    {
        static Program()
        {
            XmlConfigurator.Configure();
        }
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
//#if DEBUG
            // debug running of the service
            var myService = new CsvService();
            myService.OnDebug();
            System.Threading.Thread.Sleep(System.Threading.Timeout.Infinite);
//#else
//            // release running of the service
//            ServiceBase[] ServicesToRun;
//            ServicesToRun = new ServiceBase[] 
//            { 
//                new CsvService() 
//            };
//            ServiceBase.Run(ServicesToRun);
//#endif
        }
    }
}
