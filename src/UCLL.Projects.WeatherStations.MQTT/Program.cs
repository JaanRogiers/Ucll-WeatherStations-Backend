using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace UCLL.Projects.WeatherStations.MQTT;

internal static class Program
{
    private static async Task Main(string[] args)
    {
        IHost host = Host.CreateDefaultBuilder()
            .ConfigureHostConfiguration(hostConfigBuilder =>
            {
                //hostConfigBuilder.SetBasePath(Directory.GetCurrentDirectory());
            })
            .ConfigureAppConfiguration((hostBuilderContext, appConfigBuilder) =>
            {
                IHostEnvironment environment = hostBuilderContext.HostingEnvironment;

                appConfigBuilder.Sources.Clear();

                //appConfigBuilder.SetBasePath(environment.ContentRootPath);
                appConfigBuilder.AddConfiguration(hostBuilderContext.Configuration);
                appConfigBuilder.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                appConfigBuilder.AddJsonFile($"appsettings.{environment.EnvironmentName}.json", optional: true, reloadOnChange: true);
                appConfigBuilder.AddEnvironmentVariables(prefix: "WEATHERSTATIONS_MQTT_");
            })
            .ConfigureServices((hostBuilderContext, services) =>
            {
                /*
                 * add mqtt service
                 * add task queue + service
                 * add dbcontext
                 */
            })
            .ConfigureLogging(logBuilder =>
            {
                logBuilder.SetMinimumLevel(LogLevel.Information);
                logBuilder.ClearProviders();

                logBuilder.AddConsole();
            })
            .Build();

        await host.RunAsync(); // blocks until the host is shutdown
        /*
        await host.StartAsync(); // doesn't block
        Console.WriteLine("Host started...");
        await host.WaitForShutdownAsync();
        */
    }
}