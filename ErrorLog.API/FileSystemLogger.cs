using ErrorLog.Common;
using System;
using System.IO;

namespace ErrorLog.API
{
    public class FileSystemLogger : IBackupLogger
    {
        public void Log(Log log)
        {
            var appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            
            var logDirectory = Path.Combine(appData, "ReservedWords", log.AppName, "Logs");
            Directory.CreateDirectory(logDirectory);
            
            var currentDate = DateTime.Now.ToString("yyyy-MM-dd");
            var filename = $"{currentDate}.log";
            var filepath = Path.Combine(logDirectory, filename);

            var formatTimestamp = log.Timestamp.ToString("HH:mm:ss");

            File.AppendAllText(filepath, $"{formatTimestamp}: {log.Message}");
        }
    }
}
