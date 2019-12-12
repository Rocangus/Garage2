using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Garage2.Migrations
{
    public partial class AddBaseData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "ParkedVehicles",
                columns: new[] { "RegistrationNumber", "Colour", "Manufacturer", "Model", "NumberOfWheels", "Type" },
                values: new object[] { "PAY276", "Red", "Skoda", "Fabia Combi 1.2 TSI", 4, 0 });

            migrationBuilder.InsertData(
                table: "Contracts",
                columns: new[] { "Id", "ParkingDate", "VehicleRegistrationNumber" },
                values: new object[] { 1, new DateTime(2019, 8, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "PAY276" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Contracts",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "ParkedVehicles",
                keyColumn: "RegistrationNumber",
                keyValue: "PAY276");
        }
    }
}
