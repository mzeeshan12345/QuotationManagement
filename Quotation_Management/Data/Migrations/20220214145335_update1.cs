using Microsoft.EntityFrameworkCore.Migrations;

namespace Quotation_Management.Data.Migrations
{
    public partial class update1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PrintedCertificate",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "PrintedTag",
                table: "AspNetUsers");

            migrationBuilder.CreateTable(
                name: "AppPage",
                columns: table => new
                {
                    AppPageId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PageName = table.Column<string>(nullable: true),
                    PageUrl = table.Column<string>(nullable: true),
                    PageType = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppPage", x => x.AppPageId);
                });

            migrationBuilder.CreateTable(
                name: "AppPagePermission",
                columns: table => new
                {
                    AppPagePermissionId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Create = table.Column<bool>(nullable: false),
                    Read = table.Column<bool>(nullable: false),
                    Update = table.Column<bool>(nullable: false),
                    Delete = table.Column<bool>(nullable: false),
                    RoleId = table.Column<string>(nullable: true),
                    AppPageId = table.Column<long>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppPagePermission", x => x.AppPagePermissionId);
                    table.ForeignKey(
                        name: "FK_AppPagePermission_AppPage_AppPageId",
                        column: x => x.AppPageId,
                        principalTable: "AppPage",
                        principalColumn: "AppPageId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppPagePermission_AppPageId",
                table: "AppPagePermission",
                column: "AppPageId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppPagePermission");

            migrationBuilder.DropTable(
                name: "AppPage");

            migrationBuilder.AddColumn<bool>(
                name: "PrintedCertificate",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "PrintedTag",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
