using Microsoft.EntityFrameworkCore.Migrations;

namespace Quotation_Management.Data.Migrations
{
    public partial class update50 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MainItemRegion",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MainItemId = table.Column<long>(nullable: false),
                    RegionId = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MainItemRegion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MainItemRegion_MainItems_MainItemId",
                        column: x => x.MainItemId,
                        principalTable: "MainItems",
                        principalColumn: "MainItemId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SubItemRegion",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MasterId = table.Column<long>(nullable: false),
                    RegionId = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Master_TableMasterId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubItemRegion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubItemRegion_Master_Table_Master_TableMasterId",
                        column: x => x.Master_TableMasterId,
                        principalTable: "Master_Table",
                        principalColumn: "MasterId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MainItemRegion_MainItemId",
                table: "MainItemRegion",
                column: "MainItemId");

            migrationBuilder.CreateIndex(
                name: "IX_SubItemRegion_Master_TableMasterId",
                table: "SubItemRegion",
                column: "Master_TableMasterId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MainItemRegion");

            migrationBuilder.DropTable(
                name: "SubItemRegion");
        }
    }
}
