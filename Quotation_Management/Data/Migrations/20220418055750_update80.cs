using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Quotation_Management.Data.Migrations
{
    public partial class update80 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "CountryMaster");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "CountryMaster");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "CountryMaster");

            migrationBuilder.DropColumn(
                name: "EditedAt",
                table: "CountryMaster");

            migrationBuilder.DropColumn(
                name: "EditedBy",
                table: "CountryMaster");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "CountryMaster",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "CountryMaster",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "CountryMaster",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "EditedAt",
                table: "CountryMaster",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EditedBy",
                table: "CountryMaster",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
