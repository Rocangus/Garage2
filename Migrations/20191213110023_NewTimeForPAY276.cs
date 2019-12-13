using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Garage2.Migrations
{
    public partial class NewTimeForPAY276 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Contracts",
                keyColumn: "Id",
                keyValue: 1,
                column: "ParkingDate",
                value: new DateTime(2019, 12, 13, 7, 35, 26, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Contracts",
                keyColumn: "Id",
                keyValue: 1,
                column: "ParkingDate",
                value: new DateTime(2019, 8, 30, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
