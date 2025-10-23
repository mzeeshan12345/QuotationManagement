using Microsoft.EntityFrameworkCore.Migrations;

namespace Quotation_Management.Data.Migrations
{
    public partial class update43 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmailStatus",
                table: "Quotations");

            migrationBuilder.DropColumn(
                name: "AccountNumber",
                table: "Company");

            migrationBuilder.DropColumn(
                name: "BankName",
                table: "Company");

            migrationBuilder.DropColumn(
                name: "IBAN",
                table: "Company");

            migrationBuilder.DropColumn(
                name: "SwiftCode",
                table: "Company");

            migrationBuilder.AddColumn<string>(
                name: "QuotationStatus",
                table: "Quotations",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "BankId",
                table: "Company",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateTable(
                name: "SubClients",
                columns: table => new
                {
                    SubClientId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClientName = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    Reference = table.Column<string>(nullable: true),
                    Phone = table.Column<string>(nullable: true),
                    Landline = table.Column<string>(nullable: true),
                    Whatsapp = table.Column<string>(nullable: true),
                    ContactPerson = table.Column<string>(nullable: true),
                    RefrenceType = table.Column<string>(nullable: true),
                    CompanyId = table.Column<long>(nullable: false),
                    ClientId = table.Column<long>(nullable: false),
                    ClientsClientId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubClients", x => x.SubClientId);
                    table.ForeignKey(
                        name: "FK_SubClients_Clients_ClientsClientId",
                        column: x => x.ClientsClientId,
                        principalTable: "Clients",
                        principalColumn: "ClientId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SubClients_ClientsClientId",
                table: "SubClients",
                column: "ClientsClientId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SubClients");

            migrationBuilder.DropColumn(
                name: "QuotationStatus",
                table: "Quotations");

            migrationBuilder.DropColumn(
                name: "BankId",
                table: "Company");

            migrationBuilder.AddColumn<bool>(
                name: "EmailStatus",
                table: "Quotations",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "AccountNumber",
                table: "Company",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BankName",
                table: "Company",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IBAN",
                table: "Company",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SwiftCode",
                table: "Company",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
