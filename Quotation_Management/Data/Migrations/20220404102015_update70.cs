using Microsoft.EntityFrameworkCore.Migrations;

namespace Quotation_Management.Data.Migrations
{
    public partial class update70 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdminApproval",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "FlowStatus",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ManagerApproval",
                table: "AspNetUsers");

            migrationBuilder.CreateTable(
                name: "AssignFlowStatus",
                columns: table => new
                {
                    AFSId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FlowStatus = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: true),
                    ManagerApproval = table.Column<bool>(nullable: false),
                    AdminApproval = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssignFlowStatus", x => x.AFSId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AssignFlowStatus");

            migrationBuilder.AddColumn<bool>(
                name: "AdminApproval",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "FlowStatus",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "ManagerApproval",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
