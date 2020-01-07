﻿// <auto-generated />
using System;
using Garage2.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Garage2.Migrations
{
    [DbContext(typeof(GarageContext))]
    [Migration("20200107141523_VehicleMember")]
    partial class VehicleMember
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Garage2.Models.Member", b =>
                {
                    b.Property<int>("MemberId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CityAddress")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("MemberId");

                    b.HasAlternateKey("Email");

                    b.ToTable("Members");
                });

            modelBuilder.Entity("Garage2.Models.ParkedVehicle", b =>
                {
                    b.Property<string>("RegistrationNumber")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Colour")
                        .IsRequired()
                        .HasColumnType("nvarchar(15)")
                        .HasMaxLength(15);

                    b.Property<bool>("IsParked")
                        .HasColumnType("bit");

                    b.Property<string>("Manufacturer")
                        .HasColumnType("nvarchar(30)")
                        .HasMaxLength(30);

                    b.Property<int>("MemberId")
                        .HasColumnType("int");

                    b.Property<string>("Model")
                        .HasColumnType("nvarchar(60)")
                        .HasMaxLength(60);

                    b.Property<int>("NumberOfWheels")
                        .HasColumnType("int");

                    b.Property<DateTime>("ParkingDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("VehicleTypeId")
                        .HasColumnType("int");

                    b.HasKey("RegistrationNumber");

                    b.HasIndex("MemberId");

                    b.HasIndex("VehicleTypeId");

                    b.ToTable("ParkedVehicles");
                });

            modelBuilder.Entity("Garage2.Models.VehicleType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("VehicleTypes");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Car"
                        },
                        new
                        {
                            Id = 2,
                            Name = "Motorcycle"
                        },
                        new
                        {
                            Id = 3,
                            Name = "Bus"
                        },
                        new
                        {
                            Id = 4,
                            Name = "Truck"
                        });
                });

            modelBuilder.Entity("Garage2.Models.ParkedVehicle", b =>
                {
                    b.HasOne("Garage2.Models.Member", "Member")
                        .WithMany("OwnedVehicles")
                        .HasForeignKey("MemberId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Garage2.Models.VehicleType", "Type")
                        .WithMany()
                        .HasForeignKey("VehicleTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
