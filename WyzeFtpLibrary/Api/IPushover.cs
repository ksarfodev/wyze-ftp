using WyzeFtpLibrary.Models;

namespace WyzeFtpLibrary.Api
{
    public interface IPushover
    {
        Task TrySendNotification(string msg);
        void SetCredentials(List<string> credentials);
    }
}