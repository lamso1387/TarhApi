using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TarhApi.Migrations
{
    public partial class FileEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FileEntities",
                columns: table => new
                {
                    id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    creator_id = table.Column<long>(nullable: false),
                    modifier_id = table.Column<long>(nullable: true),
                    create_date = table.Column<DateTime>(nullable: false),
                    modify_date = table.Column<DateTime>(nullable: true),
                    status = table.Column<string>(nullable: false),
                    evidence_id = table.Column<long>(nullable: false),
                    file_name = table.Column<string>(nullable: true),
                    folder_path = table.Column<string>(nullable: true),
                    file_guid = table.Column<string>(nullable: true),
                    file_extention = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileEntities", x => x.id);
                    table.ForeignKey(
                        name: "FK_FileEntities_Evidences_evidence_id",
                        column: x => x.evidence_id,
                        principalTable: "Evidences",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FileEntities_evidence_id",
                table: "FileEntities",
                column: "evidence_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FileEntities");
        }
    }
}
