using System.Text;
using WyzeFtpLibrary.Models;
using System.Text.Json;
using System.Net.Http.Json;
using Microsoft.Extensions.Logging;

namespace WyzeFtpLibrary.Api
{
    public class FirmwareUpdate : BaseHttpClient, IFirmwareUpdate
    {
        public FirmwareUpdate(IHttpClientFactory httpClientFactory) : base(httpClientFactory)
        {
        }

        public async Task InitiateUpdate(string token, DeviceInfoModel deviceInfoConstants,
           string md5, string mac, string model, string firmwareTarFileName,
           string firmwareUrlPath, ILogger<FirmwareUpdate> logger)
        {
            var url = "https://api.wyzecam.com/app/v2/auto/run_action";

            var json = new
            {
                access_token = token,
                action_key = "upgrade",
                action_params = new
                {
                    md5,
                    model, 
                    url = $"{firmwareUrlPath}{firmwareTarFileName}"
                },

                app_name = deviceInfoConstants.App_name,
                app_version = deviceInfoConstants.App_version,
                custom_string = "",
                instance_id = mac,
                phone_id = deviceInfoConstants.Phone_id,
                phone_system_type = deviceInfoConstants.Phone_system_type,
                app_ver = deviceInfoConstants.App_ver,
                provider_key = model,

                sc = deviceInfoConstants.Sc,
                sv = deviceInfoConstants.Sv,
                ts = new DateTimeOffset(DateTime.Now).ToUnixTimeMilliseconds(),
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

            string result = await response.Content.ReadAsStringAsync();

            //print result
            logger.LogInformation(result);
        }
    }
}
