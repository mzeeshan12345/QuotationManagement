using Microsoft.EntityFrameworkCore.Migrations;

namespace Quotation_Management.Data.Migrations
{
    public partial class update51 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SubItemRegion_Master_Table_Master_TableMasterId",
                table: "SubItemRegion");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SubItemRegion",
                table: "SubItemRegion");

            migrationBuilder.RenameTable(
                name: "SubItemRegion",
                newName: "SubItemRegions");

            migrationBuilder.RenameIndex(
                name: "IX_SubItemRegion_Master_TableMasterId",
                table: "SubItemRegions",
                newName: "IX_SubItemRegions_Master_TableMasterId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SubItemRegions",
                table: "SubItemRegions",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SubItemRegions_Master_Table_Master_TableMasterId",
                table: "SubItemRegions",
                column: "Master_TableMasterId",
                principalTable: "Master_Table",
                principalColumn: "MasterId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SubItemRegions_Master_Table_Master_TableMasterId",
                table: "SubItemRegions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SubItemRegions",
                table: "SubItemRegions");

            migrationBuilder.RenameTable(
                name: "SubItemRegions",
                newName: "SubItemRegion");

            migrationBuilder.RenameIndex(
                name: "IX_SubItemRegions_Master_TableMasterId",
                table: "SubItemRegion",
                newName: "IX_SubItemRegion_Master_TableMasterId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SubItemRegion",
                table: "SubItemRegion",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SubItemRegion_Master_Table_Master_TableMasterId",
                table: "SubItemRegion",
                column: "Master_TableMasterId",
                principalTable: "Master_Table",
                principalColumn: "MasterId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
