using Microsoft.EntityFrameworkCore.Migrations;

namespace Quotation_Management.Data.Migrations
{
    public partial class update76 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsFlat",
                table: "Quotations",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsPerItem",
                table: "Quotations",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Flat",
                table: "QuotationHistory",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsFlat",
                table: "QuotationHistory",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsPerItem",
                table: "QuotationHistory",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Percentage",
                table: "QuotationHistory",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsFlat",
                table: "Quotations");

            migrationBuilder.DropColumn(
                name: "IsPerItem",
                table: "Quotations");

            migrationBuilder.DropColumn(
                name: "Flat",
                table: "QuotationHistory");

            migrationBuilder.DropColumn(
                name: "IsFlat",
                table: "QuotationHistory");

            migrationBuilder.DropColumn(
                name: "IsPerItem",
                table: "QuotationHistory");

            migrationBuilder.DropColumn(
                name: "Percentage",
                table: "QuotationHistory");
        }
    }
}
