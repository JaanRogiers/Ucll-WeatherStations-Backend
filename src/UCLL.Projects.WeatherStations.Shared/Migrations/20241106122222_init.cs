﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace UCLL.Projects.WeatherStations.WebApi.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Sensors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Unit = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sensors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Stations",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Latitude = table.Column<double>(type: "float", nullable: false),
                    Longitude = table.Column<double>(type: "float", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Measurements",
                columns: table => new
                {
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StationId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SensorId = table.Column<int>(type: "int", nullable: false),
                    Value = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Measurements", x => new { x.StationId, x.SensorId, x.Timestamp });
                    table.ForeignKey(
                        name: "FK_Measurements_Sensors_SensorId",
                        column: x => x.SensorId,
                        principalTable: "Sensors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Measurements_Stations_StationId",
                        column: x => x.StationId,
                        principalTable: "Stations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Station_Sensors",
                columns: table => new
                {
                    StationId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SensorId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Station_Sensors", x => new { x.StationId, x.SensorId });
                    table.ForeignKey(
                        name: "FK_Station_Sensors_Sensors_SensorId",
                        column: x => x.SensorId,
                        principalTable: "Sensors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Station_Sensors_Stations_StationId",
                        column: x => x.StationId,
                        principalTable: "Stations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Sensors",
                columns: new[] { "Id", "Type", "Unit" },
                values: new object[,]
                {
                    { 1, "Temperature", "Celsius" },
                    { 2, "Humidity", "%" }
                });

            migrationBuilder.InsertData(
                table: "Stations",
                columns: new[] { "Id", "Description", "Latitude", "Longitude", "Name" },
                values: new object[,]
                {
                    { "1", "Weather station in New York", 40.712800000000001, -74.006, "Station A" },
                    { "2", "Weather station in Los Angeles", 34.052199999999999, -118.2437, "Station B" }
                });

            migrationBuilder.InsertData(
                table: "Measurements",
                columns: new[] { "SensorId", "StationId", "Timestamp", "Value" },
                values: new object[,]
                {
                    { 1, "1", new DateTime(2024, 11, 4, 12, 22, 21, 797, DateTimeKind.Utc).AddTicks(7343), 23.5 },
                    { 1, "1", new DateTime(2024, 11, 5, 12, 22, 21, 797, DateTimeKind.Utc).AddTicks(7358), 22.5 },
                    { 2, "1", new DateTime(2024, 11, 4, 12, 22, 21, 797, DateTimeKind.Utc).AddTicks(7358), 60.0 },
                    { 2, "1", new DateTime(2024, 11, 5, 12, 22, 21, 797, DateTimeKind.Utc).AddTicks(7359), 58.0 },
                    { 1, "2", new DateTime(2024, 11, 4, 12, 22, 21, 797, DateTimeKind.Utc).AddTicks(7360), 19.199999999999999 },
                    { 1, "2", new DateTime(2024, 11, 6, 6, 22, 21, 797, DateTimeKind.Utc).AddTicks(7362), 18.199999999999999 },
                    { 2, "2", new DateTime(2024, 11, 4, 12, 22, 21, 797, DateTimeKind.Utc).AddTicks(7361), 61.0 },
                    { 2, "2", new DateTime(2024, 11, 6, 6, 22, 21, 797, DateTimeKind.Utc).AddTicks(7363), 57.0 }
                });

            migrationBuilder.InsertData(
                table: "Station_Sensors",
                columns: new[] { "SensorId", "StationId" },
                values: new object[,]
                {
                    { 1, "1" },
                    { 2, "1" },
                    { 1, "2" },
                    { 2, "2" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Measurements_SensorId",
                table: "Measurements",
                column: "SensorId");

            migrationBuilder.CreateIndex(
                name: "IX_Station_Sensors_SensorId",
                table: "Station_Sensors",
                column: "SensorId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Measurements");

            migrationBuilder.DropTable(
                name: "Station_Sensors");

            migrationBuilder.DropTable(
                name: "Sensors");

            migrationBuilder.DropTable(
                name: "Stations");
        }
    }
}
