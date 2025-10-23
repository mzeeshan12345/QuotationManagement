using Microsoft.EntityFrameworkCore.Migrations;

namespace Quotation_Management.Data.Migrations
{
    public partial class update59 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Approved",
                table: "Quotations");

            migrationBuilder.AddColumn<bool>(
                name: "Admin",
                table: "Quotations",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Manager",
                table: "Quotations",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Admin",
                table: "Quotations");

            migrationBuilder.DropColumn(
                name: "Manager",
                table: "Quotations");

            migrationBuilder.AddColumn<bool>(
                name: "Approved",
                table: "Quotations",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
