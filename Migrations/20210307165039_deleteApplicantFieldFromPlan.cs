using Microsoft.EntityFrameworkCore.Migrations;

namespace TarhApi.Migrations
{
    public partial class deleteApplicantFieldFromPlan : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "address",
                table: "Plans");

            migrationBuilder.DropColumn(
                name: "applicant",
                table: "Plans");

            migrationBuilder.DropColumn(
                name: "phone",
                table: "Plans");

            migrationBuilder.DropColumn(
                name: "related_category",
                table: "Plans");

            migrationBuilder.DropColumn(
                name: "representative",
                table: "Plans");

            migrationBuilder.AlterColumn<string>(
                name: "phone",
                table: "Applicants",
                fixedLength: true,
                maxLength: 11,
                nullable: false,
                oldClrType: typeof(string));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "address",
                table: "Plans",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "applicant",
                table: "Plans",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "phone",
                table: "Plans",
                fixedLength: true,
                maxLength: 11,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "related_category",
                table: "Plans",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "representative",
                table: "Plans",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "phone",
                table: "Applicants",
                nullable: false,
                oldClrType: typeof(string),
                oldFixedLength: true,
                oldMaxLength: 11);
        }
    }
}
