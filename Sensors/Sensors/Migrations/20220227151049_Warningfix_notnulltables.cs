using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sensors.Migrations
{
    public partial class Warningfix_notnulltables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IotSensors_Sensors_sensorFKsensorID",
                table: "IotSensors");

            migrationBuilder.DropForeignKey(
                name: "FK_Measurements_Units_unitFKunit",
                table: "Measurements");

            migrationBuilder.AlterColumn<string>(
                name: "unitFKunit",
                table: "Measurements",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<int>(
                name: "sensorFKsensorID",
                table: "IotSensors",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddForeignKey(
                name: "FK_IotSensors_Sensors_sensorFKsensorID",
                table: "IotSensors",
                column: "sensorFKsensorID",
                principalTable: "Sensors",
                principalColumn: "sensorID");

            migrationBuilder.AddForeignKey(
                name: "FK_Measurements_Units_unitFKunit",
                table: "Measurements",
                column: "unitFKunit",
                principalTable: "Units",
                principalColumn: "unit");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IotSensors_Sensors_sensorFKsensorID",
                table: "IotSensors");

            migrationBuilder.DropForeignKey(
                name: "FK_Measurements_Units_unitFKunit",
                table: "Measurements");

            migrationBuilder.AlterColumn<string>(
                name: "unitFKunit",
                table: "Measurements",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "sensorFKsensorID",
                table: "IotSensors",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_IotSensors_Sensors_sensorFKsensorID",
                table: "IotSensors",
                column: "sensorFKsensorID",
                principalTable: "Sensors",
                principalColumn: "sensorID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Measurements_Units_unitFKunit",
                table: "Measurements",
                column: "unitFKunit",
                principalTable: "Units",
                principalColumn: "unit",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
