using Microsoft.EntityFrameworkCore.Migrations;

namespace Quotation_Management.Data.Migrations
{
    public partial class update17 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CMEApproval",
                table: "Master_Detail_Table",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Delivery",
                table: "Master_Detail_Table",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Payment",
                table: "Master_Detail_Table",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Remarks",
                table: "Master_Detail_Table",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Validity",
                table: "Master_Detail_Table",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CMEApproval",
                table: "Master_Detail_Table");

            migrationBuilder.DropColumn(
                name: "Delivery",
                table: "Master_Detail_Table");

            migrationBuilder.DropColumn(
                name: "Payment",
                table: "Master_Detail_Table");

            migrationBuilder.DropColumn(
                name: "Remarks",
                table: "Master_Detail_Table");

            migrationBuilder.DropColumn(
                name: "Validity",
                table: "Master_Detail_Table");
        }
    }
}
