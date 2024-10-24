using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using UCLL.Projects.WeatherStations.MQTT.Interfaces.Services;
using UCLL.Projects.WeatherStations.MQTT.Models;

namespace UCLL.Projects.WeatherStations.MQTT.Services;

public class MqttMessageProcessorBackgroundService(ILogger<MqttMessageProcessorBackgroundService> logger, IMqttMessageQueue mqttMessageMqttMessageQueue) : BackgroundService
{
    private readonly ILogger<MqttMessageProcessorBackgroundService> _logger = logger;
    private readonly IMqttMessageQueue _mqttMessageQueue = mqttMessageMqttMessageQueue;

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("MqttMessageProcessorBackgroundService is starting.");

        try
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                // Wait until there is data in the queue or cancellation is requested
                if (!await _mqttMessageQueue.WaitToDequeueAsync(cancellationToken)) continue;

                // Process messages as long as there are messages in the queue
                while (!cancellationToken.IsCancellationRequested && _mqttMessageQueue.Count > 0)
                {
                    MqttMessage message = await _mqttMessageQueue.DequeueAsync(cancellationToken);

                    await ProcessMqttMessageAsync(message, cancellationToken);
                }
            }
        }
        catch (TaskCanceledException)
        {
            _logger.LogInformation("Processing canceled.");
        }
        catch (Exception ex)
        {
            _logger.LogError("Error occurred while processing MQTT messages: {ex.Message}", ex.Message);
        }
        finally
        {
            _logger.LogInformation("MqttMessageProcessorBackgroundService is stopping.");
        }
    }

    private async Task ProcessMqttMessageAsync(MqttMessage mqttMessage, CancellationToken cancellationToken)
    {
        // 1) check topic (measurement or location)

        // 2) parse payload
        // 2.1) that that is json
        // 2.2) check for valid format
        // 2.3) parse/deserialize to object

        // 3) store in db

        // ??? 4) live data websockets ???
    }
}