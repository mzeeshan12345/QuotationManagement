using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Quotation_Management.Data.Migrations
{
    public partial class update18 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Master_Detail_Table_Master_Table_Master_TableMasterId",
                table: "Master_Detail_Table");

            migrationBuilder.DropIndex(
                name: "IX_Master_Detail_Table_Master_TableMasterId",
                table: "Master_Detail_Table");

            migrationBuilder.DropColumn(
                name: "CMEApproval",
                table: "Master_Detail_Table");

            migrationBuilder.DropColumn(
                name: "ClientId",
                table: "Master_Detail_Table");

            migrationBuilder.DropColumn(
                name: "Delivery",
                table: "Master_Detail_Table");

            migrationBuilder.DropColumn(
                name: "Master_TableMasterId",
                table: "Master_Detail_Table");

            migrationBuilder.DropColumn(
                name: "Payment",
                table: "Master_Detail_Table");

            migrationBuilder.DropColumn(
                name: "Remarks",
                table: "Master_Detail_Table");

            migrationBuilder.DropColumn(
                name: "Validity",
                table: "Master_Detail_Table");

            migrationBuilder.AddColumn<long>(
                name: "QuotationId",
                table: "Master_Detail_Table",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateTable(
                name: "Quotations",
                columns: table => new
                {
                    QuotationId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    EditedAt = table.Column<DateTime>(nullable: true),
                    DeletedAt = table.Column<DateTime>(nullable: true),
                    EditedBy = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    ClientId = table.Column<long>(nullable: true),
                    CompanyId = table.Column<long>(nullable: true),
                    TermsConditions = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Quotations", x => x.QuotationId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Master_Detail_Table_QuotationId",
                table: "Master_Detail_Table",
                column: "QuotationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Master_Detail_Table_Quotations_QuotationId",
                table: "Master_Detail_Table",
                column: "QuotationId",
                principalTable: "Quotations",
                principalColumn: "QuotationId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Master_Detail_Table_Quotations_QuotationId",
                table: "Master_Detail_Table");

            migrationBuilder.DropTable(
                name: "Quotations");

            migrationBuilder.DropIndex(
                name: "IX_Master_Detail_Table_QuotationId",
                table: "Master_Detail_Table");

            migrationBuilder.DropColumn(
                name: "QuotationId",
                table: "Master_Detail_Table");

            migrationBuilder.AddColumn<string>(
                name: "CMEApproval",
                table: "Master_Detail_Table",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ClientId",
                table: "Master_Detail_Table",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Delivery",
                table: "Master_Detail_Table",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "Master_TableMasterId",
                table: "Master_Detail_Table",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Payment",
                table: "Master_Detail_Table",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Remarks",
                table: "Master_Detail_Table",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Validity",
                table: "Master_Detail_Table",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Master_Detail_Table_Master_TableMasterId",
                table: "Master_Detail_Table",
                column: "Master_TableMasterId");

            migrationBuilder.AddForeignKey(
                name: "FK_Master_Detail_Table_Master_Table_Master_TableMasterId",
                table: "Master_Detail_Table",
                column: "Master_TableMasterId",
                principalTable: "Master_Table",
                principalColumn: "MasterId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
