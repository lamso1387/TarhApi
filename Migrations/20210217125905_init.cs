using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TarhApi.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BaseInfos",
                columns: table => new
                {
                    creator_id = table.Column<long>(nullable: false),
                    modifier_id = table.Column<long>(nullable: true),
                    create_date = table.Column<DateTime>(nullable: false),
                    modify_date = table.Column<DateTime>(nullable: true),
                    id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    kind = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    title = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BaseInfos", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Levels",
                columns: table => new
                {
                    creator_id = table.Column<long>(nullable: false),
                    modifier_id = table.Column<long>(nullable: true),
                    create_date = table.Column<DateTime>(nullable: false),
                    modify_date = table.Column<DateTime>(nullable: true),
                    id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    title = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Levels", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Provinces",
                columns: table => new
                {
                    creator_id = table.Column<long>(nullable: false),
                    modifier_id = table.Column<long>(nullable: true),
                    create_date = table.Column<DateTime>(nullable: false),
                    modify_date = table.Column<DateTime>(nullable: true),
                    id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    title = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Provinces", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    creator_id = table.Column<long>(nullable: false),
                    modifier_id = table.Column<long>(nullable: true),
                    create_date = table.Column<DateTime>(nullable: false),
                    modify_date = table.Column<DateTime>(nullable: true),
                    id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(nullable: false),
                    accesses = table.Column<string>(nullable: false),
                    status = table.Column<string>(type: "nvarchar(50)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    creator_id = table.Column<long>(nullable: false),
                    modifier_id = table.Column<long>(nullable: true),
                    create_date = table.Column<DateTime>(nullable: false),
                    modify_date = table.Column<DateTime>(nullable: true),
                    id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    national_code = table.Column<string>(fixedLength: true, maxLength: 10, nullable: false),
                    first_name = table.Column<string>(nullable: false),
                    last_name = table.Column<string>(nullable: false),
                    mobile = table.Column<string>(fixedLength: true, maxLength: 11, nullable: false),
                    password_hash = table.Column<byte[]>(nullable: false),
                    password_salt = table.Column<byte[]>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "LevelEvents",
                columns: table => new
                {
                    creator_id = table.Column<long>(nullable: false),
                    modifier_id = table.Column<long>(nullable: true),
                    create_date = table.Column<DateTime>(nullable: false),
                    modify_date = table.Column<DateTime>(nullable: true),
                    id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    level_id = table.Column<long>(nullable: false),
                    title = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LevelEvents", x => x.id);
                    table.ForeignKey(
                        name: "FK_LevelEvents_Levels_level_id",
                        column: x => x.level_id,
                        principalTable: "Levels",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Cities",
                columns: table => new
                {
                    creator_id = table.Column<long>(nullable: false),
                    modifier_id = table.Column<long>(nullable: true),
                    create_date = table.Column<DateTime>(nullable: false),
                    modify_date = table.Column<DateTime>(nullable: true),
                    id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    province_id = table.Column<long>(nullable: false),
                    title = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cities", x => x.id);
                    table.ForeignKey(
                        name: "FK_Cities_Provinces_province_id",
                        column: x => x.province_id,
                        principalTable: "Provinces",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Experts",
                columns: table => new
                {
                    creator_id = table.Column<long>(nullable: false),
                    modifier_id = table.Column<long>(nullable: true),
                    create_date = table.Column<DateTime>(nullable: false),
                    modify_date = table.Column<DateTime>(nullable: true),
                    id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    user_id = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Experts", x => x.id);
                    table.ForeignKey(
                        name: "FK_Experts_Users_user_id",
                        column: x => x.user_id,
                        principalTable: "Users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                columns: table => new
                {
                    creator_id = table.Column<long>(nullable: false),
                    modifier_id = table.Column<long>(nullable: true),
                    create_date = table.Column<DateTime>(nullable: false),
                    modify_date = table.Column<DateTime>(nullable: true),
                    id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    user_id = table.Column<long>(nullable: false),
                    role_id = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => x.id);
                    table.ForeignKey(
                        name: "FK_UserRoles_Roles_role_id",
                        column: x => x.role_id,
                        principalTable: "Roles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserRoles_Users_user_id",
                        column: x => x.user_id,
                        principalTable: "Users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Plans",
                columns: table => new
                {
                    creator_id = table.Column<long>(nullable: false),
                    modifier_id = table.Column<long>(nullable: true),
                    create_date = table.Column<DateTime>(nullable: false),
                    modify_date = table.Column<DateTime>(nullable: true),
                    id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    type_id = table.Column<long>(nullable: false),
                    technology_id = table.Column<long>(nullable: false),
                    lifecycle_id = table.Column<long>(nullable: false),
                    knowledge_based_company_type_id = table.Column<long>(nullable: false),
                    assessor_position_id = table.Column<long>(nullable: false),
                    evaluation_unit_id = table.Column<long>(nullable: false),
                    city_id = table.Column<long>(nullable: false),
                    expert_id = table.Column<long>(nullable: false),
                    subject = table.Column<string>(nullable: false),
                    estimate_riali = table.Column<decimal>(nullable: false),
                    referral_pdate = table.Column<string>(nullable: false),
                    applicant = table.Column<string>(nullable: false),
                    address = table.Column<string>(nullable: false),
                    representative = table.Column<string>(nullable: false),
                    related_category = table.Column<string>(nullable: false),
                    phone = table.Column<string>(fixedLength: true, maxLength: 11, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Plans", x => x.id);
                    table.ForeignKey(
                        name: "FK_Plans_BaseInfos_assessor_position_id",
                        column: x => x.assessor_position_id,
                        principalTable: "BaseInfos",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Plans_Cities_city_id",
                        column: x => x.city_id,
                        principalTable: "Cities",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Plans_BaseInfos_evaluation_unit_id",
                        column: x => x.evaluation_unit_id,
                        principalTable: "BaseInfos",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Plans_Experts_expert_id",
                        column: x => x.expert_id,
                        principalTable: "Experts",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Plans_BaseInfos_knowledge_based_company_type_id",
                        column: x => x.knowledge_based_company_type_id,
                        principalTable: "BaseInfos",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Plans_BaseInfos_lifecycle_id",
                        column: x => x.lifecycle_id,
                        principalTable: "BaseInfos",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Plans_BaseInfos_technology_id",
                        column: x => x.technology_id,
                        principalTable: "BaseInfos",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Plans_BaseInfos_type_id",
                        column: x => x.type_id,
                        principalTable: "BaseInfos",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PlanEvents",
                columns: table => new
                {
                    creator_id = table.Column<long>(nullable: false),
                    modifier_id = table.Column<long>(nullable: true),
                    create_date = table.Column<DateTime>(nullable: false),
                    modify_date = table.Column<DateTime>(nullable: true),
                    id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    plan_id = table.Column<long>(nullable: false),
                    level_event_id = table.Column<long>(nullable: false),
                    description = table.Column<string>(nullable: false),
                    pdate = table.Column<string>(nullable: false),
                    doer = table.Column<string>(nullable: false),
                    assessor_description = table.Column<string>(nullable: false),
                    letter_number = table.Column<string>(nullable: true),
                    letter_pdate = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlanEvents", x => x.id);
                    table.ForeignKey(
                        name: "FK_PlanEvents_LevelEvents_level_event_id",
                        column: x => x.level_event_id,
                        principalTable: "LevelEvents",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PlanEvents_Plans_plan_id",
                        column: x => x.plan_id,
                        principalTable: "Plans",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cities_province_id_title",
                table: "Cities",
                columns: new[] { "province_id", "title" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Experts_user_id",
                table: "Experts",
                column: "user_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LevelEvents_level_id",
                table: "LevelEvents",
                column: "level_id");

            migrationBuilder.CreateIndex(
                name: "IX_PlanEvents_level_event_id",
                table: "PlanEvents",
                column: "level_event_id");

            migrationBuilder.CreateIndex(
                name: "IX_PlanEvents_plan_id",
                table: "PlanEvents",
                column: "plan_id");

            migrationBuilder.CreateIndex(
                name: "IX_Plans_assessor_position_id",
                table: "Plans",
                column: "assessor_position_id");

            migrationBuilder.CreateIndex(
                name: "IX_Plans_city_id",
                table: "Plans",
                column: "city_id");

            migrationBuilder.CreateIndex(
                name: "IX_Plans_evaluation_unit_id",
                table: "Plans",
                column: "evaluation_unit_id");

            migrationBuilder.CreateIndex(
                name: "IX_Plans_expert_id",
                table: "Plans",
                column: "expert_id");

            migrationBuilder.CreateIndex(
                name: "IX_Plans_knowledge_based_company_type_id",
                table: "Plans",
                column: "knowledge_based_company_type_id");

            migrationBuilder.CreateIndex(
                name: "IX_Plans_lifecycle_id",
                table: "Plans",
                column: "lifecycle_id");

            migrationBuilder.CreateIndex(
                name: "IX_Plans_technology_id",
                table: "Plans",
                column: "technology_id");

            migrationBuilder.CreateIndex(
                name: "IX_Plans_type_id",
                table: "Plans",
                column: "type_id");

            migrationBuilder.CreateIndex(
                name: "IX_Provinces_title",
                table: "Provinces",
                column: "title",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Roles_name",
                table: "Roles",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_role_id",
                table: "UserRoles",
                column: "role_id");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_user_id_role_id",
                table: "UserRoles",
                columns: new[] { "user_id", "role_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_national_code",
                table: "Users",
                column: "national_code",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PlanEvents");

            migrationBuilder.DropTable(
                name: "UserRoles");

            migrationBuilder.DropTable(
                name: "LevelEvents");

            migrationBuilder.DropTable(
                name: "Plans");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Levels");

            migrationBuilder.DropTable(
                name: "BaseInfos");

            migrationBuilder.DropTable(
                name: "Cities");

            migrationBuilder.DropTable(
                name: "Experts");

            migrationBuilder.DropTable(
                name: "Provinces");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
