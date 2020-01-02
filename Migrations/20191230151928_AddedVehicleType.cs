using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Garage2.Migrations
{
    public partial class AddedVehicleType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ParkedVehicles",
                keyColumn: "RegistrationNumber",
                keyValue: "PAY276");

            migrationBuilder.DeleteData(
                table: "Members",
                keyColumn: "MemberId",
                keyValue: 1);

            migrationBuilder.DropColumn(
                name: "Type",
                table: "ParkedVehicles");

            migrationBuilder.AddColumn<int>(
                name: "VehicleTypeId",
                table: "ParkedVehicles",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "VehicleTypes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VehicleTypes", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "VehicleTypes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Car" },
                    { 2, "Motorcycle" },
                    { 3, "Bus" },
                    { 4, "Truck" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ParkedVehicles_VehicleTypeId",
                table: "ParkedVehicles",
                column: "VehicleTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_ParkedVehicles_VehicleTypes_VehicleTypeId",
                table: "ParkedVehicles",
                column: "VehicleTypeId",
                principalTable: "VehicleTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ParkedVehicles_VehicleTypes_VehicleTypeId",
                table: "ParkedVehicles");

            migrationBuilder.DropTable(
                name: "VehicleTypes");

            migrationBuilder.DropIndex(
                name: "IX_ParkedVehicles_VehicleTypeId",
                table: "ParkedVehicles");

            migrationBuilder.DropColumn(
                name: "VehicleTypeId",
                table: "ParkedVehicles");

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "ParkedVehicles",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.InsertData(
                table: "Members",
                columns: new[] { "MemberId", "CityAddress", "Email", "FirstName", "LastName", "PhoneNumber" },
                values: new object[] { 1, "Lindevägen 60 Enskede Gård", "henning.oden@outlook.com", "Henning", "Odén", "0739753838" });

            migrationBuilder.InsertData(
                table: "ParkedVehicles",
                columns: new[] { "RegistrationNumber", "Colour", "Manufacturer", "MemberId", "Model", "NumberOfWheels", "ParkingDate", "Type" },
                values: new object[] { "PAY276", "Red", "Skoda", 1, "Fabia Combi 1.2 TSI", 4, new DateTime(2019, 12, 26, 19, 8, 27, 0, DateTimeKind.Unspecified), 0 });
        }
    }
}
