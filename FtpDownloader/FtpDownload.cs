using FluentFTP;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using WyzeFtpLibrary;
using WyzeFtpLibrary.Api;

namespace FtpDownloader
{
    public static class FtpDownload
    {
        public static async Task DownloadContent(ILogger logger, IConfigurationRoot config, IPushover notification, CancellationToken ct)
        {
            var localRecordFolder = config.GetSection("LocalRecordFolder").Value;
            var localAlarmFolder = config.GetSection("LocalAlarmFolder").Value;
            var ftpUser = config.GetSection("FtpUser").Value;
            var ftpPassword = config.GetSection("FtpPassword").Value;

            foreach (var item in CameraCollection.Dictionary)
            {
                try
                {
                    logger.LogInformation($"Downloading files from {item.Value.Item1}...");

                    using var ftp = new FtpClient($"{item.Value.Item2}", ftpUser, ftpPassword);
                    logger.LogInformation($"{localRecordFolder}{item.Value.Item1}");
                    //download record folder recursively
                    await ftp.DownloadDirectoryAsync($"{localRecordFolder}{item.Value.Item1}", @"/record",
                        FtpFolderSyncMode.Update, token: ct);

                    //download alarm folder recursively
                    await ftp.DownloadDirectoryAsync($"{localAlarmFolder}{item.Value.Item1}", @"/alarm",
                      FtpFolderSyncMode.Update, token: ct);

                    logger.LogInformation($"Finished downloading files from {item.Value.Item1}.");
                    
                }
                catch (Exception ex)
                {
                    logger.LogError(ex.Message);
                    logger.LogWarning($"Files from {item.Value.Item1} failed to download.");
                    //the following will exit if Pushover isn't being used
                    await notification.TrySendNotification($"{item.Value.Item1} failed to download.");
                }
            }
        }
    }
}
