using Microsoft.EntityFrameworkCore.Migrations;

namespace Quotation_Management.Data.Migrations
{
    public partial class update71 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "SubClients",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Quotations",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "QuotationHistoryDetail",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "QuotationHistory",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Master_Table",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Master_Detail_Table",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "ItemRegions",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Company",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Clients",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "ClientAssign",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Banks",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "SubClients");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Quotations");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "QuotationHistoryDetail");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "QuotationHistory");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Master_Table");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Master_Detail_Table");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "ItemRegions");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Company");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "ClientAssign");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Banks");
        }
    }
}
