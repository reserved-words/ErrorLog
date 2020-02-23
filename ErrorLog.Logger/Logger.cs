using Newtonsoft.Json;
using System;
using System.IO;
using System.Net.Http;
using System.Text;

namespace ErrorLog.Logger
{
    public class Logger
    {
        private readonly string _url;
        private readonly string _appName;

        public Logger(string url, string appName)
        {
            _url = url;
            _appName = appName;
        }

        public void Log(Exception ex)
        {
            using (var httpClient = new HttpClient())
            {
                Send(httpClient, _url, _appName, ex.Message);
                Send(httpClient, _url, _appName, ex.StackTrace);

                ex = ex.InnerException;
                while (ex != null)
                {
                    Send(httpClient, _url, _appName, ex.Message);
                    ex = ex.InnerException;
                }
            }
        }

        private void Send(HttpClient httpClient, string url, string appName, string message)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, url);

            request.Content = new StringContent(
                JsonConvert.SerializeObject(new { appName, message }),
                Encoding.UTF8, "application/json");

            var task = httpClient.SendAsync(request);
            task.Wait();
            var response = task.Result;

            if (!response.IsSuccessStatusCode)
            {
                BackupLog(appName, message);
            }
        }

        private void BackupLog(string appName, string message)
        {
            var appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

            var logDirectory = Path.Combine(appData, "ReservedWords", appName, "Logs");
            Directory.CreateDirectory(logDirectory);

            var currentDate = DateTime.Now.ToString("yyyy-MM-dd");
            var filename = $"{currentDate}.log";
            var filepath = Path.Combine(logDirectory, filename);

            var formatTimestamp = DateTime.Now.ToString("HH:mm:ss");

            File.AppendAllText(filepath, $"{formatTimestamp} {message}{Environment.NewLine}");
        }
    }
}
