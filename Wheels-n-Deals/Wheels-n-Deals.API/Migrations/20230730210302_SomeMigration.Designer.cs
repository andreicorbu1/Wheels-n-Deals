﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Wheels_n_Deals.API.DataLayer;

#nullable disable

namespace Wheels_n_Deals.API.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20230730210302_SomeMigration")]
    partial class SomeMigration
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.9")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Wheels_n_Deals.API.DataLayer.Models.Announcement", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("City")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<string>("County")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .HasColumnType("character varying(1000)");

                    b.Property<Guid>("OwnerId")
                        .HasColumnType("uuid");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<Guid>("VehicleId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("OwnerId");

                    b.HasIndex("VehicleId")
                        .IsUnique();

                    b.ToTable("Announcements");
                });

            modelBuilder.Entity("Wheels_n_Deals.API.DataLayer.Models.AnnouncementImage", b =>
                {
                    b.Property<Guid>("ImageId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("AnnouncementId")
                        .HasColumnType("uuid");

                    b.HasKey("ImageId", "AnnouncementId");

                    b.HasIndex("AnnouncementId");

                    b.ToTable("AnnouncementImages");
                });

            modelBuilder.Entity("Wheels_n_Deals.API.DataLayer.Models.Feature", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("CarBody")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)");

                    b.Property<long>("EngineSize")
                        .HasColumnType("bigint");

                    b.Property<string>("Fuel")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Gearbox")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<long>("HorsePower")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.ToTable("Features");
                });

            modelBuilder.Entity("Wheels_n_Deals.API.DataLayer.Models.Image", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("ImageUrl")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("ImageUrl")
                        .IsUnique();

                    b.ToTable("Images");
                });

            modelBuilder.Entity("Wheels_n_Deals.API.DataLayer.Models.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<string>("HashedPassword")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("character varying(150)");

                    b.Property<DateTime>("LastModified")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("character varying(10)");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Wheels_n_Deals.API.DataLayer.Models.Vehicle", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("FeatureId")
                        .HasColumnType("uuid");

                    b.Property<string>("Make")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<long>("Mileage")
                        .HasColumnType("bigint");

                    b.Property<string>("Model")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<Guid>("OwnerId")
                        .HasColumnType("uuid");

                    b.Property<float>("PriceInEuro")
                        .HasColumnType("real");

                    b.Property<string>("TechnicalState")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("VinNumber")
                        .IsRequired()
                        .HasMaxLength(17)
                        .HasColumnType("character varying(17)");

                    b.Property<long>("Year")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("FeatureId");

                    b.HasIndex("OwnerId");

                    b.HasIndex("VinNumber")
                        .IsUnique();

                    b.ToTable("Vehicles");
                });

            modelBuilder.Entity("Wheels_n_Deals.API.DataLayer.Models.Announcement", b =>
                {
                    b.HasOne("Wheels_n_Deals.API.DataLayer.Models.User", "Owner")
                        .WithMany("Announcements")
                        .HasForeignKey("OwnerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Wheels_n_Deals.API.DataLayer.Models.Vehicle", "Vehicle")
                        .WithOne("Announcement")
                        .HasForeignKey("Wheels_n_Deals.API.DataLayer.Models.Announcement", "VehicleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Owner");

                    b.Navigation("Vehicle");
                });

            modelBuilder.Entity("Wheels_n_Deals.API.DataLayer.Models.AnnouncementImage", b =>
                {
                    b.HasOne("Wheels_n_Deals.API.DataLayer.Models.Announcement", "Announcement")
                        .WithMany("Images")
                        .HasForeignKey("AnnouncementId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Wheels_n_Deals.API.DataLayer.Models.Image", "Image")
                        .WithMany("Announcements")
                        .HasForeignKey("ImageId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Announcement");

                    b.Navigation("Image");
                });

            modelBuilder.Entity("Wheels_n_Deals.API.DataLayer.Models.Vehicle", b =>
                {
                    b.HasOne("Wheels_n_Deals.API.DataLayer.Models.Feature", "Feature")
                        .WithMany("Vehicles")
                        .HasForeignKey("FeatureId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Wheels_n_Deals.API.DataLayer.Models.User", "Owner")
                        .WithMany("Vehicles")
                        .HasForeignKey("OwnerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Feature");

                    b.Navigation("Owner");
                });

            modelBuilder.Entity("Wheels_n_Deals.API.DataLayer.Models.Announcement", b =>
                {
                    b.Navigation("Images");
                });

            modelBuilder.Entity("Wheels_n_Deals.API.DataLayer.Models.Feature", b =>
                {
                    b.Navigation("Vehicles");
                });

            modelBuilder.Entity("Wheels_n_Deals.API.DataLayer.Models.Image", b =>
                {
                    b.Navigation("Announcements");
                });

            modelBuilder.Entity("Wheels_n_Deals.API.DataLayer.Models.User", b =>
                {
                    b.Navigation("Announcements");

                    b.Navigation("Vehicles");
                });

            modelBuilder.Entity("Wheels_n_Deals.API.DataLayer.Models.Vehicle", b =>
                {
                    b.Navigation("Announcement")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
