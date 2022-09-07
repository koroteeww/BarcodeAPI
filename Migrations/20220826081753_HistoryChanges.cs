using Microsoft.EntityFrameworkCore.Migrations;

namespace BarcodeAPI.Migrations
{
    public partial class HistoryChanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ObjectPrefix",
                table: "BarcodeHistory",
                type: "nvarchar(2)",
                maxLength: 2,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StringIdentifier",
                table: "BarcodeHistory",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "StringIdentifier",
                table: "Barcode",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ObjectPrefix",
                table: "BarcodeHistory");

            migrationBuilder.DropColumn(
                name: "StringIdentifier",
                table: "BarcodeHistory");

            migrationBuilder.AlterColumn<string>(
                name: "StringIdentifier",
                table: "Barcode",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);
        }
    }
}
