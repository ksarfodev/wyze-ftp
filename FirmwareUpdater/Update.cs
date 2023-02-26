using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using WyzeFtpLibrary.Api;
using WyzeFtpLibrary.Helpers;
using WyzeFtpLibrary.Models;

namespace FirmwareUpdater;

public class Updater
{
    private readonly IFirmwareUpdate _api;

    public Updater(IFirmwareUpdate api)
    {
        _api = api;
    }

    public async Task PerformUpdate(IConfigurationRoot config, List<string> updateList, ILogger<FirmwareUpdate> logger)
    {
        //read access token
        var tokenJson = new ConfigurationBuilder()
                      .SetBasePath(Directory.GetCurrentDirectory())
                      .AddJsonFile("token.json")
                      .Build();

        var accessToken = tokenJson.GetSection("Access_token").Value;

        //get firmware file name 
        var firmwareTarFileName = config.GetSection("FirmwareTarFileName").Value;

        //get directory containing *.tar file being served by FileServer
        var firmwareFileLocation = config.GetSection("FirmwareFileLocation").Value;

        var firmwareUrlPath = config.GetSection("FirmwareUrlPath").Value;

        var model = config.GetSection("Model").Value;

        var deviceInfoJson = File.ReadAllText("deviceInfo.json");

        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = new LowerCaseNamingPolicy(),
            WriteIndented = true
        };

        DeviceInfoModel deserializedInfo =
            JsonSerializer.Deserialize<DeviceInfoModel>(deviceInfoJson, options)!;

        //get the md5 checksum of the [*.tar] file being served by FileServer. Value must be lowercase
        var md5 = MD5Helper.GetMD5Checksum($"{firmwareFileLocation}").ToLower();
        
        logger.LogInformation($"MD5: {md5}");

        //initiate firmware upgrade for each camera in upgrade list
        foreach (var macAddress in updateList)
        {
            await _api.InitiateUpdate(accessToken!, deserializedInfo, md5, macAddress,
                model!, firmwareTarFileName!, firmwareUrlPath!,logger);
        }
    }
}
