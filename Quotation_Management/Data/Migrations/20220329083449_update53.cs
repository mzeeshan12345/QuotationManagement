using Microsoft.EntityFrameworkCore.Migrations;

namespace Quotation_Management.Data.Migrations
{
    public partial class update53 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ItemRegions_MainItems_MainItemId",
                table: "ItemRegions");

            migrationBuilder.DropForeignKey(
                name: "FK_ItemRegions_Master_Table_Master_TableMasterId",
                table: "ItemRegions");

            migrationBuilder.DropIndex(
                name: "IX_ItemRegions_MainItemId",
                table: "ItemRegions");

            migrationBuilder.DropIndex(
                name: "IX_ItemRegions_Master_TableMasterId",
                table: "ItemRegions");

            migrationBuilder.DropColumn(
                name: "MainItemId",
                table: "ItemRegions");

            migrationBuilder.DropColumn(
                name: "Master_TableMasterId",
                table: "ItemRegions");

            migrationBuilder.AddColumn<long>(
                name: "Master_TableMasterId",
                table: "SubItemRegions",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SubItemRegions_Master_TableMasterId",
                table: "SubItemRegions",
                column: "Master_TableMasterId");

            migrationBuilder.CreateIndex(
                name: "IX_MainItemRegion_MainItemId",
                table: "MainItemRegion",
                column: "MainItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_MainItemRegion_MainItems_MainItemId",
                table: "MainItemRegion",
                column: "MainItemId",
                principalTable: "MainItems",
                principalColumn: "MainItemId",
                onDelete: ReferentialAction.Cascade);

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
                name: "FK_MainItemRegion_MainItems_MainItemId",
                table: "MainItemRegion");

            migrationBuilder.DropForeignKey(
                name: "FK_SubItemRegions_Master_Table_Master_TableMasterId",
                table: "SubItemRegions");

            migrationBuilder.DropIndex(
                name: "IX_SubItemRegions_Master_TableMasterId",
                table: "SubItemRegions");

            migrationBuilder.DropIndex(
                name: "IX_MainItemRegion_MainItemId",
                table: "MainItemRegion");

            migrationBuilder.DropColumn(
                name: "Master_TableMasterId",
                table: "SubItemRegions");

            migrationBuilder.AddColumn<long>(
                name: "MainItemId",
                table: "ItemRegions",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "Master_TableMasterId",
                table: "ItemRegions",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ItemRegions_MainItemId",
                table: "ItemRegions",
                column: "MainItemId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemRegions_Master_TableMasterId",
                table: "ItemRegions",
                column: "Master_TableMasterId");

            migrationBuilder.AddForeignKey(
                name: "FK_ItemRegions_MainItems_MainItemId",
                table: "ItemRegions",
                column: "MainItemId",
                principalTable: "MainItems",
                principalColumn: "MainItemId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ItemRegions_Master_Table_Master_TableMasterId",
                table: "ItemRegions",
                column: "Master_TableMasterId",
                principalTable: "Master_Table",
                principalColumn: "MasterId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
