using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema; 


namespace TarhApi.Models
{
#pragma warning disable CS1591

    public class TarhDb : SRLCore.Model.DbEntity<TarhDb,User,Role,UserRole>
    {
        public override string GetConnectionString()
        {
            return Startup.GetConnection();
        }
        public TarhDb(DbContextOptions<TarhDb> options)
            : base(options)
        {

        }  

        public override void ModelCreator(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserRole>().HasOne(bc => bc.user).WithMany(b => b.user_roles).HasForeignKey(bc => bc.user_id);
            modelBuilder.Entity<UserRole>().HasOne(bc => bc.role).WithMany(c => c.user_roles).HasForeignKey(bc => bc.role_id);

            modelBuilder.Entity<BaseInfo>().HasMany(c => c.type_plans).WithOne(e => e.type).HasForeignKey(e => e.type_id);
            modelBuilder.Entity<BaseInfo>().HasMany(c => c.technology_plans).WithOne(e => e.technology).HasForeignKey(e => e.technology_id);
            modelBuilder.Entity<BaseInfo>().HasMany(c => c.lifecycle_plans).WithOne(e => e.lifecycle).HasForeignKey(e => e.lifecycle_id);
            modelBuilder.Entity<BaseInfo>().HasMany(c => c.knowledge_based_company_type_plans).WithOne(e => e.knowledge_based_company_type).HasForeignKey(e => e.knowledge_based_company_type_id);
            modelBuilder.Entity<BaseInfo>().HasMany(c => c.assessor_position_plans).WithOne(e => e.assessor_position).HasForeignKey(e => e.assessor_position_id);
            modelBuilder.Entity<BaseInfo>().HasMany(c => c.evaluation_unit_plans).WithOne(e => e.evaluation_unit).HasForeignKey(e => e.evaluation_unit_id);
            modelBuilder.Entity<BaseInfo>().HasMany(c => c.type_applicants).WithOne(e => e.type).HasForeignKey(e => e.type_id);
            modelBuilder.Entity<BaseInfo>().HasMany(c => c.related_category_applicants).WithOne(e => e.related_category).HasForeignKey(e => e.related_category_id);

            modelBuilder.Entity<Province>().HasMany(c => c.cities).WithOne(e => e.province).HasForeignKey(e => e.province_id);
            modelBuilder.Entity<City>().HasMany(c => c.applicants).WithOne(e => e.city).HasForeignKey(e => e.city_id);
            modelBuilder.Entity<Applicant>().HasMany(c => c.Plans).WithOne(e => e.applicant_company).HasForeignKey(e => e.applicant_id);
            modelBuilder.Entity<Plan>().HasMany(c => c.events).WithOne(e => e.plan).HasForeignKey(e => e.plan_id);
            modelBuilder.Entity<Level>().HasMany(c => c.events).WithOne(e => e.level).HasForeignKey(e => e.level_id);
            modelBuilder.Entity<LevelEvent>().HasMany(c => c.plan_events).WithOne(e => e.level_event).HasForeignKey(e => e.level_event_id);
            modelBuilder.Entity<Expert>().HasMany(c => c.Plans).WithOne(e => e.expert).HasForeignKey(e => e.expert_id);

            modelBuilder.Entity<Expert>().HasOne(a => a.user).WithOne(b => b.expert).HasForeignKey<Expert>(b => b.user_id);

            modelBuilder.Entity<Plan>().HasIndex(p => new { p.shenase }).IsUnique();
            modelBuilder.Entity<City>().HasIndex(p => new { p.province_id, p.title }).IsUnique();
            modelBuilder.Entity<User>().HasIndex(p => new { p.national_code }).IsUnique();
            modelBuilder.Entity<Role>().HasIndex(p => new { p.name }).IsUnique();
            modelBuilder.Entity<UserRole>().HasIndex(bc => new { bc.user_id, bc.role_id }).IsUnique();
            modelBuilder.Entity<BaseInfo>().HasIndex(bc => new { bc.kind, bc.is_default }).HasFilter($"{nameof(BaseInfo.is_default)} = 1").IsUnique();



            modelBuilder.ApplyConfiguration(new Applicant.ApplicantConfiguration());
            modelBuilder.ApplyConfiguration(new User.UserConfiguration());

            RestrinctDeleteBehavior(modelBuilder);
        }

        

        public DbSet<Plan> Plans { get; set; }
        public DbSet<PlanEvent> PlanEvents { get; set; }
        public DbSet<Level> Levels { get; set; }
        public DbSet<LevelEvent> LevelEvents { get; set; }
        public DbSet<BaseInfo> BaseInfos { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Province> Provinces { get; set; }
        public DbSet<Expert> Experts { get; set; }
        public DbSet<Applicant> Applicants { get; set; }
        public override DbSet<User> Users { get; set; }
        public override DbSet<Role> Roles { get; set; }
        public override  DbSet<UserRole> UserRoles { get; set; }
    }
  
    public class Plan : SRLCore.Model.CommonProperty
    {
        public long type_id { get; set; }
        public BaseInfo type { get; set; }
        public long technology_id { get; set; }
        public BaseInfo technology { get; set; }
        public long lifecycle_id { get; set; }
        public BaseInfo lifecycle { get; set; }
        public long knowledge_based_company_type_id { get; set; }
        public BaseInfo knowledge_based_company_type { get; set; }
        public long assessor_position_id { get; set; }
        public BaseInfo assessor_position { get; set; }
        public long evaluation_unit_id { get; set; }
        public BaseInfo evaluation_unit { get; set; }
        public ICollection<PlanEvent> events { get; set; }
        public long expert_id { get; set; }
        public Expert expert { get; set; }
        public long? applicant_id { get; set; }
        public Applicant applicant_company { get; set; }


        [Required]
        public string subject { get; set; }
        [Required]
        public string shenase { get; set; }
        public decimal estimate_riali { get; set; }
        [Required]
        public string referral_pdate { get; set; }

        [NotMapped]
        public string assessor_position_title => assessor_position?.title;
        [NotMapped]
        public string city_title => applicant_company?.city?.title;
        [NotMapped]
        public string evaluation_unit_title => evaluation_unit?.title;
        [NotMapped]
        public string expert_title => expert?.full_name;
        [NotMapped]
        public string knowledge_based_company_type_title => knowledge_based_company_type?.title;
        [NotMapped]
        public string lifecycle_title => lifecycle?.title;
        [NotMapped]
        public string technology_title => technology?.title;
        [NotMapped]
        public string type_title => type?.title;
        [NotMapped]
        public long? province_id => applicant_company?.city?.province_id;
        [NotMapped]
        public string province_title => applicant_company?.city?.province_title;
        [NotMapped]
        public string applicant_name => applicant_company?.name;  
        [NotMapped]
        public Level last_level { get; set; }

        [NotMapped]
        public PlanEvent last_event { get; set; }




    }
    public class PlanEvent : SRLCore.Model.CommonProperty
    {
        public long plan_id { get; set; }
        public Plan plan { get; set; }
        public long level_event_id { get; set; }
        public LevelEvent level_event { get; set; }

        [Required]
        public string description { get; set; }

        [Required]
        public string pdate { get; set; }
        public string doer { get; set; }
        public string assessor_description { get; set; }
        public string letter_number { get; set; }
        public string letter_pdate { get; set; }
        [NotMapped]
        public string level_event_title => level_event?.title;

    }
    public class Level : SRLCore.Model.CommonProperty
    {
        public ICollection<LevelEvent> events { get; set; }

        [Required]
        public string title { get; set; }
        [NotMapped]
        public string last_plan_share { get; set; }
        [NotMapped]
        public int last_plan_count { get; set; }

    }
    public class LevelEvent : SRLCore.Model.CommonProperty
    {
        public long level_id { get; set; }
        public Level level { get; set; }
        public ICollection<PlanEvent> plan_events { get; set; }


        [Required]
        public string title { get; set; }
        [NotMapped]
        public string level_title => level?.title;

    }
    public class BaseInfo : SRLCore.Model.CommonProperty
    {
        public ICollection<Plan> type_plans { get; set; }
        public ICollection<Plan> technology_plans { get; set; }
        public ICollection<Plan> lifecycle_plans { get; set; }
        public ICollection<Plan> knowledge_based_company_type_plans { get; set; }
        public ICollection<Plan> assessor_position_plans { get; set; }
        public ICollection<Plan> evaluation_unit_plans { get; set; }
        public ICollection<Applicant> type_applicants { get; set; }
        public ICollection<Applicant> related_category_applicants { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        [Required]
        public BaseKind kind { get; set; }
        [Required]
        public string title { get; set; } 
        public bool? is_default { get; set; }//add-migration "addIsDefaultToBaseinfo"

        [NotMapped]
        public string plan_share { get; set; }
        [NotMapped]
        public int plan_count { get; set; }
    }
    public class City : SRLCore.Model.CommonProperty
    {
        public long province_id { get; set; }
        public Province province { get; set; }
        public ICollection<Applicant> applicants { get; set; }


        [Required]
        public string title { get; set; }
        [NotMapped]
        public string province_title => province?.title;

    }
    public class Province : SRLCore.Model.IProvince
    {
        public ICollection<City> cities { get; set; }

        [Required]
        public override string title { get; set; } 
    }
    public class Expert : SRLCore.Model.CommonProperty
    {
        public long user_id { get; set; }
        public User user { get; set; }
        public ICollection<Plan> Plans { get; set; }

        [NotMapped]
        public string full_name => user?.full_name;

    }
    public class Applicant : SRLCore.Model.CommonProperty
    {

        public long city_id { get; set; }
        public City city { get; set; }
        public ICollection<Plan> Plans { get; set; }
        public long type_id { get; set; }
        public BaseInfo type { get; set; }
        public long related_category_id { get; set; }
        public BaseInfo related_category { get; set; }

        [Required]
        public string name { get; set; } 
        public string ceo { get; set; } 
        public string address { get; set; }  
        public string phone { get; set; } 
        public string representative { get; set; }

        [NotMapped]
        public string type_title => type?.title;

        public class ApplicantConfiguration : IEntityTypeConfiguration<Applicant>
        {
            public void Configure(EntityTypeBuilder<Applicant> builder)
            {
                builder.Property(e => e.phone).HasMaxLength(11).IsFixedLength();
            }
        }

    }
    public class User : SRLCore.Model.IUser
    {

        public ICollection<UserRole> user_roles { get; set; } 
        public Expert expert { get; set; }

        [Required]
        public string national_code { get; set; }
        [Required]
        public override string first_name { get; set; }
        [Required]
        public override string last_name { get; set; }
        [Required]
        public override string mobile { get; set; }
        [Required]
        public override byte[] password_hash { get; set; }
        [NotMapped]
        public override string password { get; set; }
        [Required]
        public override byte[] password_salt { get; set; }
        [NotMapped, DisplayName("نام و نام خانوادگی")]
        public override string full_name { get => $"{first_name} {last_name}"; }
        [NotMapped]
        public override string username { get => national_code; set => national_code = value; }
        [NotMapped]
        public override bool? change_pass_next_login { get; set; } = true;
        [NotMapped]
        public override DateTime? last_login { get; set; }
        public override long creator_id { get; set; } 

        public class UserConfiguration : IEntityTypeConfiguration<User>
        {
            public void Configure(EntityTypeBuilder<User> builder)
            {
                builder.Property(e => e.mobile).HasMaxLength(11).IsFixedLength();
                builder.Property(e => e.national_code).HasMaxLength(10).IsFixedLength();
            }
        }
    }

    public partial class Role : SRLCore.Model.IRole
    {
        public ICollection<UserRole> user_roles { get; set; }
        public override long creator_id { get; set; }  
        [Required]
        public override string name { get; set; }
        [Required]
        public override string accesses { get; set; }
        //[Column(TypeName = "nvarchar(50)")]
        //public EntityStatus status { get; set; }

    }
    public class UserRole : SRLCore.Model.IUserRole
    {
        public override long creator_id { get; set; } 

        public override long user_id { get; set; }
        public User user { get; set; }
        public override long role_id { get; set; }
        public Role role { get; set; }

    }


#pragma warning restore CS1591
}
