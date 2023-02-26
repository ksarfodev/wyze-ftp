
using System.Text;
using WyzeFtpLibrary.Models;
using System.Text.Json;


namespace WyzeFtpLibrary.Api
{
    public class AccessToken : BaseHttpClient, IAccessToken
    {
        public AccessToken(IHttpClientFactory httpClientFactory) : base(httpClientFactory)
        {
        }

        public async Task GetAccessToken(WyzeCredentials creds)
        {
            var url = "https://auth-prod.api.wyze.com/user/login";
            var json = new
            {
                email = creds.Email,
                password = creds.Password,
            };

            string jsonString = JsonSerializer.Serialize(json);
            var payload = new StringContent(jsonString, Encoding.UTF8, "application/json");

            var httpRequestMessage = new HttpRequestMessage(
                HttpMethod.Post, url)
            {
                Content = payload,
                Headers =
            {
                    {"Phone-Id","c0d21a02-c817-4223-8665-6e5b96111d84"},
                    {"User-Agent","wyze_android/2.23.21  (Pixel 4; Android 11; Scale/2.75; Height/2148; Width/1080)" },
                    {"X-API-Key","RckMFKbsds5p6QY3COEXc2ABwNTYY0q18ziEiSEm" }
                }
            };

            var response = await ProcessRequest(httpRequestMessage, url);
            string responseJson = await response.Content.ReadAsStringAsync();

            File.WriteAllText("token.json", responseJson);
        }
    }
}
