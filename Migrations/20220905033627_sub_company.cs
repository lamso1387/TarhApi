using Microsoft.EntityFrameworkCore.Migrations;

namespace TarhApi.Migrations
{
    public partial class sub_company : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "sub_company_id",
                table: "Evidences",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_Evidences_sub_company_id",
                table: "Evidences",
                column: "sub_company_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Evidences_BaseInfos_sub_company_id",
                table: "Evidences",
                column: "sub_company_id",
                principalTable: "BaseInfos",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Evidences_BaseInfos_sub_company_id",
                table: "Evidences");

            migrationBuilder.DropIndex(
                name: "IX_Evidences_sub_company_id",
                table: "Evidences");

            migrationBuilder.DropColumn(
                name: "sub_company_id",
                table: "Evidences");
        }
    }
}
