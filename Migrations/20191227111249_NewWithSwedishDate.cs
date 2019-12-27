using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Garage2.Migrations
{
    public partial class NewWithSwedishDate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Members",
                columns: table => new
                {
                    MemberId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    CityAddress = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Members", x => x.MemberId);
                });

            migrationBuilder.CreateTable(
                name: "ParkedVehicles",
                columns: table => new
                {
                    RegistrationNumber = table.Column<string>(nullable: false),
                    Type = table.Column<int>(nullable: false),
                    Colour = table.Column<string>(maxLength: 15, nullable: true),
                    Manufacturer = table.Column<string>(maxLength: 30, nullable: true),
                    Model = table.Column<string>(maxLength: 60, nullable: true),
                    MemberId = table.Column<int>(nullable: false),
                    NumberOfWheels = table.Column<int>(nullable: false),
                    ParkingDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParkedVehicles", x => x.RegistrationNumber);
                    table.ForeignKey(
                        name: "FK_ParkedVehicles_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Members",
                        principalColumn: "MemberId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Members",
                columns: new[] { "MemberId", "CityAddress", "Email", "FirstName", "LastName", "PhoneNumber" },
                values: new object[] { 1, "Lindevägen 60 Enskede Gård", "henning.oden@outlook.com", "Henning", "Odén", "0739753838" });

            migrationBuilder.InsertData(
                table: "ParkedVehicles",
                columns: new[] { "RegistrationNumber", "Colour", "Manufacturer", "MemberId", "Model", "NumberOfWheels", "ParkingDate", "Type" },
                values: new object[] { "PAY276", "Red", "Skoda", 1, "Fabia Combi 1.2 TSI", 4, new DateTime(2019, 12, 26, 19, 8, 27, 0, DateTimeKind.Unspecified), 0 });

            migrationBuilder.CreateIndex(
                name: "IX_ParkedVehicles_MemberId",
                table: "ParkedVehicles",
                column: "MemberId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ParkedVehicles");

            migrationBuilder.DropTable(
                name: "Members");
        }
    }
}
