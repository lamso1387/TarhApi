using Microsoft.EntityFrameworkCore.Migrations;

namespace TarhApi.Migrations
{
    public partial class provinceFromSrlCore : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "status",
                table: "Provinces",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "status",
                table: "Provinces");
        }
    }
}
