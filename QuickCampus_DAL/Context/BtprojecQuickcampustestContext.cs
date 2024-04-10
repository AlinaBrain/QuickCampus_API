using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace QuickCampus_DAL.Context;

public partial class BtprojecQuickcampustestContext : DbContext
{
    public BtprojecQuickcampustestContext()
    {
    }

    public BtprojecQuickcampustestContext(DbContextOptions<BtprojecQuickcampustestContext> options)
        : base(options)
    {
    }

    public virtual DbSet<MstAppRole> MstAppRoles { get; set; }

    public virtual DbSet<MstApplicantStatus> MstApplicantStatuses { get; set; }

    public virtual DbSet<MstCity> MstCities { get; set; }

    public virtual DbSet<MstCityState> MstCityStates { get; set; }

    public virtual DbSet<MstCityStateCountry> MstCityStateCountries { get; set; }

    public virtual DbSet<MstGroupdl> MstGroupdls { get; set; }

    public virtual DbSet<MstMenuItem> MstMenuItems { get; set; }

    public virtual DbSet<MstMenuSubItem> MstMenuSubItems { get; set; }

    public virtual DbSet<MstPermission> MstPermissions { get; set; }

    public virtual DbSet<MstQualification> MstQualifications { get; set; }

    public virtual DbSet<MstQuestionType> MstQuestionTypes { get; set; }

    public virtual DbSet<MstSection> MstSections { get; set; }

    public virtual DbSet<MstSkill> MstSkills { get; set; }

    public virtual DbSet<TblApplicant> TblApplicants { get; set; }

    public virtual DbSet<TblApplicantSkill> TblApplicantSkills { get; set; }

    public virtual DbSet<TblClient> TblClients { get; set; }

    public virtual DbSet<TblCollege> TblColleges { get; set; }

    public virtual DbSet<TblMenuItemUserPermission> TblMenuItemUserPermissions { get; set; }

    public virtual DbSet<TblQuestion> TblQuestions { get; set; }

    public virtual DbSet<TblQuestionOption> TblQuestionOptions { get; set; }

    public virtual DbSet<TblRole> TblRoles { get; set; }

    public virtual DbSet<TblRolePermission> TblRolePermissions { get; set; }

    public virtual DbSet<TblTemplate> TblTemplates { get; set; }

    public virtual DbSet<TblUser> TblUsers { get; set; }

    public virtual DbSet<TblUserAppRole> TblUserAppRoles { get; set; }

    public virtual DbSet<TblUserRole> TblUserRoles { get; set; }

    public virtual DbSet<TblWalkIn> TblWalkIns { get; set; }

    public virtual DbSet<TblWalkInCollege> TblWalkInColleges { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=SYS69\\SQLEXPRESS;Database=btprojec_quickcampustest;TrustServerCertificate=true;Integrated Security=true;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseCollation("Latin1_General_CI_AI");

        modelBuilder.Entity<MstAppRole>(entity =>
        {
            entity.HasKey(e => e.AppRoleId).HasName("PK__MstAppRo__E66DD698EDF1A5D6");

            entity.ToTable("MstAppRole");

            entity.Property(e => e.AppRoleName)
                .HasMaxLength(100)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");
            entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");
        });

        modelBuilder.Entity<MstApplicantStatus>(entity =>
        {
            entity.HasKey(e => e.StatusId).HasName("PK_Status");

            entity.ToTable("MstApplicantStatus");

            entity.Property(e => e.StatusId).ValueGeneratedNever();
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
            entity.Property(e => e.StatusName)
                .HasMaxLength(200)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
        });

        modelBuilder.Entity<MstCity>(entity =>
        {
            entity.HasKey(e => e.CityId).HasName("PK__City__F2D21B760C7D81FB");

            entity.ToTable("MstCity");

            entity.Property(e => e.CityName)
                .HasMaxLength(150)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

            entity.HasOne(d => d.State).WithMany(p => p.MstCities)
                .HasForeignKey(d => d.StateId)
                .HasConstraintName("FK__City__StateId__7D439ABD");
        });

        modelBuilder.Entity<MstCityState>(entity =>
        {
            entity.HasKey(e => e.StateId).HasName("PK_State");

            entity.ToTable("MstCity_State");

            entity.Property(e => e.StateId).ValueGeneratedNever();
            entity.Property(e => e.StateName)
                .HasMaxLength(150)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");

            entity.HasOne(d => d.Country).WithMany(p => p.MstCityStates)
                .HasForeignKey(d => d.CountryId)
                .HasConstraintName("FK_State_Country");
        });

        modelBuilder.Entity<MstCityStateCountry>(entity =>
        {
            entity.HasKey(e => e.CountryId).HasName("PK_Country");

            entity.ToTable("MstCity_State_Country");

            entity.Property(e => e.CountryId).ValueGeneratedNever();
            entity.Property(e => e.CountryName)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
        });

        modelBuilder.Entity<MstGroupdl>(entity =>
        {
            entity.HasKey(e => e.GroupId).HasName("PK_Group");

            entity.ToTable("MstGroupdl");

            entity.Property(e => e.GroupId).ValueGeneratedNever();
            entity.Property(e => e.GroupName)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
        });

        modelBuilder.Entity<MstMenuItem>(entity =>
        {
            entity.HasKey(e => e.ItemId).HasName("PK__MstMenuI__727E838BD5B99911");

            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");
            entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");
            entity.Property(e => e.ItemDisplayName).HasMaxLength(1000);
            entity.Property(e => e.ItemName).HasMaxLength(100);
            entity.Property(e => e.ModifiedAt).HasColumnType("datetime");
        });

        modelBuilder.Entity<MstMenuSubItem>(entity =>
        {
            entity.HasKey(e => e.SubItemId).HasName("PK__MstMenuS__8A6B7585D74A99CF");

            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");
            entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");
            entity.Property(e => e.ModifiedAt).HasColumnType("datetime");
            entity.Property(e => e.SubItemDisplayName).HasMaxLength(1000);
            entity.Property(e => e.SubItemName).HasMaxLength(100);

            entity.HasOne(d => d.Item).WithMany(p => p.MstMenuSubItems)
                .HasForeignKey(d => d.ItemId)
                .HasConstraintName("FK__MstMenuSu__ItemI__531856C7");
        });

        modelBuilder.Entity<MstPermission>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Tbl_perm__3214EC07F1375A94");

            entity.Property(e => e.PermissionDisplay)
                .HasMaxLength(100)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.PermissionName)
                .HasMaxLength(100)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
        });

        modelBuilder.Entity<MstQualification>(entity =>
        {
            entity.HasKey(e => e.QualId).HasName("PK__MstQuali__B8C9022335534D56");

            entity.ToTable("MstQualification");

            entity.Property(e => e.CreateAt).HasColumnType("datetime");
            entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");
            entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");
            entity.Property(e => e.ModifiedAt).HasColumnType("datetime");
            entity.Property(e => e.QualName).HasMaxLength(200);
        });

        modelBuilder.Entity<MstQuestionType>(entity =>
        {
            entity.HasKey(e => e.QuestionTypeId).HasName("PK_QuestionType");

            entity.ToTable("MstQuestionType");

            entity.Property(e => e.QuestionTypeId).ValueGeneratedNever();
            entity.Property(e => e.QuestionType)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
        });

        modelBuilder.Entity<MstSection>(entity =>
        {
            entity.HasKey(e => e.SectionId).HasName("PK_ExaminationSection");

            entity.ToTable("MstSection");

            entity.Property(e => e.SectionId).ValueGeneratedNever();
            entity.Property(e => e.Section)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
        });

        modelBuilder.Entity<MstSkill>(entity =>
        {
            entity.HasKey(e => e.SkillId).HasName("PK__mst_Skil__DFA0918741539A92");

            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
            entity.Property(e => e.SkillName)
                .HasMaxLength(200)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TblApplicant>(entity =>
        {
            entity.HasKey(e => e.ApplicantId).HasName("PK_Applicant");

            entity.ToTable("tblApplicant");

            entity.Property(e => e.CollegeName)
                .HasMaxLength(255)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.Comment).UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.EmailAddress)
                .HasMaxLength(100)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.FirstName)
                .HasMaxLength(100)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.HigestQualification)
                .HasMaxLength(100)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.LastName)
                .HasMaxLength(100)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(25)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");

            entity.HasOne(d => d.Client).WithMany(p => p.TblApplicants)
                .HasForeignKey(d => d.ClientId)
                .HasConstraintName("FK__Applicant__Clien__2DB1C7EE");

            entity.HasOne(d => d.HighestQualificationNavigation).WithMany(p => p.TblApplicants)
                .HasForeignKey(d => d.HighestQualification)
                .HasConstraintName("FK__Applicant__Highe__5E8A0973");

            entity.HasOne(d => d.Status).WithMany(p => p.TblApplicants)
                .HasForeignKey(d => d.StatusId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Applicant_Status");
        });

        modelBuilder.Entity<TblApplicantSkill>(entity =>
        {
            entity.HasKey(e => e.SkillId).HasName("PK__Skill__DFA091873B15CBFE");

            entity.ToTable("tblApplicant_Skills");

            entity.Property(e => e.ApplicantId).HasColumnName("Applicant_Id");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

            entity.HasOne(d => d.Applicant).WithMany(p => p.TblApplicantSkills)
                .HasForeignKey(d => d.ApplicantId)
                .HasConstraintName("FK__Skill__Applicant__793DFFAF");

            entity.HasOne(d => d.ApplicantSkill).WithMany(p => p.TblApplicantSkills)
                .HasForeignKey(d => d.ApplicantSkillId)
                .HasConstraintName("FK__Applicant__Appli__2CBDA3B5");
        });

        modelBuilder.Entity<TblClient>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__tbl_Clie__3214EC0728D8A50C");

            entity.ToTable("tblClient");

            entity.Property(e => e.Address).UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.CompanyName).HasMaxLength(100);
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Email).UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.Latitude).HasColumnType("decimal(20, 2)");
            entity.Property(e => e.Longitude).HasColumnType("decimal(20, 2)");
            entity.Property(e => e.ModofiedDate).HasColumnType("datetime");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.Password)
                .HasMaxLength(100)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.Phone).UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.SubscriptionPlan).UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.UserName)
                .HasMaxLength(100)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
        });

        modelBuilder.Entity<TblCollege>(entity =>
        {
            entity.HasKey(e => e.CollegeId).HasName("PK_College");

            entity.ToTable("tblCollege");

            entity.Property(e => e.Address1)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.Address2)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.CollegeCode)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.CollegeName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.ContectEmail)
                .HasMaxLength(100)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.ContectPerson)
                .HasMaxLength(100)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.ContectPhone)
                .HasMaxLength(100)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Logo)
                .HasMaxLength(250)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

            entity.HasOne(d => d.City).WithMany(p => p.TblColleges)
                .HasForeignKey(d => d.CityId)
                .HasConstraintName("FK__College__CityId__7E37BEF6");
        });

        modelBuilder.Entity<TblMenuItemUserPermission>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__tblMenuI__3214EC07ECB77D04");

            entity.ToTable("tblMenuItemUserPermission");

            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");
            entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");
            entity.Property(e => e.ModifiedAt).HasColumnType("datetime");

            entity.HasOne(d => d.User).WithMany(p => p.TblMenuItemUserPermissions)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__tblMenuIt__UserI__57DD0BE4");
        });

        modelBuilder.Entity<TblQuestion>(entity =>
        {
            entity.HasKey(e => e.QuestionId).HasName("PK_Question");

            entity.ToTable("tblQuestion");

            entity.Property(e => e.Text).UseCollation("SQL_Latin1_General_CP1_CI_AS");

            entity.HasOne(d => d.Client).WithMany(p => p.TblQuestions)
                .HasForeignKey(d => d.ClientId)
                .HasConstraintName("FK__tblQuesti__Clien__2EA5EC27");

            entity.HasOne(d => d.Group).WithMany(p => p.TblQuestions)
                .HasForeignKey(d => d.GroupId)
                .HasConstraintName("FK_Question_Group");

            entity.HasOne(d => d.QuestionType).WithMany(p => p.TblQuestions)
                .HasForeignKey(d => d.QuestionTypeId)
                .HasConstraintName("FK_Question_QuestionType");

            entity.HasOne(d => d.Section).WithMany(p => p.TblQuestions)
                .HasForeignKey(d => d.SectionId)
                .HasConstraintName("FK_Question_ExaminationSection");
        });

        modelBuilder.Entity<TblQuestionOption>(entity =>
        {
            entity.HasKey(e => e.OptionId).HasName("PK_Answer");

            entity.ToTable("tblQuestion_Option");

            entity.Property(e => e.Imagepath).UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.OptionText).UseCollation("SQL_Latin1_General_CP1_CI_AS");

            entity.HasOne(d => d.Question).WithMany(p => p.TblQuestionOptions)
                .HasForeignKey(d => d.QuestionId)
                .HasConstraintName("FK_Answer_Question");
        });

        modelBuilder.Entity<TblRole>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__tbl_Role__3214EC07D22FB3FE");

            entity.ToTable("tblRole");

            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.ModifiedBy).HasColumnName("modifiedBy");
            entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
        });

        modelBuilder.Entity<TblRolePermission>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__tbl_Role__3214EC0723025571");

            entity.ToTable("tblRole_Permission");

            entity.HasOne(d => d.Permission).WithMany(p => p.TblRolePermissions)
                .HasForeignKey(d => d.PermissionId)
                .HasConstraintName("FK__tbl_RoleP__Permi__160F4887");

            entity.HasOne(d => d.Role).WithMany(p => p.TblRolePermissions)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("FK__tbl_RoleP__RoleI__151B244E");
        });

        modelBuilder.Entity<TblTemplate>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__tblTempl__3214EC0769945682");

            entity.ToTable("tblTemplates");

            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.ModifiedAt).HasColumnType("datetime");

            entity.HasOne(d => d.Client).WithMany(p => p.TblTemplates)
                .HasForeignKey(d => d.ClientId)
                .HasConstraintName("FK__tblTempla__Clien__41B8C09B");
        });

        modelBuilder.Entity<TblUser>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__tbl_User__3214EC073AFA6D64");

            entity.ToTable("tblUser");

            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.IsActive).HasDefaultValueSql("('true')");
            entity.Property(e => e.IsDelete).HasDefaultValueSql("('false')");
            entity.Property(e => e.Mobile)
                .HasMaxLength(15)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.Password)
                .HasMaxLength(500)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");

            entity.HasOne(d => d.Client).WithMany(p => p.TblUsers)
                .HasForeignKey(d => d.ClientId)
                .HasConstraintName("FK__tbl_User__Client__2180FB33");
        });

        modelBuilder.Entity<TblUserAppRole>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__tblUserA__3214EC0734D6D21C");

            entity.ToTable("tblUserAppRole");

            entity.HasOne(d => d.Role).WithMany(p => p.TblUserAppRoles)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("FK__tblUserAp__RoleI__245D67DE");

            entity.HasOne(d => d.User).WithMany(p => p.TblUserAppRoles)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__tblUserAp__UserI__25518C17");
        });

        modelBuilder.Entity<TblUserRole>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Tbl_User__3214EC07BF879BE9");

            entity.ToTable("tblUser_Role");

            entity.HasOne(d => d.Role).WithMany(p => p.TblUserRoles)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("FK__Tbl_UserR__RoleI__22751F6C");

            entity.HasOne(d => d.User).WithMany(p => p.TblUserRoles)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Tbl_UserR__UserI__236943A5");
        });

        modelBuilder.Entity<TblWalkIn>(entity =>
        {
            entity.HasKey(e => e.WalkInId).HasName("PK_CampusWalkIn");

            entity.ToTable("tblWalkIn");

            entity.Property(e => e.Address1)
                .HasMaxLength(100)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.Address2)
                .HasMaxLength(100)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.JobDescription).UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.Title)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.WalkInDate).HasColumnType("datetime");

            entity.HasOne(d => d.CityNavigation).WithMany(p => p.TblWalkIns)
                .HasForeignKey(d => d.City)
                .HasConstraintName("FK__WalkIn__City__3A4CA8FD");

            entity.HasOne(d => d.Client).WithMany(p => p.TblWalkIns)
                .HasForeignKey(d => d.ClientId)
                .HasConstraintName("FK__tblWalkIn__Clien__2F9A1060");

            entity.HasOne(d => d.Country).WithMany(p => p.TblWalkIns)
                .HasForeignKey(d => d.CountryId)
                .HasConstraintName("FK_WalkIn_Country");

            entity.HasOne(d => d.State).WithMany(p => p.TblWalkIns)
                .HasForeignKey(d => d.StateId)
                .HasConstraintName("FK_WalkIn_State");
        });

        modelBuilder.Entity<TblWalkInCollege>(entity =>
        {
            entity.HasKey(e => e.CampusId).HasName("PK_CampusCollege");

            entity.ToTable("tblWalkInCollege");

            entity.Property(e => e.CollegeCode)
                .HasMaxLength(255)
                .HasDefaultValueSql("('ABB')")
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.StartDateTime).HasColumnType("datetime");

            entity.HasOne(d => d.College).WithMany(p => p.TblWalkInColleges)
                .HasForeignKey(d => d.CollegeId)
                .HasConstraintName("FK_CampusCollege_College");

            entity.HasOne(d => d.WalkIn).WithMany(p => p.TblWalkInColleges)
                .HasForeignKey(d => d.WalkInId)
                .HasConstraintName("FK_CampusCollege_CampusWalkIn");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
