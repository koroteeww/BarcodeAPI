﻿// <auto-generated />
using System;
using BarcodeAPI.DB;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace BarcodeAPI.Migrations
{
    [DbContext(typeof(BarcodeDbContext))]
    [Migration("20220824095015_StringLength")]
    partial class StringLength
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.17")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("BarcodeAPI.Models.Barcode", b =>
                {
                    b.Property<long>("id_Barcode")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id_Barcode")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("BarcodeString")
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<DateTime>("DateTimeBarCodeRequest")
                        .HasColumnType("datetime2");

                    b.Property<int>("Identifier")
                        .HasColumnType("int");

                    b.Property<string>("ObjectPrefix")
                        .HasMaxLength(2)
                        .HasColumnType("nvarchar(2)");

                    b.Property<string>("StringIdentifier")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UniqueNumber")
                        .HasColumnType("int");

                    b.Property<long>("id_ModuleDirectory")
                        .HasColumnType("bigint");

                    b.Property<long>("id_ObjectDirectory")
                        .HasColumnType("bigint");

                    b.HasKey("id_Barcode");

                    b.ToTable("Barcode");
                });

            modelBuilder.Entity("BarcodeAPI.Models.BarcodeHistory", b =>
                {
                    b.Property<long>("id_BarcodeHistory")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id_BarcodeHistory")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("ArchivingDateTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("BarcodeString")
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<int>("Identifier")
                        .HasColumnType("int");

                    b.Property<bool>("IsAutoArchiving")
                        .HasColumnType("bit");

                    b.Property<int>("UniqueNumber")
                        .HasColumnType("int");

                    b.Property<long>("id_ModuleDirectory")
                        .HasColumnType("bigint");

                    b.Property<long>("id_ObjectDirectory")
                        .HasColumnType("bigint");

                    b.HasKey("id_BarcodeHistory");

                    b.ToTable("BarcodeHistory");
                });

            modelBuilder.Entity("BarcodeAPI.Models.ModuleDirectory", b =>
                {
                    b.Property<long>("id_ModuleDirectory")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id_ModuleDirectory")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ModuleTitle")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.HasKey("id_ModuleDirectory");

                    b.ToTable("ModuleDirectory");
                });

            modelBuilder.Entity("BarcodeAPI.Models.ObjectDirectory", b =>
                {
                    b.Property<long>("id_ObjectDirectory")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id_ObjectDirectory")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("ObjectPrefix")
                        .HasMaxLength(2)
                        .HasColumnType("nvarchar(2)");

                    b.Property<string>("ObjectTitle")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.HasKey("id_ObjectDirectory");

                    b.ToTable("ObjectDirectory");
                });
#pragma warning restore 612, 618
        }
    }
}
