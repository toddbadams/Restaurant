using System;
using System.Configuration;
using System.IO;
using System.Linq;
using log4net;

namespace Csv
{
    [System.ComponentModel.DefaultEvent("FileCreated")]
    public class FileWatcher
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(FileWatcher));

        public FileWatcher()
        {
            var fileWatcher = new FileSystemWatcher(PathLocation());

            fileWatcher.Created += _fileWatcher_Created;
            fileWatcher.Deleted += _fileWatcher_Deleted;
            fileWatcher.EnableRaisingEvents = true;
        }

        public event FileWatcherEventHandler FileCreated;

        protected virtual void OnFileCreated(FileWatcherEventArgs e)
        {
            var handler = FileCreated;
            if (handler != null) handler(this, e);
        }

        private static string PathLocation()
        {
            var value = String.Empty;

            try
            {
                value = ConfigurationManager.AppSettings["location"];
            }
            catch (Exception ex)
            {
                Logger.Error("error getting directory location", ex);
            }

            return value;
        }

        private static void _fileWatcher_Deleted(object sender, FileSystemEventArgs e)
        {
            Logger.Info(String.Format("File Deleted: {0}", e.FullPath));
        }

        private void _fileWatcher_Created(object sender, FileSystemEventArgs e)
        {
            Logger.Info(String.Format("File Created: {0}", e.FullPath));
            var maxLines = int.Parse(ConfigurationManager.AppSettings["maxlines"]);

            // split if too large
            var lineCount = File.ReadLines(e.FullPath).Count();
            if (lineCount > maxLines)
            {
                FileParser.SplitFile(e.FullPath, maxLines);
            }
            else
            {
                // raise event, provide filename to parse
                OnFileCreated(new FileWatcherEventArgs { FullPath = e.FullPath });
            }
        }
    }
}
