using Microsoft.EntityFrameworkCore.Migrations;

namespace TarhApi.Migrations
{
    public partial class addIsDefaultToBaseInfo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "is_default",
                table: "BaseInfos",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BaseInfos_kind_is_default",
                table: "BaseInfos",
                columns: new[] { "kind", "is_default" },
                unique: true,
                filter: "is_default = 1");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_BaseInfos_kind_is_default",
                table: "BaseInfos");

            migrationBuilder.DropColumn(
                name: "is_default",
                table: "BaseInfos");
        }
    }
}
