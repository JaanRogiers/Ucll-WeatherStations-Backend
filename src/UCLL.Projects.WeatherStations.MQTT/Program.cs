using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using UCLL.Projects.WeatherStations.MQTT.Services;
using UCLL.Projects.WeatherStations.MQTT.Settings;

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
                IConfiguration configuration = hostBuilderContext.Configuration;

                services.Configure<MqttSettings>(configuration.GetSection("MQTT"));

                services.AddHostedService<MqttSubscribeService>();

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