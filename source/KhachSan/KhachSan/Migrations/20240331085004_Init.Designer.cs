﻿// <auto-generated />
using System;
using KhachSan.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace KhachSan.Migrations
{
    [DbContext(typeof(QLKSContext))]
    [Migration("20240331085004_Init")]
    partial class Init
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.25")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("KhachSan.Models.Booking", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTime>("createdDate")
                        .HasColumnType("datetime2");

                    b.Property<double>("toTalPrice")
                        .HasColumnType("float");

                    b.Property<int>("userId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("userId");

                    b.ToTable("Bookings");
                });

            modelBuilder.Entity("KhachSan.Models.BookingRoomDetail", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("bookingId")
                        .HasColumnType("int");

                    b.Property<DateTime>("checkIn")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("checkOut")
                        .HasColumnType("datetime2");

                    b.Property<int>("roomId")
                        .HasColumnType("int");

                    b.Property<double>("toTalPrice")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.HasIndex("bookingId");

                    b.HasIndex("roomId");

                    b.ToTable("BookingsRoomDetails");
                });

            modelBuilder.Entity("KhachSan.Models.BookingServiceDetail", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("bookingId")
                        .HasColumnType("int");

                    b.Property<int>("serviceId")
                        .HasColumnType("int");

                    b.Property<double>("toTalPrice")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.HasIndex("bookingId");

                    b.HasIndex("serviceId");

                    b.ToTable("BookingsServiceDetails");
                });

            modelBuilder.Entity("KhachSan.Models.Room", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("image")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("numPeople")
                        .HasColumnType("int");

                    b.Property<double>("price")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.ToTable("Rooms");
                });

            modelBuilder.Entity("KhachSan.Models.Service", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("image")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("price")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.ToTable("Services");
                });

            modelBuilder.Entity("KhachSan.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("role")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("username")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("KhachSan.Models.Booking", b =>
                {
                    b.HasOne("KhachSan.Models.User", null)
                        .WithMany("Bookings")
                        .HasForeignKey("userId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("KhachSan.Models.BookingRoomDetail", b =>
                {
                    b.HasOne("KhachSan.Models.Booking", "Booking")
                        .WithMany("BookingRoomDetails")
                        .HasForeignKey("bookingId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("KhachSan.Models.Room", "room")
                        .WithMany("BookingRoomDetails")
                        .HasForeignKey("roomId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Booking");

                    b.Navigation("room");
                });

            modelBuilder.Entity("KhachSan.Models.BookingServiceDetail", b =>
                {
                    b.HasOne("KhachSan.Models.Booking", "Booking")
                        .WithMany("BookingServiceDetails")
                        .HasForeignKey("bookingId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("KhachSan.Models.Service", "Service")
                        .WithMany("BookingServiceDetails")
                        .HasForeignKey("serviceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Booking");

                    b.Navigation("Service");
                });

            modelBuilder.Entity("KhachSan.Models.Booking", b =>
                {
                    b.Navigation("BookingRoomDetails");

                    b.Navigation("BookingServiceDetails");
                });

            modelBuilder.Entity("KhachSan.Models.Room", b =>
                {
                    b.Navigation("BookingRoomDetails");
                });

            modelBuilder.Entity("KhachSan.Models.Service", b =>
                {
                    b.Navigation("BookingServiceDetails");
                });

            modelBuilder.Entity("KhachSan.Models.User", b =>
                {
                    b.Navigation("Bookings");
                });
#pragma warning restore 612, 618
        }
    }
}
