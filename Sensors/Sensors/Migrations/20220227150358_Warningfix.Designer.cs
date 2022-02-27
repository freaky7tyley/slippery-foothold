﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Sensors;

#nullable disable

namespace Sensors.Migrations
{
    [DbContext(typeof(DataModel))]
    [Migration("20220227150358_Warningfix")]
    partial class Warningfix
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "6.0.2");

            modelBuilder.Entity("Sensors.IotSensor", b =>
                {
                    b.Property<string>("url")
                        .HasColumnType("TEXT");

                    b.Property<int>("sensorFKsensorID")
                        .HasColumnType("INTEGER");

                    b.HasKey("url");

                    b.HasIndex("sensorFKsensorID");

                    b.ToTable("IotSensors");
                });

            modelBuilder.Entity("Sensors.Measurement", b =>
                {
                    b.Property<int>("measurementID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("measurementTypeFKmeasurementType")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("sensorFKsensorID")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("timestamp")
                        .HasColumnType("TEXT");

                    b.Property<string>("unitFKunit")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<double>("value")
                        .HasColumnType("REAL");

                    b.HasKey("measurementID");

                    b.HasIndex("measurementTypeFKmeasurementType");

                    b.HasIndex("sensorFKsensorID");

                    b.HasIndex("unitFKunit");

                    b.ToTable("Measurements");
                });

            modelBuilder.Entity("Sensors.MeasurementType", b =>
                {
                    b.Property<string>("measurementType")
                        .HasColumnType("TEXT");

                    b.HasKey("measurementType");

                    b.ToTable("MeasurementTypes");
                });

            modelBuilder.Entity("Sensors.Sensor", b =>
                {
                    b.Property<int>("sensorID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("name")
                        .HasColumnType("TEXT");

                    b.HasKey("sensorID");

                    b.ToTable("Sensors");
                });

            modelBuilder.Entity("Sensors.Unit", b =>
                {
                    b.Property<string>("unit")
                        .HasColumnType("TEXT");

                    b.HasKey("unit");

                    b.ToTable("Units");
                });

            modelBuilder.Entity("Sensors.IotSensor", b =>
                {
                    b.HasOne("Sensors.Sensor", "sensorFK")
                        .WithMany()
                        .HasForeignKey("sensorFKsensorID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("sensorFK");
                });

            modelBuilder.Entity("Sensors.Measurement", b =>
                {
                    b.HasOne("Sensors.MeasurementType", "measurementTypeFK")
                        .WithMany()
                        .HasForeignKey("measurementTypeFKmeasurementType")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Sensors.Sensor", "sensorFK")
                        .WithMany()
                        .HasForeignKey("sensorFKsensorID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Sensors.Unit", "unitFK")
                        .WithMany()
                        .HasForeignKey("unitFKunit")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("measurementTypeFK");

                    b.Navigation("sensorFK");

                    b.Navigation("unitFK");
                });
#pragma warning restore 612, 618
        }
    }
}
