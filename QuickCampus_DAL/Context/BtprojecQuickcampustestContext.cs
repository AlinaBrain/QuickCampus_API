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

    public virtual DbSet<Applicant> Applicants { get; set; }

    public virtual DbSet<ApplicantAnswerSummary> ApplicantAnswerSummaries { get; set; }

    public virtual DbSet<ApplicantComment> ApplicantComments { get; set; }

    public virtual DbSet<ApplicantTest> ApplicantTests { get; set; }

    public virtual DbSet<ApplicantTestSummary> ApplicantTestSummaries { get; set; }

    public virtual DbSet<ApplicationRole> ApplicationRoles { get; set; }

    public virtual DbSet<ApplicationUser> ApplicationUsers { get; set; }

    public virtual DbSet<ApplicationUserRole> ApplicationUserRoles { get; set; }

    public virtual DbSet<CampusWalkInCollege> CampusWalkInColleges { get; set; }

    public virtual DbSet<College> Colleges { get; set; }

    public virtual DbSet<Company> Companies { get; set; }

    public virtual DbSet<Error> Errors { get; set; }

    public virtual DbSet<Groupdl> Groupdls { get; set; }

    public virtual DbSet<MstAppRole> MstAppRoles { get; set; }

    public virtual DbSet<MstCity> MstCities { get; set; }

    public virtual DbSet<MstCityState> MstCityStates { get; set; }

    public virtual DbSet<MstCityStateCountry> MstCityStateCountries { get; set; }

    public virtual DbSet<MstMenuItem> MstMenuItems { get; set; }

    public virtual DbSet<MstMenuSubItem> MstMenuSubItems { get; set; }

    public virtual DbSet<MstQualification> MstQualifications { get; set; }

    public virtual DbSet<Question> Questions { get; set; }

    public virtual DbSet<QuestionOption> QuestionOptions { get; set; }

    public virtual DbSet<QuestionType> QuestionTypes { get; set; }

    public virtual DbSet<Section> Sections { get; set; }

    public virtual DbSet<Status> Statuses { get; set; }

    public virtual DbSet<TblClient> TblClients { get; set; }

    public virtual DbSet<TblContent> TblContents { get; set; }

    public virtual DbSet<TblDepartment> TblDepartments { get; set; }

    public virtual DbSet<TblGoal> TblGoals { get; set; }

    public virtual DbSet<TblMenuItemUserPermission> TblMenuItemUserPermissions { get; set; }

    public virtual DbSet<TblParentSkill> TblParentSkills { get; set; }

    public virtual DbSet<TblPermission> TblPermissions { get; set; }

    public virtual DbSet<TblRole> TblRoles { get; set; }

    public virtual DbSet<TblRolePermission> TblRolePermissions { get; set; }

    public virtual DbSet<TblTag> TblTags { get; set; }

    public virtual DbSet<TblTopicPCChildSkill> TblTopicPCChildSkills { get; set; }

    public virtual DbSet<TblTopicPChildSkill> TblTopicPChildSkills { get; set; }

    public virtual DbSet<TblUser> TblUsers { get; set; }

    public virtual DbSet<TblUserAppRole> TblUserAppRoles { get; set; }

    public virtual DbSet<TblUserRole> TblUserRoles { get; set; }

    public virtual DbSet<TempTable> TempTables { get; set; }

    public virtual DbSet<WalkIn> WalkIns { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=103.93.16.117;Database=btprojec_quickcampustest;TrustServerCertificate=true;user id=btprojec_admin;password=Bwy0w65ixN*bsE9wy;Integrated Security=false;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("btprojec_admin");

        modelBuilder.Entity<Applicant>(entity =>
        {
            entity.ToTable("Applicant", "dbo");

            entity.Property(e => e.ClientId).HasColumnName("ClientID");
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
            entity.Property(e => e.Skills)
                .HasMaxLength(500)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");

            entity.HasOne(d => d.AssignedToCompanyNavigation).WithMany(p => p.Applicants)
                .HasForeignKey(d => d.AssignedToCompany)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Applicant_Company");

            entity.HasOne(d => d.HighestQualificationNavigation).WithMany(p => p.Applicants)
                .HasForeignKey(d => d.HighestQualification)
                .HasConstraintName("FK__Applicant__Highe__5E8A0973");

            entity.HasOne(d => d.Status).WithMany(p => p.Applicants)
                .HasForeignKey(d => d.StatusId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Applicant_Status");
        });

        modelBuilder.Entity<ApplicantAnswerSummary>(entity =>
        {
            entity.HasKey(e => e.AnswerSummaryId);

            entity.ToTable("ApplicantAnswerSummary", "dbo");

            entity.HasOne(d => d.Answer).WithMany(p => p.ApplicantAnswerSummaries)
                .HasForeignKey(d => d.AnswerId)
                .HasConstraintName("FK_ApplicantAnswerSummary_Answer");

            entity.HasOne(d => d.Summary).WithMany(p => p.ApplicantAnswerSummaries)
                .HasForeignKey(d => d.SummaryId)
                .HasConstraintName("FK_ApplicantAnswerSummary_ApplicantTestSummary");
        });

        modelBuilder.Entity<ApplicantComment>(entity =>
        {
            entity.HasKey(e => e.CommentId);

            entity.ToTable("ApplicantComment", "dbo");

            entity.Property(e => e.CommentedOn).HasColumnType("datetime");
            entity.Property(e => e.Description).UseCollation("SQL_Latin1_General_CP1_CI_AS");

            entity.HasOne(d => d.Applicant).WithMany(p => p.ApplicantComments)
                .HasForeignKey(d => d.ApplicantId)
                .HasConstraintName("FK_ApplicantComment_Applicant");
        });

        modelBuilder.Entity<ApplicantTest>(entity =>
        {
            entity.HasKey(e => e.TestId).HasName("PK_ApplicantExamination");

            entity.ToTable("ApplicantTest", "dbo");

            entity.Property(e => e.TestDate).HasColumnType("datetime");

            entity.HasOne(d => d.Applicant).WithMany(p => p.ApplicantTests)
                .HasForeignKey(d => d.ApplicantId)
                .HasConstraintName("FK_ApplicantExamination_Applicant");

            entity.HasOne(d => d.Campus).WithMany(p => p.ApplicantTests)
                .HasForeignKey(d => d.CampusId)
                .HasConstraintName("FK_ApplicantTest_CampusWalkInCollege");

            entity.HasOne(d => d.WalkIn).WithMany(p => p.ApplicantTests)
                .HasForeignKey(d => d.WalkInId)
                .HasConstraintName("FK_ApplicantExamination_CampusWalkIn");
        });

        modelBuilder.Entity<ApplicantTestSummary>(entity =>
        {
            entity.HasKey(e => e.SummaryId).HasName("PK_ExaminationDetail");

            entity.ToTable("ApplicantTestSummary", "dbo");

            entity.Property(e => e.Answer).UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.SubmittedOn).HasColumnType("datetime");

            entity.HasOne(d => d.Question).WithMany(p => p.ApplicantTestSummaries)
                .HasForeignKey(d => d.QuestionId)
                .HasConstraintName("FK_ExaminationDetail_Question");

            entity.HasOne(d => d.Test).WithMany(p => p.ApplicantTestSummaries)
                .HasForeignKey(d => d.TestId)
                .HasConstraintName("FK_ExaminationDetail_ApplicantExamination");
        });

        modelBuilder.Entity<ApplicationRole>(entity =>
        {
            entity.ToTable("ApplicationRole", "dbo");

            entity.Property(e => e.ApplicationRoleId).ValueGeneratedNever();
            entity.Property(e => e.Description)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.Role)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
        });

        modelBuilder.Entity<ApplicationUser>(entity =>
        {
            entity.ToTable("ApplicationUser", "dbo");

            entity.HasIndex(e => e.UserName, "UK_UserName_User").IsUnique();

            entity.Property(e => e.ApplicationUserId).ValueGeneratedNever();
            entity.Property(e => e.Password)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.Token)
                .HasMaxLength(1)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.UserName)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
        });

        modelBuilder.Entity<ApplicationUserRole>(entity =>
        {
            entity.ToTable("ApplicationUserRole", "dbo");

            entity.HasOne(d => d.ApplicationRole).WithMany(p => p.ApplicationUserRoles)
                .HasForeignKey(d => d.ApplicationRoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ApplicationUserRole_ApplicationRole");

            entity.HasOne(d => d.ApplicationUser).WithMany(p => p.ApplicationUserRoles)
                .HasForeignKey(d => d.ApplicationUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ApplicationUserRole_ApplicationUser");
        });

        modelBuilder.Entity<CampusWalkInCollege>(entity =>
        {
            entity.HasKey(e => e.CampusId).HasName("PK_CampusCollege");

            entity.ToTable("CampusWalkInCollege", "dbo");

            entity.Property(e => e.CollegeCode)
                .HasMaxLength(255)
                .HasDefaultValueSql("('ABB')")
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.StartDateTime).HasColumnType("datetime");

            entity.HasOne(d => d.College).WithMany(p => p.CampusWalkInColleges)
                .HasForeignKey(d => d.CollegeId)
                .HasConstraintName("FK_CampusCollege_College");

            entity.HasOne(d => d.WalkIn).WithMany(p => p.CampusWalkInColleges)
                .HasForeignKey(d => d.WalkInId)
                .HasConstraintName("FK_CampusCollege_CampusWalkIn");
        });

        modelBuilder.Entity<College>(entity =>
        {
            entity.ToTable("College", "dbo");

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

            entity.HasOne(d => d.City).WithMany(p => p.Colleges)
                .HasForeignKey(d => d.CityId)
                .HasConstraintName("FK__College__CityId__7E37BEF6");
        });

        modelBuilder.Entity<Company>(entity =>
        {
            entity.ToTable("Company", "dbo");

            entity.Property(e => e.CompanyId).ValueGeneratedNever();
            entity.Property(e => e.CompanyName)
                .HasMaxLength(150)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Isdeleted).HasColumnName("ISDeleted");
            entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<Error>(entity =>
        {
            entity.ToTable("Error", "dbo");

            entity.Property(e => e.Date).HasColumnType("datetime");
            entity.Property(e => e.Error1)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnType("text")
                .HasColumnName("Error");
        });

        modelBuilder.Entity<Groupdl>(entity =>
        {
            entity.HasKey(e => e.GroupId).HasName("PK_Group");

            entity.ToTable("Groupdl", "dbo");

            entity.Property(e => e.GroupId).ValueGeneratedNever();
            entity.Property(e => e.GroupName)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");

            entity.HasOne(d => d.Clent).WithMany(p => p.Groupdls)
                .HasForeignKey(d => d.ClentId)
                .HasConstraintName("FK__Groupdl__ClentId__7F2BE32F");
        });

        modelBuilder.Entity<MstAppRole>(entity =>
        {
            entity.HasKey(e => e.AppRoleId).HasName("PK__MstAppRo__E66DD698EDF1A5D6");

            entity.ToTable("MstAppRole", "dbo");

            entity.Property(e => e.AppRoleName)
                .HasMaxLength(100)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");
            entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");
        });

        modelBuilder.Entity<MstCity>(entity =>
        {
            entity.HasKey(e => e.CityId).HasName("PK__City__F2D21B760C7D81FB");

            entity.ToTable("MstCity", "dbo");

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

            entity.ToTable("MstCity_State", "dbo");

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

            entity.ToTable("MstCity_State_Country", "dbo");

            entity.Property(e => e.CountryId).ValueGeneratedNever();
            entity.Property(e => e.CountryName)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
        });

        modelBuilder.Entity<MstMenuItem>(entity =>
        {
            entity.HasKey(e => e.ItemId).HasName("PK__MstMenuI__727E838BD5B99911");

            entity.ToTable("MstMenuItems", "dbo");

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

            entity.ToTable("MstMenuSubItems", "dbo");

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

        modelBuilder.Entity<MstQualification>(entity =>
        {
            entity.HasKey(e => e.QualId).HasName("PK__MstQuali__B8C9022335534D56");

            entity.ToTable("MstQualification", "dbo");

            entity.Property(e => e.CreateAt).HasColumnType("datetime");
            entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");
            entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");
            entity.Property(e => e.ModifiedAt).HasColumnType("datetime");
            entity.Property(e => e.QualName).HasMaxLength(200);
        });

        modelBuilder.Entity<Question>(entity =>
        {
            entity.ToTable("Question", "dbo");

            entity.Property(e => e.Text).UseCollation("SQL_Latin1_General_CP1_CI_AS");

            entity.HasOne(d => d.Group).WithMany(p => p.Questions)
                .HasForeignKey(d => d.GroupId)
                .HasConstraintName("FK_Question_Group");

            entity.HasOne(d => d.QuestionType).WithMany(p => p.Questions)
                .HasForeignKey(d => d.QuestionTypeId)
                .HasConstraintName("FK_Question_QuestionType");

            entity.HasOne(d => d.Section).WithMany(p => p.Questions)
                .HasForeignKey(d => d.SectionId)
                .HasConstraintName("FK_Question_ExaminationSection");
        });

        modelBuilder.Entity<QuestionOption>(entity =>
        {
            entity.HasKey(e => e.OptionId).HasName("PK_Answer");

            entity.ToTable("QuestionOption", "dbo");

            entity.Property(e => e.Imagepath).UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.OptionText).UseCollation("SQL_Latin1_General_CP1_CI_AS");

            entity.HasOne(d => d.Question).WithMany(p => p.QuestionOptions)
                .HasForeignKey(d => d.QuestionId)
                .HasConstraintName("FK_Answer_Question");
        });

        modelBuilder.Entity<QuestionType>(entity =>
        {
            entity.ToTable("QuestionType", "dbo");

            entity.Property(e => e.QuestionTypeId).ValueGeneratedNever();
            entity.Property(e => e.QuestionType1)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("QuestionType");

            entity.HasOne(d => d.Clent).WithMany(p => p.QuestionTypes)
                .HasForeignKey(d => d.ClentId)
                .HasConstraintName("FK__QuestionT__Clent__03F0984C");
        });

        modelBuilder.Entity<Section>(entity =>
        {
            entity.HasKey(e => e.SectionId).HasName("PK_ExaminationSection");

            entity.ToTable("Section", "dbo");

            entity.Property(e => e.SectionId).ValueGeneratedNever();
            entity.Property(e => e.Section1)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("Section");
        });

        modelBuilder.Entity<Status>(entity =>
        {
            entity.ToTable("Status", "dbo");

            entity.Property(e => e.StatusId).ValueGeneratedNever();
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
            entity.Property(e => e.StatusName)
                .HasMaxLength(200)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
        });

        modelBuilder.Entity<TblClient>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__tbl_Clie__3214EC0728D8A50C");

            entity.ToTable("tbl_Client", "dbo");

            entity.Property(e => e.Address).UseCollation("SQL_Latin1_General_CP1_CI_AS");
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

            entity.HasOne(d => d.User).WithMany(p => p.TblClients)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__tbl_Clien__UserI__05D8E0BE");
        });

        modelBuilder.Entity<TblContent>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Tbl_Cont__3214EC07C3575B93");

            entity.ToTable("Tbl_Content", "dbo");

            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.Description)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.ExternalLink)
                .HasMaxLength(1000)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("External_link");
            entity.Property(e => e.FileName)
                .HasMaxLength(1000)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("File_name");
            entity.Property(e => e.ModefiedOn).HasColumnType("datetime");
            entity.Property(e => e.Name)
                .HasMaxLength(200)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.TagId).HasColumnName("Tag_Id");
            entity.Property(e => e.TopicId).HasColumnName("Topic_Id");
            entity.Property(e => e.Type)
                .HasMaxLength(100)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");

            entity.HasOne(d => d.Client).WithMany(p => p.TblContents)
                .HasForeignKey(d => d.ClientId)
                .HasConstraintName("FK__Tbl_Conte__Clien__06CD04F7");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.TblContentCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("FK__Tbl_Conte__Creat__07C12930");

            entity.HasOne(d => d.Deparment).WithMany(p => p.TblContents)
                .HasForeignKey(d => d.DeparmentId)
                .HasConstraintName("FK__Tbl_Conte__Depar__08B54D69");

            entity.HasOne(d => d.ModefiedByNavigation).WithMany(p => p.TblContentModefiedByNavigations)
                .HasForeignKey(d => d.ModefiedBy)
                .HasConstraintName("FK__Tbl_Conte__Modef__09A971A2");

            entity.HasOne(d => d.Tag).WithMany(p => p.TblContents)
                .HasForeignKey(d => d.TagId)
                .HasConstraintName("FK__Tbl_Conte__Tag_I__0A9D95DB");

            entity.HasOne(d => d.Topic).WithMany(p => p.TblContents)
                .HasForeignKey(d => d.TopicId)
                .HasConstraintName("FK__Tbl_Conte__Topic__0B91BA14");
        });

        modelBuilder.Entity<TblDepartment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Tbl_Depa__3214EC0731C5B2A4");

            entity.ToTable("Tbl_Department", "dbo");

            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.DepartmentName)
                .HasMaxLength(100)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.Description)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.ModefiedOn).HasColumnType("datetime");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.TblDepartments)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("FK__Tbl_Depar__Creat__0C85DE4D");
        });

        modelBuilder.Entity<TblGoal>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__tbl_Goal__3214EC07134B0881");

            entity.ToTable("tbl_Goal", "dbo");

            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.Goal)
                .HasMaxLength(200)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.ModefiedOn).HasColumnType("datetime");
            entity.Property(e => e.TargetYear)
                .HasMaxLength(10)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");

            entity.HasOne(d => d.Client).WithMany(p => p.TblGoals)
                .HasForeignKey(d => d.ClientId)
                .HasConstraintName("FK__tbl_Goal__Client__0D7A0286");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.TblGoalCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("FK__tbl_Goal__Create__0E6E26BF");

            entity.HasOne(d => d.ModefiedByNavigation).WithMany(p => p.TblGoalModefiedByNavigations)
                .HasForeignKey(d => d.ModefiedBy)
                .HasConstraintName("FK__tbl_Goal__Modefi__0F624AF8");
        });

        modelBuilder.Entity<TblMenuItemUserPermission>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__tblMenuI__3214EC07ECB77D04");

            entity.ToTable("tblMenuItemUserPermission", "dbo");

            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");
            entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");
            entity.Property(e => e.ModifiedAt).HasColumnType("datetime");

            entity.HasOne(d => d.User).WithMany(p => p.TblMenuItemUserPermissions)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__tblMenuIt__UserI__57DD0BE4");
        });

        modelBuilder.Entity<TblParentSkill>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Tbl_Pare__3214EC07C1B6BE7C");

            entity.ToTable("Tbl_ParentSkill", "dbo");

            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.Description)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.ModefiedOn).HasColumnType("datetime");
            entity.Property(e => e.Name)
                .HasMaxLength(200)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");

            entity.HasOne(d => d.Client).WithMany(p => p.TblParentSkills)
                .HasForeignKey(d => d.ClientId)
                .HasConstraintName("FK__Tbl_Paren__Clien__10566F31");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.TblParentSkillCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("FK__Tbl_Paren__Creat__114A936A");

            entity.HasOne(d => d.ModefiedByNavigation).WithMany(p => p.TblParentSkillModefiedByNavigations)
                .HasForeignKey(d => d.ModefiedBy)
                .HasConstraintName("FK__Tbl_Paren__Modef__123EB7A3");
        });

        modelBuilder.Entity<TblPermission>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Tbl_perm__3214EC07F1375A94");

            entity.ToTable("Tbl_permission", "dbo");

            entity.Property(e => e.PermissionDisplay)
                .HasMaxLength(100)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.PermissionName)
                .HasMaxLength(100)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
        });

        modelBuilder.Entity<TblRole>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__tbl_Role__3214EC07D22FB3FE");

            entity.ToTable("tbl_Role", "dbo");

            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.ModifiedBy).HasColumnName("modifiedBy");
            entity.Property(e => e.ModofiedDate).HasColumnType("datetime");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.TblRoleCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("FK__tbl_Role__Create__1332DBDC");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.TblRoleModifiedByNavigations)
                .HasForeignKey(d => d.ModifiedBy)
                .HasConstraintName("FK__tbl_Role__modifi__14270015");
        });

        modelBuilder.Entity<TblRolePermission>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__tbl_Role__3214EC0723025571");

            entity.ToTable("tbl_RolePermission", "dbo");

            entity.HasOne(d => d.Permission).WithMany(p => p.TblRolePermissions)
                .HasForeignKey(d => d.PermissionId)
                .HasConstraintName("FK__tbl_RoleP__Permi__160F4887");

            entity.HasOne(d => d.Role).WithMany(p => p.TblRolePermissions)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("FK__tbl_RoleP__RoleI__151B244E");
        });

        modelBuilder.Entity<TblTag>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Tbl_Tags__3214EC070332C303");

            entity.ToTable("Tbl_Tags", "dbo");

            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.ModefiedOn).HasColumnType("datetime");
            entity.Property(e => e.Tag)
                .HasMaxLength(100)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");

            entity.HasOne(d => d.Client).WithMany(p => p.TblTags)
                .HasForeignKey(d => d.ClientId)
                .HasConstraintName("FK__Tbl_Tags__Client__17036CC0");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.TblTagCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("FK__Tbl_Tags__Create__17F790F9");

            entity.HasOne(d => d.ModefiedByNavigation).WithMany(p => p.TblTagModefiedByNavigations)
                .HasForeignKey(d => d.ModefiedBy)
                .HasConstraintName("FK__Tbl_Tags__Modefi__18EBB532");
        });

        modelBuilder.Entity<TblTopicPCChildSkill>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Tbl_Topi__3214EC077434B10E");

            entity.ToTable("Tbl_Topic_P_C_ChildSkill", "dbo");

            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.Description)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.ModefiedOn).HasColumnType("datetime");
            entity.Property(e => e.Name)
                .HasMaxLength(200)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");

            entity.HasOne(d => d.Child).WithMany(p => p.TblTopicPCChildSkills)
                .HasForeignKey(d => d.ChildId)
                .HasConstraintName("FK__Tbl_Topic__Child__19DFD96B");

            entity.HasOne(d => d.Client).WithMany(p => p.TblTopicPCChildSkills)
                .HasForeignKey(d => d.ClientId)
                .HasConstraintName("FK__Tbl_Topic__Clien__1AD3FDA4");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.TblTopicPCChildSkillCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("FK__Tbl_Topic__Creat__1BC821DD");

            entity.HasOne(d => d.ModefiedByNavigation).WithMany(p => p.TblTopicPCChildSkillModefiedByNavigations)
                .HasForeignKey(d => d.ModefiedBy)
                .HasConstraintName("FK__Tbl_Topic__Modef__1CBC4616");
        });

        modelBuilder.Entity<TblTopicPChildSkill>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Tbl_Topi__3214EC070687A866");

            entity.ToTable("Tbl_Topic_P_ChildSkill", "dbo");

            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.Description)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.ModefiedOn).HasColumnType("datetime");
            entity.Property(e => e.Name)
                .HasMaxLength(200)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");

            entity.HasOne(d => d.Client).WithMany(p => p.TblTopicPChildSkills)
                .HasForeignKey(d => d.ClientId)
                .HasConstraintName("FK__Tbl_Topic__Clien__1DB06A4F");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.TblTopicPChildSkillCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("FK__Tbl_Topic__Creat__1EA48E88");

            entity.HasOne(d => d.ModefiedByNavigation).WithMany(p => p.TblTopicPChildSkillModefiedByNavigations)
                .HasForeignKey(d => d.ModefiedBy)
                .HasConstraintName("FK__Tbl_Topic__Modef__1F98B2C1");

            entity.HasOne(d => d.Parent).WithMany(p => p.TblTopicPChildSkills)
                .HasForeignKey(d => d.ParentId)
                .HasConstraintName("FK__Tbl_Topic__Paren__208CD6FA");
        });

        modelBuilder.Entity<TblUser>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__tbl_User__3214EC073AFA6D64");

            entity.ToTable("tbl_User", "dbo");

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

            entity.ToTable("tblUserAppRole", "dbo");

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

            entity.ToTable("Tbl_UserRole", "dbo");

            entity.HasOne(d => d.Role).WithMany(p => p.TblUserRoles)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("FK__Tbl_UserR__RoleI__22751F6C");

            entity.HasOne(d => d.User).WithMany(p => p.TblUserRoles)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Tbl_UserR__UserI__236943A5");
        });

        modelBuilder.Entity<TempTable>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("temp_table", "dbo");

            entity.Property(e => e.Age).HasColumnName("age");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("name");
        });

        modelBuilder.Entity<WalkIn>(entity =>
        {
            entity.HasKey(e => e.WalkInId).HasName("PK_CampusWalkIn");

            entity.ToTable("WalkIn", "dbo");

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

            entity.HasOne(d => d.CityNavigation).WithMany(p => p.WalkIns)
                .HasForeignKey(d => d.City)
                .HasConstraintName("FK__WalkIn__City__3A4CA8FD");

            entity.HasOne(d => d.Country).WithMany(p => p.WalkIns)
                .HasForeignKey(d => d.CountryId)
                .HasConstraintName("FK_WalkIn_Country");

            entity.HasOne(d => d.State).WithMany(p => p.WalkIns)
                .HasForeignKey(d => d.StateId)
                .HasConstraintName("FK_WalkIn_State");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
