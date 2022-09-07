using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BarcodeAPI.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Barcode",
                columns: table => new
                {
                    id_Barcode = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateTimeBarCodeRequest = table.Column<DateTime>(type: "datetime2", nullable: false),
                    id_ModuleDirectory = table.Column<long>(type: "bigint", nullable: false),
                    id_ObjectDirectory = table.Column<long>(type: "bigint", nullable: false),
                    ObjectPrefix = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Identifier = table.Column<int>(type: "int", nullable: false),
                    StringIdentifier = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UniqueNumber = table.Column<int>(type: "int", nullable: false),
                    BarcodeString = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Barcode", x => x.id_Barcode);
                });

            migrationBuilder.CreateTable(
                name: "BarcodeHistory",
                columns: table => new
                {
                    id_BarcodeHistory = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ArchivingDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsAutoArchiving = table.Column<bool>(type: "bit", nullable: false),
                    id_ModuleDirectory = table.Column<long>(type: "bigint", nullable: false),
                    id_ObjectDirectory = table.Column<long>(type: "bigint", nullable: false),
                    Identifier = table.Column<int>(type: "int", nullable: false),
                    UniqueNumber = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BarcodeHistory", x => x.id_BarcodeHistory);
                });

            migrationBuilder.CreateTable(
                name: "ModuleDirectory",
                columns: table => new
                {
                    id_ModuleDirectory = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ModuleTitle = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModuleDirectory", x => x.id_ModuleDirectory);
                });

            migrationBuilder.CreateTable(
                name: "ObjectDirectory",
                columns: table => new
                {
                    id_ObjectDirectory = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ObjectTitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ObjectPrefix = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ObjectDirectory", x => x.id_ObjectDirectory);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Barcode");

            migrationBuilder.DropTable(
                name: "BarcodeHistory");

            migrationBuilder.DropTable(
                name: "ModuleDirectory");

            migrationBuilder.DropTable(
                name: "ObjectDirectory");
        }
    }
}
