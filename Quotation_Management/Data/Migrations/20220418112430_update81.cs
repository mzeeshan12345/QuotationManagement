using Microsoft.EntityFrameworkCore.Migrations;

namespace Quotation_Management.Data.Migrations
{
    public partial class update81 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SubClients_Clients_ClientsClientId",
                table: "SubClients");

            migrationBuilder.DropIndex(
                name: "IX_SubClients_ClientsClientId",
                table: "SubClients");

            migrationBuilder.DropColumn(
                name: "ClientsClientId",
                table: "SubClients");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "ClientsClientId",
                table: "SubClients",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SubClients_ClientsClientId",
                table: "SubClients",
                column: "ClientsClientId");

            migrationBuilder.AddForeignKey(
                name: "FK_SubClients_Clients_ClientsClientId",
                table: "SubClients",
                column: "ClientsClientId",
                principalTable: "Clients",
                principalColumn: "ClientId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
