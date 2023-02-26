using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using WyzeFtpLibrary;
using WyzeFtpLibrary.Api;
using WyzeFtpLibrary.Helpers;
using WyzeFtpLibrary.Models;

namespace FtpDownloader
{
    public class CameraList
    {
        private readonly IDeviceInfo _api;
        public CameraList(IDeviceInfo api)
        {
            _api = api;
        }

        public async Task GetCameras(IConfigurationRoot config,ILogger<CameraList> logger)
        {
            var accessToken = config.GetSection("WyzeAccessToken").Value;
            var ftpUser = config.GetSection("FtpUser").Value;
            var ftpPassword = config.GetSection("FtpPassword").Value;

            var macList = config.GetSection($"Cameras").GetChildren()
                                     .ToArray()
                                     .Select(c => c.Value)
                                     .ToList();

            
            if (!File.Exists("wyzeCameraList.json"))
            {
                await CreateCameraListJson(macList, accessToken,logger);
            }
            else
            {
                //no api call needed, fill CameraCollection
                string cameras = File.ReadAllText("wyzeCameraList.json");
                CameraCollection.Dictionary = JsonSerializer.Deserialize<Dictionary<string, Tuple<string, string>>>(cameras)!;
            }
        }

        private async Task CreateCameraListJson(List<string> macList, string accessToken,ILogger<CameraList> logger)
        {
            try
            {
                var deviceInfoJson = File.ReadAllText("deviceInfo.json");

                var options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = new LowerCaseNamingPolicy(),
                    WriteIndented = true
                };

                DeviceInfoModel deserializedInfo =
                    JsonSerializer.Deserialize<DeviceInfoModel>(deviceInfoJson, options)!;

                //for each mac address get ip address and other info then add to dictionary
                foreach (var macAddress in macList)
                {
                    //make api call
                    var output = await _api.GetDeviceInfo(accessToken, deserializedInfo, macAddress);

                    if (output.data != null)
                    {
                        CameraCollection.Dictionary[output.data.mac] = new Tuple<string, string>(output.data.nickname, output.data.ip);
                    }
                }

                logger.LogInformation("Creating wyzeCameraList.json file.");
                //write the list of cameras to a json file so it's not necessary to make API calls each time.
                File.WriteAllText("wyzeCameraList.json", JsonSerializer.Serialize(CameraCollection.Dictionary));

            }
            catch (Exception ex)
            {

                logger.LogCritical(ex.Message);
            }
            }
    }
}
