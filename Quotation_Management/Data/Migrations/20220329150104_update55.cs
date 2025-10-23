using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Quotation_Management.Data.Migrations
{
    public partial class update55 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "QuotationHistory",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "QuotationHistory",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "EditedAt",
                table: "QuotationHistory",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "EditedBy",
                table: "QuotationHistory",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "QuotationHistory");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "QuotationHistory");

            migrationBuilder.DropColumn(
                name: "EditedAt",
                table: "QuotationHistory");

            migrationBuilder.DropColumn(
                name: "EditedBy",
                table: "QuotationHistory");
        }
    }
}
