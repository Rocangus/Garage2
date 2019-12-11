﻿// <auto-generated />
using System;
using Garage2.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Garage2.Migrations
{
    [DbContext(typeof(GarageContext))]
    partial class GarageContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Garage2.Models.ParkedVehicle", b =>
                {
                    b.Property<string>("RegistrationNumber")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Colour")
                        .HasColumnType("nvarchar(15)")
                        .HasMaxLength(15);

                    b.Property<string>("Manufacturer")
                        .HasColumnType("nvarchar(30)")
                        .HasMaxLength(30);

                    b.Property<string>("Model")
                        .HasColumnType("nvarchar(60)")
                        .HasMaxLength(60);

                    b.Property<int>("NumberOfWheels")
                        .HasColumnType("int");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.HasKey("RegistrationNumber");

                    b.ToTable("ParkedVehicles");
                });

            modelBuilder.Entity("Garage2.Models.ParkingContract", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("ParkingDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("VehicleRegistrationNumber")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("VehicleRegistrationNumber");

                    b.ToTable("Contracts");
                });

            modelBuilder.Entity("Garage2.Models.ParkingContract", b =>
                {
                    b.HasOne("Garage2.Models.ParkedVehicle", "Vehicle")
                        .WithMany()
                        .HasForeignKey("VehicleRegistrationNumber");
                });
#pragma warning restore 612, 618
        }
    }
}
