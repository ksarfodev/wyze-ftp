using Microsoft.Extensions.FileProviders;

try
{
    //pass in as arguments --folderName --customUrlPath
    var builder = WebApplication.CreateBuilder(args);

    //dns spoofing works with http 
    builder.WebHost.UseUrls("http://*8080");

    //default to wyzecam v3 firmware folder if no arguments are provided
    string folderName = args.Any() && args[0] != "" ? args[0] : "camV3Firmware";

    //default to wyzecam v3 url path if no arguments are provided
    string customPath = args.Any() && args[1] != "" ? args[1] : "/wuv2/upgrade/WYZE_CAKP2JFUS/firmware";
    
    //add services to the container.
    var app = builder.Build();

    //enable logging to console. Modify appsettings.json middleware setting
    app.UseHttpLogging();

    //configure the HTTP request pipeline.
    app.UseHttpsRedirection();

    var path = Path.Combine(Directory.GetCurrentDirectory(), @"camV3Firmware");


    if (OperatingSystem.IsLinux())
    {
        path = path.Replace("\\", "/");
    }

    Console.WriteLine(new Uri(path).AbsolutePath);


    app.UseStaticFiles(new StaticFileOptions()
    {
        FileProvider = new PhysicalFileProvider(new Uri(path).AbsolutePath),
       
        RequestPath = new PathString($"{customPath}"),
        ServeUnknownFileTypes = true,
        DefaultContentType = "application/octet-stream"

    });

    app.MapGet("/", () => "");

    Console.WriteLine($"Now serving file...");

    app.Run();
}
catch (Exception ex)
{
    Console.WriteLine(ex); ;
}