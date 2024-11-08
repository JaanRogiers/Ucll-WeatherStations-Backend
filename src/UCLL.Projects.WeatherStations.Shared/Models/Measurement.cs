﻿namespace UCLL.Projects.WeatherStations.Shared.Models;

public class Measurement
{
    public DateTime Timestamp { get; set; }

    public string StationId { get; set; }

    public int SensorId { get; set; }

    public double Value { get; set; }
}