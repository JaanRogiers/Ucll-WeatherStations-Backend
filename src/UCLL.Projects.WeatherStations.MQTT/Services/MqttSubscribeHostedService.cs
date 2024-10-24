using System.Text;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Protocol;
using UCLL.Projects.WeatherStations.MQTT.Interfaces.Services;
using UCLL.Projects.WeatherStations.MQTT.Settings;

namespace UCLL.Projects.WeatherStations.MQTT.Services;

public class MqttSubscribeHostedService(ILogger<MqttSubscribeHostedService> logger, IOptions<MqttSettings> mqttOptions, IMqttMessageQueue mqttMessageQueue) : IHostedService
{
    private readonly ILogger<MqttSubscribeHostedService> _logger = logger;
    private readonly MqttSettings _mqttSettings = mqttOptions.Value;
    private readonly IMqttMessageQueue _mqttMessageQueue = mqttMessageQueue;
    private readonly IMqttClient _mqttClient = new MqttFactory().CreateMqttClient();

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _mqttClient.ConnectedAsync += OnConnected;
        _mqttClient.DisconnectedAsync += OnDisconnected;
        _mqttClient.ApplicationMessageReceivedAsync += OnMessageReceived;

        await _mqttClient.ConnectAsync(
            options: new MqttClientOptionsBuilder()
                .WithTcpServer(
                    host: _mqttSettings.Host,
                    port: _mqttSettings.Port
                )
                .WithCredentials(_mqttSettings.Username, _mqttSettings.Password)
                .WithClientId($"weatherstations_to_db_{Guid.NewGuid()}") //idk if I like this naming
                //.WithReceiveMaximum(1000) //5.0.0
                .Build(),
            cancellationToken: cancellationToken
        );

        _logger.LogInformation("MQTT client starting...");
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await _mqttClient.DisconnectAsync(cancellationToken: cancellationToken);

        _logger.LogInformation("Disconnected from MQTT broker at {_mqttSettings.Host}:{_mqttSettings.Port}", _mqttSettings.Host, _mqttSettings.Port);
        _logger.LogInformation("MQTT client stopping...");
    }

    private async Task OnConnected(MqttClientConnectedEventArgs e)
    {
        _logger.LogInformation("Connected to MQTT broker at {_mqttSettings.Host}:{_mqttSettings.Port}", _mqttSettings.Host, _mqttSettings.Port);

        await _mqttClient.SubscribeAsync(
            topicFilter: new MqttTopicFilterBuilder()
                .WithTopic(_mqttSettings.SubscribeTopic)
                .WithQualityOfServiceLevel(MqttQualityOfServiceLevel.AtLeastOnce)
                .Build()
        );

        _logger.LogInformation("MQTT client subscribed topic '{_mqttSettings.SubscribeTopic}'", _mqttSettings.SubscribeTopic);
    }

    private Task OnDisconnected(MqttClientDisconnectedEventArgs e)
    {
        _logger.LogInformation("Disconnected from the MQTT broker.");

        return Task.CompletedTask;
    }

    private async Task OnMessageReceived(MqttApplicationMessageReceivedEventArgs e)
    {
        string topic = e.ApplicationMessage.Topic;
        MqttPayloadFormatIndicator payloadFormatIndicator = e.ApplicationMessage.PayloadFormatIndicator; //idk how this works
        byte[] payloadByteArray = e.ApplicationMessage.PayloadSegment.ToArray();

        /*
        string? message = payloadByteArray.Length > 0
            ? Encoding.UTF8.GetString(payloadByteArray)
            : null;

        _logger.LogInformation("Received message on topic: {topic}", topic);
        _logger.LogInformation("payload format indicator: {payloadFormatIndicator}", payloadFormatIndicator);
        _logger.LogInformation("Message: {message}", message);
        */

        await _mqttMessageQueue.EnqueueAsync(new(
            topic: topic,
            payloadFormatIndicator: payloadFormatIndicator,
            payload: payloadByteArray
        ));
    }
}