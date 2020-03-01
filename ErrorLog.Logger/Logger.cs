using IdentityModel.Client;
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

        private readonly string _clientId;
        private readonly string _clientSecret;
        private readonly string _tokenEndpoint;
        private readonly string _loggingEndpoint;

        public Logger(string clientId, string clientSecret, string tokenEndpoint, string loggingEndpoint)
        {
            _clientId = clientId;
            _clientSecret = clientSecret;
            _tokenEndpoint = tokenEndpoint;
            _loggingEndpoint = loggingEndpoint;
        }

        public void Log(Exception ex)
        {
            using (var client = new HttpClient())
            {
                var tokenTask = client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
                {
                    Address = _tokenEndpoint,
                    ClientId = _clientId,
                    ClientSecret = _clientSecret,
                    Scope = ApiScope
                });
                tokenTask.Wait();
                var tokenResponse = tokenTask.Result;

                if (tokenResponse.IsError)
                {
                    // tokenResponse.Error;
                }

                client.SetBearerToken(tokenResponse.AccessToken);

                Send(client, ex.Message);
                Send(client, ex.StackTrace);

                ex = ex.InnerException;
                while (ex != null)
                {
                    Send(client, ex.Message);
                    ex = ex.InnerException;
                }
            }
        }

        private void Send(HttpClient client, string message)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, _loggingEndpoint);

            request.Content = new StringContent(
                JsonConvert.SerializeObject(new { _clientId, message }),
                Encoding.UTF8, "application/json");

            var task = client.SendAsync(request);
            task.Wait();
            var response = task.Result;

            if (!response.IsSuccessStatusCode)
            {
                BackupLog(message);
            }
        }

        private void BackupLog(string message)
        {
            var appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

            var logDirectory = Path.Combine(appData, "ReservedWords", _clientId, "Logs");
            Directory.CreateDirectory(logDirectory);

            var currentDate = DateTime.Now.ToString("yyyy-MM-dd");
            var filename = $"{currentDate}.log";
            var filepath = Path.Combine(logDirectory, filename);

            var formatTimestamp = DateTime.Now.ToString("HH:mm:ss");

            File.AppendAllText(filepath, $"{formatTimestamp} {message}{Environment.NewLine}");
        }
    }
}
