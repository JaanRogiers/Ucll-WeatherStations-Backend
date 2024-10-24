using UCLL.Projects.WeatherStations.MQTT.Models;

namespace UCLL.Projects.WeatherStations.MQTT.Interfaces.Services;

public interface IMqttMessageQueue
{
    int Count { get; }
    bool IsEmpty { get; }
    ValueTask EnqueueAsync(MqttMessage message);
    Task<bool> WaitToDequeueAsync(CancellationToken cancellationToken);
    ValueTask<MqttMessage> DequeueAsync(CancellationToken cancellationToken);
}