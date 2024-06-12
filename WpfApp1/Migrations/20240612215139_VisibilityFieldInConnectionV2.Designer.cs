﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WpfApp1.Confuguration;

#nullable disable

namespace WpfApp1.Migrations
{
    [DbContext(typeof(ApplicationContext))]
    [Migration("20240612215139_VisibilityFieldInConnectionV2")]
    partial class VisibilityFieldInConnectionV2
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "6.0.29");

            modelBuilder.Entity("WpfApp1.Confuguration.Models.Connection", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<double>("DataTransferingSpeed")
                        .HasColumnType("REAL");

                    b.Property<int>("FirstDeviceId")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsVisible")
                        .HasColumnType("INTEGER");

                    b.Property<int>("SecondDeviceId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("FirstDeviceId");

                    b.HasIndex("SecondDeviceId");

                    b.ToTable("Connections");
                });

            modelBuilder.Entity("WpfApp1.Confuguration.Models.DataTransferLog", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("ConnectionId")
                        .HasColumnType("INTEGER");

                    b.Property<double>("DataSize")
                        .HasColumnType("REAL");

                    b.Property<DateTime?>("RecieveTime")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("SendingTime")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("ConnectionId");

                    b.ToTable("DataTransferLogs");
                });

            modelBuilder.Entity("WpfApp1.Confuguration.Models.Device", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("DeviceTypeId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Ip")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("DeviceTypeId");

                    b.ToTable("Devices");
                });

            modelBuilder.Entity("WpfApp1.Confuguration.Models.DeviceType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("DeviceTypes");
                });

            modelBuilder.Entity("WpfApp1.Confuguration.Models.State", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Type")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("States");
                });

            modelBuilder.Entity("WpfApp1.Confuguration.Models.StateLog", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("DeviceId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("StateId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("Time")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("DeviceId");

                    b.HasIndex("StateId");

                    b.ToTable("StateLogs");
                });

            modelBuilder.Entity("WpfApp1.Confuguration.Models.Connection", b =>
                {
                    b.HasOne("WpfApp1.Confuguration.Models.Device", "FirstDevice")
                        .WithMany("HostConnections")
                        .HasForeignKey("FirstDeviceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WpfApp1.Confuguration.Models.Device", "SecondDevice")
                        .WithMany("ClientConnections")
                        .HasForeignKey("SecondDeviceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("FirstDevice");

                    b.Navigation("SecondDevice");
                });

            modelBuilder.Entity("WpfApp1.Confuguration.Models.DataTransferLog", b =>
                {
                    b.HasOne("WpfApp1.Confuguration.Models.Connection", "Connection")
                        .WithMany("DataTransferLogs")
                        .HasForeignKey("ConnectionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Connection");
                });

            modelBuilder.Entity("WpfApp1.Confuguration.Models.Device", b =>
                {
                    b.HasOne("WpfApp1.Confuguration.Models.DeviceType", "DeviceType")
                        .WithMany("Devices")
                        .HasForeignKey("DeviceTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("DeviceType");
                });

            modelBuilder.Entity("WpfApp1.Confuguration.Models.StateLog", b =>
                {
                    b.HasOne("WpfApp1.Confuguration.Models.Device", "Device")
                        .WithMany("StateLogs")
                        .HasForeignKey("DeviceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WpfApp1.Confuguration.Models.State", "State")
                        .WithMany("StateLogs")
                        .HasForeignKey("StateId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Device");

                    b.Navigation("State");
                });

            modelBuilder.Entity("WpfApp1.Confuguration.Models.Connection", b =>
                {
                    b.Navigation("DataTransferLogs");
                });

            modelBuilder.Entity("WpfApp1.Confuguration.Models.Device", b =>
                {
                    b.Navigation("ClientConnections");

                    b.Navigation("HostConnections");

                    b.Navigation("StateLogs");
                });

            modelBuilder.Entity("WpfApp1.Confuguration.Models.DeviceType", b =>
                {
                    b.Navigation("Devices");
                });

            modelBuilder.Entity("WpfApp1.Confuguration.Models.State", b =>
                {
                    b.Navigation("StateLogs");
                });
#pragma warning restore 612, 618
        }
    }
}
