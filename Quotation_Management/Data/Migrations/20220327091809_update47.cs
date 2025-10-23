using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Quotation_Management.Data.Migrations
{
    public partial class update47 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Company",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "Company",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "EditedAt",
                table: "Company",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "EditedBy",
                table: "Company",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Company");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "Company");

            migrationBuilder.DropColumn(
                name: "EditedAt",
                table: "Company");

            migrationBuilder.DropColumn(
                name: "EditedBy",
                table: "Company");
        }
    }
}
