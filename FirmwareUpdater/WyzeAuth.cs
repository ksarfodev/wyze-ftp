using WyzeFtpLibrary.Api;
using WyzeFtpLibrary.Models;

namespace FirmwareUpdater;

public class WyzeAuth
{
    private readonly IAccessToken _api;
    public WyzeAuth(IAccessToken api)
    {
        _api = api;
    }

    public async Task Authorize(List<string> credentials)
    {
        await _api.GetAccessToken( new WyzeCredentials
        {
            Email = credentials[0],
            Password = credentials[1]
        });
    }
}
