using Microsoft.EntityFrameworkCore.Migrations;

namespace TarhApi.Migrations
{
    public partial class shenase_required_unique : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Provinces_title",
                table: "Provinces");

            migrationBuilder.AlterColumn<string>(
                name: "title",
                table: "Provinces",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "shenase",
                table: "Plans",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Plans_shenase",
                table: "Plans",
                column: "shenase",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Plans_shenase",
                table: "Plans");

            migrationBuilder.AlterColumn<string>(
                name: "title",
                table: "Provinces",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "shenase",
                table: "Plans",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.CreateIndex(
                name: "IX_Provinces_title",
                table: "Provinces",
                column: "title",
                unique: true);
        }
    }
}
