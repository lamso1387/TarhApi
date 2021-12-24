using Microsoft.EntityFrameworkCore.Migrations;

namespace TarhApi.Migrations
{
    public partial class dbfromsrlcoreuserservicefromsrlcore : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "status",
                table: "Users",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "status",
                table: "UserRoles",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "status",
                table: "Roles",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "status",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "status",
                table: "UserRoles");

            migrationBuilder.AlterColumn<string>(
                name: "status",
                table: "Roles",
                type: "nvarchar(50)",
                nullable: false,
                oldClrType: typeof(string));
        }
    }
}
