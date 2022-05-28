using Microsoft.EntityFrameworkCore.Migrations;

namespace TarhApi.Migrations
{
    public partial class fromSrlCore : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "status",
                table: "Plans",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "status",
                table: "PlanEvents",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "status",
                table: "Levels",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "status",
                table: "LevelEvents",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "status",
                table: "Experts",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "status",
                table: "Cities",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "status",
                table: "BaseInfos",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "status",
                table: "Applicants",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "status",
                table: "Plans");

            migrationBuilder.DropColumn(
                name: "status",
                table: "PlanEvents");

            migrationBuilder.DropColumn(
                name: "status",
                table: "Levels");

            migrationBuilder.DropColumn(
                name: "status",
                table: "LevelEvents");

            migrationBuilder.DropColumn(
                name: "status",
                table: "Experts");

            migrationBuilder.DropColumn(
                name: "status",
                table: "Cities");

            migrationBuilder.DropColumn(
                name: "status",
                table: "BaseInfos");

            migrationBuilder.DropColumn(
                name: "status",
                table: "Applicants");
        }
    }
}
