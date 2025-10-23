using Microsoft.EntityFrameworkCore.Migrations;

namespace Quotation_Management.Data.Migrations
{
    public partial class update33 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DiscountStatus",
                table: "Quotations");

            migrationBuilder.AddColumn<string>(
                name: "Discount",
                table: "Quotations",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Discount",
                table: "Quotations");

            migrationBuilder.AddColumn<string>(
                name: "DiscountStatus",
                table: "Quotations",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
