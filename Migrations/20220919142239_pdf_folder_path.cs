using Microsoft.EntityFrameworkCore.Migrations;

namespace TarhApi.Migrations
{
    public partial class pdf_folder_path : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "pdf_folder_path",
                table: "Evidences",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "pdf_folder_path",
                table: "Evidences");
        }
    }
}
