using Microsoft.EntityFrameworkCore.Migrations;

namespace Quotation_Management.Data.Migrations
{
    public partial class update73 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "CompanyId",
                table: "ItemRegions",
                nullable: false,
                defaultValue: 0L);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "ItemRegions");
        }
    }
}
