using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TarhApi.Migrations
{
    public partial class evidence : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Evidences",
                columns: table => new
                {
                    id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    creator_id = table.Column<long>(nullable: false),
                    modifier_id = table.Column<long>(nullable: true),
                    create_date = table.Column<DateTime>(nullable: false),
                    modify_date = table.Column<DateTime>(nullable: true),
                    status = table.Column<string>(nullable: false),
                    doc_type_id = table.Column<long>(nullable: false),
                    evidence_date = table.Column<DateTime>(nullable: false),
                    description = table.Column<string>(nullable: true),
                    tag = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Evidences", x => x.id);
                    table.ForeignKey(
                        name: "FK_Evidences_BaseInfos_doc_type_id",
                        column: x => x.doc_type_id,
                        principalTable: "BaseInfos",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Evidences_doc_type_id",
                table: "Evidences",
                column: "doc_type_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Evidences");
        }
    }
}
