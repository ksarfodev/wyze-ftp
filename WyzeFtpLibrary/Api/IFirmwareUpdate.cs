using Microsoft.Extensions.Logging;
using WyzeFtpLibrary.Models;

namespace WyzeFtpLibrary.Api
{
    public interface IFirmwareUpdate
    {
        Task InitiateUpdate(string token, DeviceInfoModel deviceInfoConstants, string md5, 
            string mac, string model, string firmwareTarFileName, string firmwareUrlPath, ILogger<FirmwareUpdate> logger);
    }
}