using Microsoft.EntityFrameworkCore.Migrations;

namespace Quotation_Management.Data.Migrations
{
    public partial class update74 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Phone",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "Whatsapp",
                table: "Clients");

            migrationBuilder.AddColumn<string>(
                name: "Address1",
                table: "Clients",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Address2",
                table: "Clients",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Address3",
                table: "Clients",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "Clients",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "RegionId",
                table: "Clients",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "TRN",
                table: "Clients",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address1",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "Address2",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "Address3",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "Country",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "RegionId",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "TRN",
                table: "Clients");

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "Clients",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Whatsapp",
                table: "Clients",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
