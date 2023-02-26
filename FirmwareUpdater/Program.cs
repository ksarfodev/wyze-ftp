
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using FirmwareUpdater;
using WyzeFtpLibrary.Api;

ILogger<FirmwareUpdate> _logger;


var config = new ConfigurationBuilder()
                 .SetBasePath(Directory.GetCurrentDirectory())
                 .AddJsonFile("appSettings.json")
                 .Build();

var builder = new HostBuilder()
         .ConfigureServices((hostContext, services) =>
         {
             services.AddScoped<Program>();
             services.AddLogging(configure => configure.AddConsole());
             services.AddSingleton<IFirmwareUpdate, FirmwareUpdate>();
             services.AddSingleton<IAccessToken, AccessToken>();
             services.AddSingleton<Updater>();
             services.AddSingleton<WyzeAuth>();
             services.AddHttpClient();

             services.AddLogging(loggingBuilder =>
                  loggingBuilder.AddFile($"FirmwareUpdater_{DateTime.Now.ToString("yy-MMM-dd-hh-mm-ss")}.log", append: true));
         });

var host = builder.Build();

using (var serviceScope = host.Services.CreateScope())
{
    var services = serviceScope.ServiceProvider;

    _logger = services.GetRequiredService<ILogger<FirmwareUpdate>>();
    _logger.LogInformation("Starting...");

    var auth = services.GetRequiredService<WyzeAuth>();
    var updater = services.GetRequiredService<Updater>();

    //get values from config file
    var updateList = config.GetSection($"CamerasToUpdate").GetChildren()
                             .ToArray()
                             .Select(c => c.Value)
                             .ToList();

    List<string?> credentials = config.GetSection($"Credentials").GetChildren()
                             .ToArray()
                             .Select(c => c.Value)
                             .ToList();

    //The FileServer should be serving the *.tar firmware file
    Console.WriteLine("DNS spoofing should be enabled and FileServer should be running.");

    //if token file does not exists, get access token using credentials then save to a json file
    if (!File.Exists("token.json"))
    {
        await auth.Authorize(credentials!);
    }

    //check for minimum of 1 camera to update 
    if (updateList.Count > 0)
    {
        try
        {
            await updater.PerformUpdate(config, updateList!,_logger);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
        }
    }
}