using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Quotation_Management.Data.Migrations
{
    public partial class update7 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Master_Table",
                columns: table => new
                {
                    MasterId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    EditedAt = table.Column<DateTime>(nullable: true),
                    DeletedAt = table.Column<DateTime>(nullable: true),
                    EditedBy = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Item = table.Column<string>(nullable: true),
                    Price = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Master_Table", x => x.MasterId);
                });

            migrationBuilder.CreateTable(
                name: "Master_Detail_Table",
                columns: table => new
                {
                    MasterDetailId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    EditedAt = table.Column<DateTime>(nullable: true),
                    DeletedAt = table.Column<DateTime>(nullable: true),
                    EditedBy = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Payable = table.Column<string>(nullable: true),
                    Vat5 = table.Column<string>(nullable: true),
                    Total = table.Column<string>(nullable: true),
                    Currency = table.Column<string>(nullable: true),
                    Price = table.Column<string>(nullable: true),
                    MasterId = table.Column<long>(nullable: true),
                    Master_TableMasterId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Master_Detail_Table", x => x.MasterDetailId);
                    table.ForeignKey(
                        name: "FK_Master_Detail_Table_Master_Table_Master_TableMasterId",
                        column: x => x.Master_TableMasterId,
                        principalTable: "Master_Table",
                        principalColumn: "MasterId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Master_Detail_Table_Master_TableMasterId",
                table: "Master_Detail_Table",
                column: "Master_TableMasterId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Master_Detail_Table");

            migrationBuilder.DropTable(
                name: "Master_Table");
        }
    }
}
