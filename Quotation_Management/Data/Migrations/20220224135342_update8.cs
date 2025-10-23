using Microsoft.EntityFrameworkCore.Migrations;

namespace Quotation_Management.Data.Migrations
{
    public partial class update8 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Currency",
                table: "Master_Detail_Table");

            migrationBuilder.DropColumn(
                name: "Vat5",
                table: "Master_Detail_Table");

            migrationBuilder.AddColumn<string>(
                name: "Quantity",
                table: "Master_Detail_Table",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Currency",
                table: "Company",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Vat",
                table: "Company",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "Master_Detail_Table");

            migrationBuilder.DropColumn(
                name: "Currency",
                table: "Company");

            migrationBuilder.DropColumn(
                name: "Vat",
                table: "Company");

            migrationBuilder.AddColumn<string>(
                name: "Currency",
                table: "Master_Detail_Table",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Vat5",
                table: "Master_Detail_Table",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
