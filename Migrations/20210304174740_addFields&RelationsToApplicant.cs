using Microsoft.EntityFrameworkCore.Migrations;

namespace TarhApi.Migrations
{
    public partial class addFieldsRelationsToApplicant : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "representative",
                table: "Applicants",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "related_category",
                table: "Applicants",
                newName: "ceo");

            migrationBuilder.AddColumn<long>(
                name: "related_category_id",
                table: "Applicants",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "representative_id",
                table: "Applicants",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "type_id",
                table: "Applicants",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_Applicants_related_category_id",
                table: "Applicants",
                column: "related_category_id");

            migrationBuilder.CreateIndex(
                name: "IX_Applicants_representative_id",
                table: "Applicants",
                column: "representative_id");

            migrationBuilder.CreateIndex(
                name: "IX_Applicants_type_id",
                table: "Applicants",
                column: "type_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Applicants_BaseInfos_related_category_id",
                table: "Applicants",
                column: "related_category_id",
                principalTable: "BaseInfos",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Applicants_Users_representative_id",
                table: "Applicants",
                column: "representative_id",
                principalTable: "Users",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Applicants_BaseInfos_type_id",
                table: "Applicants",
                column: "type_id",
                principalTable: "BaseInfos",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Applicants_BaseInfos_related_category_id",
                table: "Applicants");

            migrationBuilder.DropForeignKey(
                name: "FK_Applicants_Users_representative_id",
                table: "Applicants");

            migrationBuilder.DropForeignKey(
                name: "FK_Applicants_BaseInfos_type_id",
                table: "Applicants");

            migrationBuilder.DropIndex(
                name: "IX_Applicants_related_category_id",
                table: "Applicants");

            migrationBuilder.DropIndex(
                name: "IX_Applicants_representative_id",
                table: "Applicants");

            migrationBuilder.DropIndex(
                name: "IX_Applicants_type_id",
                table: "Applicants");

            migrationBuilder.DropColumn(
                name: "related_category_id",
                table: "Applicants");

            migrationBuilder.DropColumn(
                name: "representative_id",
                table: "Applicants");

            migrationBuilder.DropColumn(
                name: "type_id",
                table: "Applicants");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "Applicants",
                newName: "representative");

            migrationBuilder.RenameColumn(
                name: "ceo",
                table: "Applicants",
                newName: "related_category");
        }
    }
}
