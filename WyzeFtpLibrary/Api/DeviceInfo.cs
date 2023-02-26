
using System.Text;
using WyzeFtpLibrary.Models;
using System.Text.Json;
using System.Net.Http.Json;
using Microsoft.Extensions.Options;
using System.Text.Json.Nodes;

namespace WyzeFtpLibrary.Api
{
    public class DeviceInfo : BaseHttpClient, IDeviceInfo
    {
        public DeviceInfo(IHttpClientFactory httpClientFactory) : base(httpClientFactory)
        {
        }

        public async Task<CameraInfo> GetDeviceInfo(string token, DeviceInfoModel deviceInfoTemplate, string mac)
        {
            var url = "https://api.wyzecam.com/app/v2/device/get_device_info";

            var json = new
            {
                access_token = token,
                app_name = deviceInfoTemplate.App_name,
                app_version = deviceInfoTemplate.App_version,
                phone_system_type = deviceInfoTemplate.Phone_system_type,
                app_ver = deviceInfoTemplate.App_ver,
                phone_id = deviceInfoTemplate.Phone_id,
                sc = deviceInfoTemplate.Sc,
                sv = deviceInfoTemplate.Sv,
                ts = new DateTimeOffset(DateTime.Now).ToUnixTimeMilliseconds(),
                device_mac = mac, 
                device_model = deviceInfoTemplate.Device_model
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

            CameraInfo? result = await response.Content.ReadFromJsonAsync<CameraInfo>();

            return result;

        }
    }
}