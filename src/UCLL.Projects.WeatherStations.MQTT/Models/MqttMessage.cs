using MQTTnet.Protocol;

namespace UCLL.Projects.WeatherStations.MQTT.Models;

public class MqttMessage(string topic, MqttPayloadFormatIndicator payloadFormatIndicator, byte[] payload)
{
    public string Topic { get; set; } = topic;
    public MqttPayloadFormatIndicator PayloadFormatIndicator { get; set; } = payloadFormatIndicator;
    public byte[] Payload { get; set; } = payload;
}