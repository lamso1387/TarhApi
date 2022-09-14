using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TarhApi.Migrations
{
    public partial class evidence_pdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "evidence_date",
                table: "Evidences");

            migrationBuilder.AddColumn<string>(
                name: "evidence_pdate",
                table: "Evidences",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "evidence_pdate",
                table: "Evidences");

            migrationBuilder.AddColumn<DateTime>(
                name: "evidence_date",
                table: "Evidences",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
