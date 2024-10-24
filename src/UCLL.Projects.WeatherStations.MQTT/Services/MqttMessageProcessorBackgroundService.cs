using Microsoft.Extensions.Hosting;
using UCLL.Projects.WeatherStations.MQTT.Models;

namespace UCLL.Projects.WeatherStations.MQTT.Services;

public class MqttMessageProcessorBackgroundService : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            // Background task logic
            await Task.Delay(1000, cancellationToken);
        }
    }

    private async Task ProcessMqttMessageAsync(MqttMessage mqttMessage)
    {
        // Process the message
    }
}