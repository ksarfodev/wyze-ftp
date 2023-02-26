using WyzeFtpLibrary.Models;

namespace WyzeFtpLibrary.Api
{
    public interface IDeviceInfo
    {
        Task<CameraInfo> GetDeviceInfo(string token, DeviceInfoModel deviceInfoTemplate, string mac);
    }
}