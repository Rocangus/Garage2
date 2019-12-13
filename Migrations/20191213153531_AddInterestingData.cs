using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Garage2.Migrations
{
    public partial class AddInterestingData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "ParkedVehicles",
                columns: new[] { "RegistrationNumber", "Colour", "Manufacturer", "Model", "NumberOfWheels", "Type" },
                values: new object[] { "PAY276", "Red", "Skoda", "Fabia Combi 1.2 TSI", 4, 0 });

            migrationBuilder.InsertData(
                table: "ParkedVehicles",
                columns: new[] { "RegistrationNumber", "Colour", "Manufacturer", "Model", "NumberOfWheels", "Type" },
                values: new object[] { "AAA123", "White", "MAN", "Buss", 6, 2 });

            migrationBuilder.InsertData(
                table: "ParkedVehicles",
                columns: new[] { "RegistrationNumber", "Colour", "Manufacturer", "Model", "NumberOfWheels", "Type" },
                values: new object[] { "HUJ63E", "Blue", "BMW", "S1000RR", 2, 1 });

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

        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DeleteData(
                table: "ParkedVehicles",
                keyColumn: "RegistrationNumber",
                keyValue: "AAA123");

            migrationBuilder.DeleteData(
                table: "ParkedVehicles",
                keyColumn: "RegistrationNumber",
                keyValue: "HUJ63E");

            migrationBuilder.DeleteData(
                table: "ParkedVehicles",
                keyColumn: "RegistrationNumber",
                keyValue: "PAY276");
        }
    }
}
