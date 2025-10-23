using Microsoft.EntityFrameworkCore.Migrations;

namespace Quotation_Management.Data.Migrations
{
    public partial class update68 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Discount",
                table: "Quotations");

            migrationBuilder.DropColumn(
                name: "VAT",
                table: "Quotations");

            migrationBuilder.DropColumn(
                name: "Discount",
                table: "QuotationHistory");

            migrationBuilder.DropColumn(
                name: "VAT",
                table: "QuotationHistory");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Discount",
                table: "Quotations",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "VAT",
                table: "Quotations",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Discount",
                table: "QuotationHistory",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "VAT",
                table: "QuotationHistory",
                type: "float",
                nullable: true);
        }
    }
}
