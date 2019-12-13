using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Garage2.Migrations
{
    public partial class ChangedVehicle3Date : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Contracts",
                keyColumn: "Id",
                keyValue: 3,
                column: "ParkingDate",
                value: new DateTime(2019, 12, 3, 14, 2, 33, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Contracts",
                keyColumn: "Id",
                keyValue: 3,
                column: "ParkingDate",
                value: new DateTime(2019, 12, 13, 14, 2, 33, 0, DateTimeKind.Unspecified));
        }
    }
}
