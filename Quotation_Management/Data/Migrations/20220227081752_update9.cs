using Microsoft.EntityFrameworkCore.Migrations;

namespace Quotation_Management.Data.Migrations
{
    public partial class update9 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "CompanyId",
                table: "Master_Detail_Table",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Master_Detail_Table_CompanyId",
                table: "Master_Detail_Table",
                column: "CompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Master_Detail_Table_Company_CompanyId",
                table: "Master_Detail_Table",
                column: "CompanyId",
                principalTable: "Company",
                principalColumn: "CompanyId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Master_Detail_Table_Company_CompanyId",
                table: "Master_Detail_Table");

            migrationBuilder.DropIndex(
                name: "IX_Master_Detail_Table_CompanyId",
                table: "Master_Detail_Table");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "Master_Detail_Table");
        }
    }
}
