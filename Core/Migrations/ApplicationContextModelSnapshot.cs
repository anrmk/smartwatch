﻿// <auto-generated />
using System;
using Core.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Core.Migrations
{
    [DbContext(typeof(ApplicationContext))]
    partial class ApplicationContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("Core.Entities.DeviceEntity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Code");

                    b.Property<string>("CreatedBy")
                        .HasMaxLength(256);

                    b.Property<DateTime>("CreatedDate");

                    b.Property<string>("Idiom");

                    b.Property<string>("Imei")
                        .HasMaxLength(24);

                    b.Property<string>("Manufacturer");

                    b.Property<string>("Model");

                    b.Property<string>("Name");

                    b.Property<string>("Platform");

                    b.Property<long?>("ProfileCardEntity_Id");

                    b.Property<int>("Status");

                    b.Property<string>("UpdatedBy")
                        .HasMaxLength(256);

                    b.Property<DateTime>("UpdatedDate");

                    b.Property<string>("Version");

                    b.HasKey("Id");

                    b.HasIndex("ProfileCardEntity_Id");

                    b.ToTable("Devices");
                });

            modelBuilder.Entity("Core.Entities.DeviceLastLocationEntity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<double>("Accuracy");

                    b.Property<double>("Altitude");

                    b.Property<string>("Code");

                    b.Property<long?>("DeviceEntity_Id");

                    b.Property<double>("Direction");

                    b.Property<bool>("IsFromMockProvider");

                    b.Property<double>("Latitude");

                    b.Property<double>("Longitude");

                    b.Property<string>("Name");

                    b.Property<double>("Speed");

                    b.Property<DateTimeOffset>("Timestamp");

                    b.HasKey("Id");

                    b.HasIndex("DeviceEntity_Id");

                    b.ToTable("DeviceLastLocations");
                });

            modelBuilder.Entity("Core.Entities.DeviceLocationEntity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<double>("Accuracy");

                    b.Property<double>("Altitude");

                    b.Property<string>("Code");

                    b.Property<long?>("DeviceEntity_Id");

                    b.Property<double>("Direction");

                    b.Property<bool>("IsFromMockProvider");

                    b.Property<double>("Latitude");

                    b.Property<double>("Longitude");

                    b.Property<string>("Name");

                    b.Property<double>("Speed");

                    b.Property<DateTimeOffset>("Timestamp");

                    b.HasKey("Id");

                    b.HasIndex("DeviceEntity_Id");

                    b.ToTable("DeviceLocations");
                });

            modelBuilder.Entity("Core.Entities.ProfileCardEntity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Address")
                        .HasMaxLength(512);

                    b.Property<DateTime>("BirthDate");

                    b.Property<string>("Code");

                    b.Property<string>("CreatedBy")
                        .HasMaxLength(256);

                    b.Property<DateTime>("CreatedDate");

                    b.Property<long?>("DeviceEntity_Id");

                    b.Property<int>("Diastolic");

                    b.Property<string>("Middlename")
                        .HasMaxLength(64);

                    b.Property<string>("Name");

                    b.Property<string>("PhoneNumber")
                        .HasMaxLength(12);

                    b.Property<string>("Surname")
                        .IsRequired()
                        .HasMaxLength(64);

                    b.Property<int>("Systolic");

                    b.Property<string>("UpdatedBy")
                        .HasMaxLength(256);

                    b.Property<DateTime>("UpdatedDate");

                    b.HasKey("Id");

                    b.HasIndex("DeviceEntity_Id");

                    b.ToTable("ProfileCards");
                });

            modelBuilder.Entity("Core.Entities.ProfileCardMediaEntity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Code");

                    b.Property<string>("ContentType");

                    b.Property<string>("CreatedBy")
                        .HasMaxLength(256);

                    b.Property<DateTime>("CreatedDate");

                    b.Property<string>("Name");

                    b.Property<long?>("ProfileCardId")
                        .HasColumnName("ProfileCardEntity_Id");

                    b.Property<byte[]>("Source");

                    b.Property<byte[]>("Thumbnail");

                    b.Property<string>("UpdatedBy")
                        .HasMaxLength(256);

                    b.Property<DateTime>("UpdatedDate");

                    b.HasKey("Id");

                    b.HasIndex("ProfileCardId");

                    b.ToTable("ProfileCardMedias");
                });

            modelBuilder.Entity("Core.Entities.DeviceEntity", b =>
                {
                    b.HasOne("Core.Entities.ProfileCardEntity", "ProfileCard")
                        .WithMany()
                        .HasForeignKey("ProfileCardEntity_Id");
                });

            modelBuilder.Entity("Core.Entities.DeviceLastLocationEntity", b =>
                {
                    b.HasOne("Core.Entities.DeviceEntity", "Device")
                        .WithMany()
                        .HasForeignKey("DeviceEntity_Id");
                });

            modelBuilder.Entity("Core.Entities.DeviceLocationEntity", b =>
                {
                    b.HasOne("Core.Entities.DeviceEntity", "Device")
                        .WithMany("Locations")
                        .HasForeignKey("DeviceEntity_Id");
                });

            modelBuilder.Entity("Core.Entities.ProfileCardEntity", b =>
                {
                    b.HasOne("Core.Entities.DeviceEntity", "Device")
                        .WithMany()
                        .HasForeignKey("DeviceEntity_Id");
                });

            modelBuilder.Entity("Core.Entities.ProfileCardMediaEntity", b =>
                {
                    b.HasOne("Core.Entities.ProfileCardEntity", "ProfileCard")
                        .WithMany("Medias")
                        .HasForeignKey("ProfileCardId");
                });
#pragma warning restore 612, 618
        }
    }
}
