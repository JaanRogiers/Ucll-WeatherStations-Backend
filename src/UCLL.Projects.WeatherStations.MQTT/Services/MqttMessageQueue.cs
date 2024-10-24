using System.Threading.Channels;
using UCLL.Projects.WeatherStations.MQTT.Interfaces.Services;
using UCLL.Projects.WeatherStations.MQTT.Models;

namespace UCLL.Projects.WeatherStations.MQTT.Services;

public class MqttMessageQueue : IMqttMessageQueue
{
    private readonly Channel<MqttMessage> _queue = Channel.CreateUnbounded<MqttMessage>(options: new()
    {
        SingleReader = false,
        SingleWriter = true,
        AllowSynchronousContinuations = false,
    });

    public int Count => _queue.Reader.Count;

    public bool IsEmpty => _queue.Reader.Count == 0;

    public async ValueTask EnqueueAsync(MqttMessage message) => await _queue.Writer.WriteAsync(message);

    public async Task<bool> WaitToDequeueAsync(CancellationToken cancellationToken) => await _queue.Reader.WaitToReadAsync(cancellationToken);

    public async ValueTask<MqttMessage> DequeueAsync(CancellationToken cancellationToken) => await _queue.Reader.ReadAsync(cancellationToken);
}