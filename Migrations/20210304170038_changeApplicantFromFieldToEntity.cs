using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TarhApi.Migrations
{
    public partial class changeApplicantFromFieldToEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Plans_Cities_city_id",
                table: "Plans");

            migrationBuilder.DropIndex(
                name: "IX_Plans_city_id",
                table: "Plans");

            migrationBuilder.DropColumn(
                name: "city_id",
                table: "Plans");

            migrationBuilder.AlterColumn<string>(
                name: "representative",
                table: "Plans",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "related_category",
                table: "Plans",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "phone",
                table: "Plans",
                fixedLength: true,
                maxLength: 11,
                nullable: true,
                oldClrType: typeof(string),
                oldFixedLength: true,
                oldMaxLength: 11);

            migrationBuilder.AlterColumn<string>(
                name: "applicant",
                table: "Plans",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "address",
                table: "Plans",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddColumn<long>(
                name: "applicant_id",
                table: "Plans",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Applicants",
                columns: table => new
                {
                    creator_id = table.Column<long>(nullable: false),
                    modifier_id = table.Column<long>(nullable: true),
                    create_date = table.Column<DateTime>(nullable: false),
                    modify_date = table.Column<DateTime>(nullable: true),
                    id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    city_id = table.Column<long>(nullable: false),
                    address = table.Column<string>(nullable: false),
                    representative = table.Column<string>(nullable: false),
                    related_category = table.Column<string>(nullable: false),
                    phone = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Applicants", x => x.id);
                    table.ForeignKey(
                        name: "FK_Applicants_Cities_city_id",
                        column: x => x.city_id,
                        principalTable: "Cities",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Plans_applicant_id",
                table: "Plans",
                column: "applicant_id");

            migrationBuilder.CreateIndex(
                name: "IX_Applicants_city_id",
                table: "Applicants",
                column: "city_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Plans_Applicants_applicant_id",
                table: "Plans",
                column: "applicant_id",
                principalTable: "Applicants",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Plans_Applicants_applicant_id",
                table: "Plans");

            migrationBuilder.DropTable(
                name: "Applicants");

            migrationBuilder.DropIndex(
                name: "IX_Plans_applicant_id",
                table: "Plans");

            migrationBuilder.DropColumn(
                name: "applicant_id",
                table: "Plans");

            migrationBuilder.AlterColumn<string>(
                name: "representative",
                table: "Plans",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "related_category",
                table: "Plans",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "phone",
                table: "Plans",
                fixedLength: true,
                maxLength: 11,
                nullable: false,
                oldClrType: typeof(string),
                oldFixedLength: true,
                oldMaxLength: 11,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "applicant",
                table: "Plans",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "address",
                table: "Plans",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<long>(
                name: "city_id",
                table: "Plans",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_Plans_city_id",
                table: "Plans",
                column: "city_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Plans_Cities_city_id",
                table: "Plans",
                column: "city_id",
                principalTable: "Cities",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
