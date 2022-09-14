using Microsoft.EntityFrameworkCore.Migrations;

namespace TarhApi.Migrations
{
    public partial class pdf_guid : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "pdf_guid",
                table: "Evidences",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "pdf_guid",
                table: "Evidences");
        }
    }
}
