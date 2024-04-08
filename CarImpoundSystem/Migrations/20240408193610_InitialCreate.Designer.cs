﻿// <auto-generated />
using System;
using CarImpoundSystem.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace CarImpoundSystem.Migrations
{
    [DbContext(typeof(AppDBContext))]
    [Migration("20240408193610_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("CarImpoundSystem.Models.ImpoundmentRecord", b =>
                {
                    b.Property<string>("recordId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LicensePlate")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<int?>("UserId")
                        .HasColumnType("int");

                    b.Property<DateTime>("date")
                        .HasColumnType("datetime2");

                    b.Property<string>("location")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("payment")
                        .HasColumnType("float");

                    b.Property<string>("reason")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("recordId");

                    b.HasIndex("LicensePlate");

                    b.HasIndex("UserId");

                    b.ToTable("impoundmentRecords");
                });

            modelBuilder.Entity("CarImpoundSystem.Models.Payment", b =>
                {
                    b.Property<string>("paymentId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LicensePlate")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<double>("amount")
                        .HasColumnType("float");

                    b.Property<DateTime>("date")
                        .HasColumnType("datetime2");

                    b.HasKey("paymentId");

                    b.HasIndex("LicensePlate");

                    b.ToTable("payments");
                });

            modelBuilder.Entity("CarImpoundSystem.Models.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UserId"));

                    b.Property<string>("password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("role")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("username")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId");

                    b.ToTable("users");
                });

            modelBuilder.Entity("CarImpoundSystem.Models.Vehicle", b =>
                {
                    b.Property<string>("LicensePlate")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Color")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Make")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Model")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("VIN")
                        .HasColumnType("int");

                    b.HasKey("LicensePlate");

                    b.ToTable("vehicles");
                });

            modelBuilder.Entity("CarImpoundSystem.Models.ImpoundmentRecord", b =>
                {
                    b.HasOne("CarImpoundSystem.Models.Vehicle", "vehicle")
                        .WithMany("ImpoundmentRecords")
                        .HasForeignKey("LicensePlate")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CarImpoundSystem.Models.User", "User")
                        .WithMany("ImpoundmentRecords")
                        .HasForeignKey("UserId");

                    b.Navigation("User");

                    b.Navigation("vehicle");
                });

            modelBuilder.Entity("CarImpoundSystem.Models.Payment", b =>
                {
                    b.HasOne("CarImpoundSystem.Models.Vehicle", "vehicle")
                        .WithMany()
                        .HasForeignKey("LicensePlate")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("vehicle");
                });

            modelBuilder.Entity("CarImpoundSystem.Models.User", b =>
                {
                    b.Navigation("ImpoundmentRecords");
                });

            modelBuilder.Entity("CarImpoundSystem.Models.Vehicle", b =>
                {
                    b.Navigation("ImpoundmentRecords");
                });
#pragma warning restore 612, 618
        }
    }
}
