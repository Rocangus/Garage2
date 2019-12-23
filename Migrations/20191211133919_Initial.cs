using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Garage2.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ParkedVehicles",
                columns: table => new
                {
                    RegistrationNumber = table.Column<string>(nullable: false),
                    Type = table.Column<int>(nullable: false),
                    Colour = table.Column<string>(maxLength: 15, nullable: true),
                    Manufacturer = table.Column<string>(maxLength: 30, nullable: true),
                    Model = table.Column<string>(maxLength: 60, nullable: true),
                    NumberOfWheels = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParkedVehicles", x => x.RegistrationNumber);
                });

            migrationBuilder.CreateTable(
                name: "Contracts",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VehicleRegistrationNumber = table.Column<string>(nullable: true),
                    ParkingDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contracts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Contracts_ParkedVehicles_VehicleRegistrationNumber",
                        column: x => x.VehicleRegistrationNumber,
                        principalTable: "ParkedVehicles",
                        principalColumn: "RegistrationNumber",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Contracts_VehicleRegistrationNumber",
                table: "Contracts",
                column: "VehicleRegistrationNumber");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Contracts");

            migrationBuilder.DropTable(
                name: "ParkedVehicles");
        }
    }
}
