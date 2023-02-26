using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using WyzeFtpLibrary.Models;

namespace WyzeFtpLibrary.Api
{

    public class Pushover : BaseHttpClient, IPushover
    {
        public Pushover(IHttpClientFactory httpClientFactory) : base(httpClientFactory)
        {
        }

        private PushoverCredentials _credentials = new();

        public async Task TrySendNotification(string msg)
        {
            try
            {
                //if not using Pushover or invalid credentials provided, exit
                if(_credentials.token == null && _credentials.user == null)
                {
                    return;
                }

                var url = "https://api.pushover.net/1/messages.json";
                var json = new
                {
                    token = _credentials.token,
                    user = _credentials.user,
                    message = msg,
                };

                string jsonString = JsonSerializer.Serialize(json);
                var payload = new StringContent(jsonString, Encoding.UTF8, "application/json");

                var httpRequestMessage = new HttpRequestMessage(
                    HttpMethod.Post, url)
                {
                    Content = payload,
                    Headers =
                        {
                            {"User-Agent","okhttp/3.8.1"},
                        }
                };

                var response = await ProcessRequest(httpRequestMessage, url);
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public void SetCredentials(List<string> credentials)
        {
            _credentials.token = credentials[0];
            _credentials.user = credentials[1];
        }
    }
}
