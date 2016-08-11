using System;
using System.IO;
using System.ServiceProcess;
using Csv;
using log4net;

namespace CsvService
{
    public partial class CsvService : ServiceBase
    {

        private static readonly ILog Logger = LogManager.GetLogger(typeof(CsvService));

        public CsvService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            var f = new FileWatcher();
            f.FileCreated += CreatedHandler;
        }

        private static void CreatedHandler(object sender, FileWatcherEventArgs e)
        {
            using (var file = new StreamReader(e.FullPath))
            {
                string line;
                var csvFields = CsvColumn<ImportedMenuItem>.Create();
                while ((line = file.ReadLine()) != null)
                {
                    var data = CsvLine<ImportedMenuItem>.Create(line, csvFields);
                    Logger.Info(String.Format("Line parsed: {0}", data));
                }
            }

            // all done delete input file
            File.Delete(e.FullPath);
        }

        protected override void OnStop()
        {

        }

        public void OnDebug()
        {
            OnStart(null);
        }
    }
}
