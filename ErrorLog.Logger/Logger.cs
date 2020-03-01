using IdentityModel.Client;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net.Http;
using System.Text;

namespace ErrorLog.Logger
{
    public class Logger
    {
        private const string ApiScope = "LoggingAPI";

        private readonly IConfiguration _config;

        public Logger(IConfiguration config)
        {
            _config = config;
        }

        public void Log(Exception ex)
        {
            var loggingConfig = _config.GetSection("Logging");

            var loggingEndpoint = loggingConfig["Url"];
            var clientID = loggingConfig["ClientId"];
            var clientSecret = loggingConfig["ClientSecret"];
            var tokenEndpoint = loggingConfig["TokenEndpoint"];

            using (var client = new HttpClient())
            {
                var tokenTask = client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
                {
                    Address = tokenEndpoint,
                    ClientId = clientID,
                    ClientSecret = clientSecret,
                    Scope = ApiScope
                });
                tokenTask.Wait();
                var tokenResponse = tokenTask.Result;

                if (tokenResponse.IsError)
                {
                    // tokenResponse.Error;
                }

                client.SetBearerToken(tokenResponse.AccessToken);

                Send(client, loggingEndpoint, clientID, ex.Message);
                Send(client, loggingEndpoint, clientID, ex.StackTrace);

                ex = ex.InnerException;
                while (ex != null)
                {
                    Send(client, loggingEndpoint, clientID, ex.Message);
                    ex = ex.InnerException;
                }
            }
        }

        private void Send(HttpClient client, string loggingEndpoint, string clientID, string message)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, loggingEndpoint);

            request.Content = new StringContent(
                JsonConvert.SerializeObject(new { ClientId = clientID, Message = message }),
                Encoding.UTF8, "application/json");

            var task = client.SendAsync(request);
            task.Wait();
            var response = task.Result;

            if (!response.IsSuccessStatusCode)
            {
                BackupLog(clientID, message);
            }
        }

        private void BackupLog(string clientID, string message)
        {
            var appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

            var logDirectory = Path.Combine(appData, "ReservedWords", clientID, "Logs");
            Directory.CreateDirectory(logDirectory);

            var currentDate = DateTime.Now.ToString("yyyy-MM-dd");
            var filename = $"{currentDate}.log";
            var filepath = Path.Combine(logDirectory, filename);

            var formatTimestamp = DateTime.Now.ToString("HH:mm:ss");

            File.AppendAllText(filepath, $"{formatTimestamp} {message}{Environment.NewLine}");
        }
    }
}
