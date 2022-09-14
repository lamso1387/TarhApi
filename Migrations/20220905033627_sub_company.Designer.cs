﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TarhApi.Models;

namespace TarhApi.Migrations
{
    [DbContext(typeof(TarhDb))]
    [Migration("20220905033627_sub_company")]
    partial class sub_company
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.14-servicing-32113")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("TarhApi.Models.Applicant", b =>
                {
                    b.Property<long>("id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("address");

                    b.Property<string>("ceo");

                    b.Property<long>("city_id");

                    b.Property<DateTime>("create_date");

                    b.Property<long>("creator_id");

                    b.Property<long?>("modifier_id");

                    b.Property<DateTime?>("modify_date");

                    b.Property<string>("name")
                        .IsRequired();

                    b.Property<string>("phone")
                        .IsFixedLength(true)
                        .HasMaxLength(11);

                    b.Property<long>("related_category_id");

                    b.Property<string>("representative");

                    b.Property<string>("status")
                        .IsRequired();

                    b.Property<long>("type_id");

                    b.HasKey("id");

                    b.HasIndex("city_id");

                    b.HasIndex("related_category_id");

                    b.HasIndex("type_id");

                    b.ToTable("Applicants");
                });

            modelBuilder.Entity("TarhApi.Models.BaseInfo", b =>
                {
                    b.Property<long>("id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("create_date");

                    b.Property<long>("creator_id");

                    b.Property<bool?>("is_default");

                    b.Property<string>("kind")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)");

                    b.Property<long?>("modifier_id");

                    b.Property<DateTime?>("modify_date");

                    b.Property<string>("status")
                        .IsRequired();

                    b.Property<string>("title")
                        .IsRequired();

                    b.HasKey("id");

                    b.HasIndex("kind", "is_default")
                        .IsUnique()
                        .HasFilter("is_default = 1");

                    b.ToTable("BaseInfos");
                });

            modelBuilder.Entity("TarhApi.Models.City", b =>
                {
                    b.Property<long>("id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("create_date");

                    b.Property<long>("creator_id");

                    b.Property<long?>("modifier_id");

                    b.Property<DateTime?>("modify_date");

                    b.Property<long>("province_id");

                    b.Property<string>("status")
                        .IsRequired();

                    b.Property<string>("title")
                        .IsRequired();

                    b.HasKey("id");

                    b.HasIndex("province_id", "title")
                        .IsUnique();

                    b.ToTable("Cities");
                });

            modelBuilder.Entity("TarhApi.Models.Evidence", b =>
                {
                    b.Property<long>("id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("create_date");

                    b.Property<long>("creator_id");

                    b.Property<string>("description");

                    b.Property<long>("doc_type_id");

                    b.Property<string>("evidence_pdate")
                        .IsRequired();

                    b.Property<long?>("modifier_id");

                    b.Property<DateTime?>("modify_date");

                    b.Property<string>("status")
                        .IsRequired();

                    b.Property<long>("sub_company_id");

                    b.Property<string>("tag");

                    b.HasKey("id");

                    b.HasIndex("doc_type_id");

                    b.HasIndex("sub_company_id");

                    b.ToTable("Evidences");
                });

            modelBuilder.Entity("TarhApi.Models.Expert", b =>
                {
                    b.Property<long>("id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("create_date");

                    b.Property<long>("creator_id");

                    b.Property<long?>("modifier_id");

                    b.Property<DateTime?>("modify_date");

                    b.Property<string>("status")
                        .IsRequired();

                    b.Property<long>("user_id");

                    b.HasKey("id");

                    b.HasIndex("user_id")
                        .IsUnique();

                    b.ToTable("Experts");
                });

            modelBuilder.Entity("TarhApi.Models.Level", b =>
                {
                    b.Property<long>("id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("create_date");

                    b.Property<long>("creator_id");

                    b.Property<long?>("modifier_id");

                    b.Property<DateTime?>("modify_date");

                    b.Property<string>("status")
                        .IsRequired();

                    b.Property<string>("title")
                        .IsRequired();

                    b.HasKey("id");

                    b.ToTable("Levels");
                });

            modelBuilder.Entity("TarhApi.Models.LevelEvent", b =>
                {
                    b.Property<long>("id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("create_date");

                    b.Property<long>("creator_id");

                    b.Property<long>("level_id");

                    b.Property<long?>("modifier_id");

                    b.Property<DateTime?>("modify_date");

                    b.Property<string>("status")
                        .IsRequired();

                    b.Property<string>("title")
                        .IsRequired();

                    b.HasKey("id");

                    b.HasIndex("level_id");

                    b.ToTable("LevelEvents");
                });

            modelBuilder.Entity("TarhApi.Models.Plan", b =>
                {
                    b.Property<long>("id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long?>("applicant_id");

                    b.Property<long>("assessor_position_id");

                    b.Property<DateTime>("create_date");

                    b.Property<long>("creator_id");

                    b.Property<decimal>("estimate_riali");

                    b.Property<long>("evaluation_unit_id");

                    b.Property<long>("expert_id");

                    b.Property<long>("knowledge_based_company_type_id");

                    b.Property<long>("lifecycle_id");

                    b.Property<long?>("modifier_id");

                    b.Property<DateTime?>("modify_date");

                    b.Property<string>("referral_pdate")
                        .IsRequired();

                    b.Property<string>("shenase")
                        .IsRequired();

                    b.Property<string>("status")
                        .IsRequired();

                    b.Property<string>("subject")
                        .IsRequired();

                    b.Property<long>("technology_id");

                    b.Property<long>("type_id");

                    b.HasKey("id");

                    b.HasIndex("applicant_id");

                    b.HasIndex("assessor_position_id");

                    b.HasIndex("evaluation_unit_id");

                    b.HasIndex("expert_id");

                    b.HasIndex("knowledge_based_company_type_id");

                    b.HasIndex("lifecycle_id");

                    b.HasIndex("shenase")
                        .IsUnique();

                    b.HasIndex("technology_id");

                    b.HasIndex("type_id");

                    b.ToTable("Plans");
                });

            modelBuilder.Entity("TarhApi.Models.PlanEvent", b =>
                {
                    b.Property<long>("id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("assessor_description");

                    b.Property<DateTime>("create_date");

                    b.Property<long>("creator_id");

                    b.Property<string>("description")
                        .IsRequired();

                    b.Property<string>("doer");

                    b.Property<string>("letter_number");

                    b.Property<string>("letter_pdate");

                    b.Property<long>("level_event_id");

                    b.Property<long?>("modifier_id");

                    b.Property<DateTime?>("modify_date");

                    b.Property<string>("pdate")
                        .IsRequired();

                    b.Property<long>("plan_id");

                    b.Property<string>("status")
                        .IsRequired();

                    b.HasKey("id");

                    b.HasIndex("level_event_id");

                    b.HasIndex("plan_id");

                    b.ToTable("PlanEvents");
                });

            modelBuilder.Entity("TarhApi.Models.Province", b =>
                {
                    b.Property<long>("id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("create_date");

                    b.Property<long>("creator_id");

                    b.Property<long?>("modifier_id");

                    b.Property<DateTime?>("modify_date");

                    b.Property<string>("status")
                        .IsRequired();

                    b.Property<string>("title")
                        .IsRequired();

                    b.HasKey("id");

                    b.ToTable("Provinces");
                });

            modelBuilder.Entity("TarhApi.Models.Role", b =>
                {
                    b.Property<long>("id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("accesses")
                        .IsRequired();

                    b.Property<DateTime>("create_date");

                    b.Property<long>("creator_id");

                    b.Property<long?>("modifier_id");

                    b.Property<DateTime?>("modify_date");

                    b.Property<string>("name")
                        .IsRequired();

                    b.Property<string>("status")
                        .IsRequired();

                    b.HasKey("id");

                    b.HasIndex("name")
                        .IsUnique();

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("TarhApi.Models.User", b =>
                {
                    b.Property<long>("id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("create_date");

                    b.Property<long>("creator_id");

                    b.Property<string>("first_name")
                        .IsRequired();

                    b.Property<string>("last_name")
                        .IsRequired();

                    b.Property<string>("mobile")
                        .IsRequired()
                        .IsFixedLength(true)
                        .HasMaxLength(11);

                    b.Property<long?>("modifier_id");

                    b.Property<DateTime?>("modify_date");

                    b.Property<string>("national_code")
                        .IsRequired()
                        .IsFixedLength(true)
                        .HasMaxLength(10);

                    b.Property<byte[]>("password_hash")
                        .IsRequired();

                    b.Property<byte[]>("password_salt")
                        .IsRequired();

                    b.Property<string>("status")
                        .IsRequired();

                    b.HasKey("id");

                    b.HasIndex("national_code")
                        .IsUnique();

                    b.ToTable("Users");
                });

            modelBuilder.Entity("TarhApi.Models.UserRole", b =>
                {
                    b.Property<long>("id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("create_date");

                    b.Property<long>("creator_id");

                    b.Property<long?>("modifier_id");

                    b.Property<DateTime?>("modify_date");

                    b.Property<long>("role_id");

                    b.Property<string>("status")
                        .IsRequired();

                    b.Property<long>("user_id");

                    b.HasKey("id");

                    b.HasIndex("role_id");

                    b.HasIndex("user_id", "role_id")
                        .IsUnique();

                    b.ToTable("UserRoles");
                });

            modelBuilder.Entity("TarhApi.Models.Applicant", b =>
                {
                    b.HasOne("TarhApi.Models.City", "city")
                        .WithMany("applicants")
                        .HasForeignKey("city_id")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("TarhApi.Models.BaseInfo", "related_category")
                        .WithMany("related_category_applicants")
                        .HasForeignKey("related_category_id")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("TarhApi.Models.BaseInfo", "type")
                        .WithMany("type_applicants")
                        .HasForeignKey("type_id")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("TarhApi.Models.City", b =>
                {
                    b.HasOne("TarhApi.Models.Province", "province")
                        .WithMany("cities")
                        .HasForeignKey("province_id")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("TarhApi.Models.Evidence", b =>
                {
                    b.HasOne("TarhApi.Models.BaseInfo", "doc_type")
                        .WithMany("evidences")
                        .HasForeignKey("doc_type_id")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("TarhApi.Models.BaseInfo", "sub_company")
                        .WithMany("sub_companies")
                        .HasForeignKey("sub_company_id")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("TarhApi.Models.Expert", b =>
                {
                    b.HasOne("TarhApi.Models.User", "user")
                        .WithOne("expert")
                        .HasForeignKey("TarhApi.Models.Expert", "user_id")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("TarhApi.Models.LevelEvent", b =>
                {
                    b.HasOne("TarhApi.Models.Level", "level")
                        .WithMany("events")
                        .HasForeignKey("level_id")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("TarhApi.Models.Plan", b =>
                {
                    b.HasOne("TarhApi.Models.Applicant", "applicant_company")
                        .WithMany("Plans")
                        .HasForeignKey("applicant_id")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("TarhApi.Models.BaseInfo", "assessor_position")
                        .WithMany("assessor_position_plans")
                        .HasForeignKey("assessor_position_id")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("TarhApi.Models.BaseInfo", "evaluation_unit")
                        .WithMany("evaluation_unit_plans")
                        .HasForeignKey("evaluation_unit_id")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("TarhApi.Models.Expert", "expert")
                        .WithMany("Plans")
                        .HasForeignKey("expert_id")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("TarhApi.Models.BaseInfo", "knowledge_based_company_type")
                        .WithMany("knowledge_based_company_type_plans")
                        .HasForeignKey("knowledge_based_company_type_id")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("TarhApi.Models.BaseInfo", "lifecycle")
                        .WithMany("lifecycle_plans")
                        .HasForeignKey("lifecycle_id")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("TarhApi.Models.BaseInfo", "technology")
                        .WithMany("technology_plans")
                        .HasForeignKey("technology_id")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("TarhApi.Models.BaseInfo", "type")
                        .WithMany("type_plans")
                        .HasForeignKey("type_id")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("TarhApi.Models.PlanEvent", b =>
                {
                    b.HasOne("TarhApi.Models.LevelEvent", "level_event")
                        .WithMany("plan_events")
                        .HasForeignKey("level_event_id")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("TarhApi.Models.Plan", "plan")
                        .WithMany("events")
                        .HasForeignKey("plan_id")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("TarhApi.Models.UserRole", b =>
                {
                    b.HasOne("TarhApi.Models.Role", "role")
                        .WithMany("user_roles")
                        .HasForeignKey("role_id")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("TarhApi.Models.User", "user")
                        .WithMany("user_roles")
                        .HasForeignKey("user_id")
                        .OnDelete(DeleteBehavior.Restrict);
                });
#pragma warning restore 612, 618
        }
    }
}
