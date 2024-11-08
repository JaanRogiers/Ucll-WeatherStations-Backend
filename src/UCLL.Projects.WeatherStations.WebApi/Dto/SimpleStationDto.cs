﻿namespace UCLL.Projects.WeatherStations.WebApi.Dto;

public class SimpleStationDto
{
    public string Id { get; set; }
    public string Name { get; set; } = "";
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string Description { get; set; } = "";
}