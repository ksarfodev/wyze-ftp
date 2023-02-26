using WyzeFtpLibrary.Models;

namespace WyzeFtpLibrary.Api
{
    public interface IAccessToken
    {
        Task GetAccessToken(WyzeCredentials creds);
    }
}