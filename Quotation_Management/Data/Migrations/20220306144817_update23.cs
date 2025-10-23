using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Quotation_Management.Data.Migrations
{
    public partial class update23 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                    Payable = table.Column<decimal>(nullable: false),
                    Quantity = table.Column<string>(nullable: true),
                    Total = table.Column<decimal>(nullable: false),
                    Price = table.Column<string>(nullable: true),
                    MasterId = table.Column<long>(nullable: true),
                    QuotationId = table.Column<long>(nullable: false),
                    CompanyId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Master_Detail_Table", x => x.MasterDetailId);
                    table.ForeignKey(
                        name: "FK_Master_Detail_Table_Company_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Company",
                        principalColumn: "CompanyId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Master_Detail_Table_Quotations_QuotationId",
                        column: x => x.QuotationId,
                        principalTable: "Quotations",
                        principalColumn: "QuotationId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Master_Detail_Table_CompanyId",
                table: "Master_Detail_Table",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Master_Detail_Table_QuotationId",
                table: "Master_Detail_Table",
                column: "QuotationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Master_Detail_Table");
        }
    }
}
