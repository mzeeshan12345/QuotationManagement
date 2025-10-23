using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Quotation_Management.Data.Migrations
{
    public partial class update54 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "QuotationHistory",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QuotationId = table.Column<long>(nullable: false),
                    ClientId = table.Column<long>(nullable: true),
                    ClientReference = table.Column<string>(nullable: true),
                    CompanyId = table.Column<long>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: true),
                    Status = table.Column<string>(nullable: true),
                    QuotationStatus = table.Column<string>(nullable: true),
                    VAT = table.Column<decimal>(nullable: false),
                    Discount = table.Column<decimal>(nullable: false),
                    TermsConditions = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuotationHistory", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "QuotationHistoryDetail",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    EditedAt = table.Column<DateTime>(nullable: true),
                    DeletedAt = table.Column<DateTime>(nullable: true),
                    EditedBy = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    MasterDetailId = table.Column<long>(nullable: false),
                    Payable = table.Column<decimal>(nullable: false),
                    Quantity = table.Column<string>(nullable: true),
                    Total = table.Column<decimal>(nullable: false),
                    Discount = table.Column<decimal>(nullable: false),
                    Price = table.Column<string>(nullable: true),
                    MainItemId = table.Column<long>(nullable: true),
                    MasterId = table.Column<long>(nullable: true),
                    QuotationId = table.Column<long>(nullable: false),
                    QuotationHistoryId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuotationHistoryDetail", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuotationHistoryDetail_QuotationHistory_QuotationHistoryId",
                        column: x => x.QuotationHistoryId,
                        principalTable: "QuotationHistory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_QuotationHistoryDetail_QuotationHistoryId",
                table: "QuotationHistoryDetail",
                column: "QuotationHistoryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "QuotationHistoryDetail");

            migrationBuilder.DropTable(
                name: "QuotationHistory");
        }
    }
}
