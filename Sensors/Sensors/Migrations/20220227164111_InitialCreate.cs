using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sensors.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MeasurementTypes",
                columns: table => new
                {
                    measurementType = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MeasurementTypes", x => x.measurementType);
                });

            migrationBuilder.CreateTable(
                name: "Sensors",
                columns: table => new
                {
                    sensorID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    name = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sensors", x => x.sensorID);
                });

            migrationBuilder.CreateTable(
                name: "Units",
                columns: table => new
                {
                    unit = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Units", x => x.unit);
                });

            migrationBuilder.CreateTable(
                name: "IotSensors",
                columns: table => new
                {
                    url = table.Column<string>(type: "TEXT", nullable: false),
                    sensorFKsensorID = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IotSensors", x => x.url);
                    table.ForeignKey(
                        name: "FK_IotSensors_Sensors_sensorFKsensorID",
                        column: x => x.sensorFKsensorID,
                        principalTable: "Sensors",
                        principalColumn: "sensorID");
                });

            migrationBuilder.CreateTable(
                name: "Measurements",
                columns: table => new
                {
                    measurementID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    timestamp = table.Column<DateTime>(type: "TEXT", nullable: false),
                    value = table.Column<double>(type: "REAL", nullable: false),
                    sensorFKsensorID = table.Column<int>(type: "INTEGER", nullable: false),
                    unitFKunit = table.Column<string>(type: "TEXT", nullable: false),
                    measurementTypeFKmeasurementType = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Measurements", x => x.measurementID);
                    table.ForeignKey(
                        name: "FK_Measurements_MeasurementTypes_measurementTypeFKmeasurementType",
                        column: x => x.measurementTypeFKmeasurementType,
                        principalTable: "MeasurementTypes",
                        principalColumn: "measurementType",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Measurements_Sensors_sensorFKsensorID",
                        column: x => x.sensorFKsensorID,
                        principalTable: "Sensors",
                        principalColumn: "sensorID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Measurements_Units_unitFKunit",
                        column: x => x.unitFKunit,
                        principalTable: "Units",
                        principalColumn: "unit",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_IotSensors_sensorFKsensorID",
                table: "IotSensors",
                column: "sensorFKsensorID");

            migrationBuilder.CreateIndex(
                name: "IX_Measurements_measurementTypeFKmeasurementType",
                table: "Measurements",
                column: "measurementTypeFKmeasurementType");

            migrationBuilder.CreateIndex(
                name: "IX_Measurements_sensorFKsensorID",
                table: "Measurements",
                column: "sensorFKsensorID");

            migrationBuilder.CreateIndex(
                name: "IX_Measurements_unitFKunit",
                table: "Measurements",
                column: "unitFKunit");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IotSensors");

            migrationBuilder.DropTable(
                name: "Measurements");

            migrationBuilder.DropTable(
                name: "MeasurementTypes");

            migrationBuilder.DropTable(
                name: "Sensors");

            migrationBuilder.DropTable(
                name: "Units");
        }
    }
}
