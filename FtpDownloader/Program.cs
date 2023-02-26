
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using FtpDownloader;
using WyzeFtpLibrary.Api;
using System.IO;

ILogger<CameraList> _logger;

var config = new ConfigurationBuilder()
                 .SetBasePath(Directory.GetCurrentDirectory())
                 .AddJsonFile("appSettings.json")
                 .Build();

var currentDir = Directory.GetCurrentDirectory();

// To create a string that specifies the path to a subfolder under your
// top-level folder, add a name for the subfolder to folderName.
string logsPathString = System.IO.Path.Combine(currentDir, "logs",
    $"FtpDownloader_{DateTime.Now.ToString("yy-MMM-dd-hh-mm-ss")}.log");



var builder = new HostBuilder()
         .ConfigureServices((hostContext, services) =>
{
    services.AddScoped<Program>();
    services.AddLogging(configure => configure.AddConsole());
    services.AddSingleton<IDeviceInfo, DeviceInfo>();
    services.AddSingleton<IPushover, Pushover>();
    services.AddSingleton<CameraList>();
    services.AddHttpClient();

    services.AddLogging(loggingBuilder =>
         loggingBuilder.AddFile(logsPathString, append: true));
});

var host = builder.Build();

using (var serviceScope = host.Services.CreateScope())
{
    var services = serviceScope.ServiceProvider;

    _logger = services.GetRequiredService<ILogger<CameraList>>();
    _logger.LogInformation("Starting...");

    var cameras = services.GetRequiredService<CameraList>();
    var notification = services.GetRequiredService<IPushover>();

    List<string?> pushoverCredentials = config.GetSection($"PushoverCredentials").GetChildren()
                          .ToArray()
                          .Select(c => c.Value)
                          .ToList();

    //if pushover credentials are provided in appSettings, pass along
    if (pushoverCredentials.Count > 0)
    {
        notification.SetCredentials(pushoverCredentials);
    }

    //populate CameraCollection with IP addresses
    await cameras.GetCameras(config,_logger);

    //download content
    var ct = new CancellationToken();
    await FtpDownload.DownloadContent(_logger, config, notification, ct);
}