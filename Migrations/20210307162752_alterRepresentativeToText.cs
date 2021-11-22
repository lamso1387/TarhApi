using Microsoft.EntityFrameworkCore.Migrations;

namespace TarhApi.Migrations
{
    public partial class alterRepresentativeToText : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Applicants_Users_representative_id",
                table: "Applicants");

            migrationBuilder.DropIndex(
                name: "IX_Applicants_representative_id",
                table: "Applicants");

            migrationBuilder.DropColumn(
                name: "representative_id",
                table: "Applicants");

            migrationBuilder.AddColumn<string>(
                name: "representative",
                table: "Applicants",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "representative",
                table: "Applicants");

            migrationBuilder.AddColumn<long>(
                name: "representative_id",
                table: "Applicants",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_Applicants_representative_id",
                table: "Applicants",
                column: "representative_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Applicants_Users_representative_id",
                table: "Applicants",
                column: "representative_id",
                principalTable: "Users",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
