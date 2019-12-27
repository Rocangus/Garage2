using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Garage2.Migrations
{
    public partial class MoveParkingDateIntoVehicle : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Contracts",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Contracts",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Contracts",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.AddColumn<DateTime>(
                name: "ParkingDate",
                table: "ParkedVehicles",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ParkingDate",
                table: "ParkedVehicles");

            migrationBuilder.InsertData(
                table: "Contracts",
                columns: new[] { "Id", "ParkingDate", "VehicleRegistrationNumber" },
                values: new object[] { 1, new DateTime(2019, 12, 13, 10, 35, 26, 0, DateTimeKind.Unspecified), "PAY276" });

            migrationBuilder.InsertData(
                table: "Contracts",
                columns: new[] { "Id", "ParkingDate", "VehicleRegistrationNumber" },
                values: new object[] { 2, new DateTime(2019, 12, 13, 8, 29, 47, 0, DateTimeKind.Unspecified), "AAA123" });

            migrationBuilder.InsertData(
                table: "Contracts",
                columns: new[] { "Id", "ParkingDate", "VehicleRegistrationNumber" },
                values: new object[] { 3, new DateTime(2019, 12, 3, 14, 2, 33, 0, DateTimeKind.Unspecified), "HUJ63E" });
        }
    }
}
