using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace QuickCampus_DAL.Context;

public partial class BtprojecQuickcampusContext : DbContext
{
    public BtprojecQuickcampusContext()
    {
    }

    public BtprojecQuickcampusContext(DbContextOptions<BtprojecQuickcampusContext> options)
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

    public virtual DbSet<City> Cities { get; set; }

    public virtual DbSet<College> Colleges { get; set; }

    public virtual DbSet<Company> Companies { get; set; }

    public virtual DbSet<Country> Countries { get; set; }

    public virtual DbSet<Error> Errors { get; set; }

    public virtual DbSet<Groupdl> Groupdls { get; set; }

    public virtual DbSet<MstAppRole> MstAppRoles { get; set; }

    public virtual DbSet<MstMenuItem> MstMenuItems { get; set; }

    public virtual DbSet<MstMenuSubItem> MstMenuSubItems { get; set; }

    public virtual DbSet<Question> Questions { get; set; }

    public virtual DbSet<QuestionOption> QuestionOptions { get; set; }

    public virtual DbSet<QuestionType> QuestionTypes { get; set; }

    public virtual DbSet<Section> Sections { get; set; }

    public virtual DbSet<State> States { get; set; }

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
        => optionsBuilder.UseSqlServer("Server=103.93.16.117;Database=btprojec_quickcampus;TrustServerCertificate=true;user id=btprojec_admin;password=Bwy0w65ixN*bsE9wy;Integrated Security=false");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .HasDefaultSchema("btprojec_admin")
            .UseCollation("SQL_Latin1_General_CP1_CI_AS");

        modelBuilder.Entity<Applicant>(entity =>
        {
            entity.ToTable("Applicant", "dbo");

            entity.Property(e => e.ClientId).HasColumnName("ClientID");
            entity.Property(e => e.CollegeName).HasMaxLength(255);
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.EmailAddress).HasMaxLength(100);
            entity.Property(e => e.FirstName).HasMaxLength(100);
            entity.Property(e => e.HigestQualification).HasMaxLength(100);
            entity.Property(e => e.HigestQualificationPercentage).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.IntermediatePercentage).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.LastName).HasMaxLength(100);
            entity.Property(e => e.MatricPercentage).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
            entity.Property(e => e.PhoneNumber).HasMaxLength(25);
            entity.Property(e => e.Skills).HasMaxLength(500);

            entity.HasOne(d => d.AssignedToCompanyNavigation).WithMany(p => p.Applicants)
                .HasForeignKey(d => d.AssignedToCompany)
                .HasConstraintName("FK_Applicant_Company");

            entity.HasOne(d => d.Status).WithMany(p => p.Applicants)
                .HasForeignKey(d => d.StatusId)
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
            entity.Property(e => e.Description).HasMaxLength(50);
            entity.Property(e => e.Role).HasMaxLength(50);
        });

        modelBuilder.Entity<ApplicationUser>(entity =>
        {
            entity.ToTable("ApplicationUser", "dbo");

            entity.HasIndex(e => e.UserName, "UK_UserName_User").IsUnique();

            entity.Property(e => e.ApplicationUserId).ValueGeneratedNever();
            entity.Property(e => e.Password).HasMaxLength(50);
            entity.Property(e => e.Token).HasMaxLength(1);
            entity.Property(e => e.UserName).HasMaxLength(50);
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
                .HasDefaultValueSql("('ABB')");
            entity.Property(e => e.StartDateTime).HasColumnType("datetime");

            entity.HasOne(d => d.College).WithMany(p => p.CampusWalkInColleges)
                .HasForeignKey(d => d.CollegeId)
                .HasConstraintName("FK_CampusCollege_College");

            entity.HasOne(d => d.WalkIn).WithMany(p => p.CampusWalkInColleges)
                .HasForeignKey(d => d.WalkInId)
                .HasConstraintName("FK_CampusCollege_CampusWalkIn");
        });

        modelBuilder.Entity<City>(entity =>
        {
            entity.HasKey(e => e.CityId).HasName("PK__City__F2D21B7678E7EEB8");

            entity.ToTable("City", "dbo");

            entity.Property(e => e.CityName)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

            entity.HasOne(d => d.State).WithMany(p => p.Cities)
                .HasForeignKey(d => d.StateId)
                .HasConstraintName("FK__City__StateId__41B8C09B");
        });

        modelBuilder.Entity<College>(entity =>
        {
            entity.ToTable("College", "dbo");

            entity.Property(e => e.Address1).HasMaxLength(50);
            entity.Property(e => e.Address2).HasMaxLength(50);
            entity.Property(e => e.CollegeCode).HasMaxLength(50);
            entity.Property(e => e.CollegeName).HasMaxLength(250);
            entity.Property(e => e.ContectEmail).HasMaxLength(100);
            entity.Property(e => e.ContectPerson).HasMaxLength(100);
            entity.Property(e => e.ContectPhone).HasMaxLength(100);
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Logo)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

            entity.HasOne(d => d.City).WithMany(p => p.Colleges)
                .HasForeignKey(d => d.CityId)
                .HasConstraintName("FK__College__CityId__42ACE4D4");
        });

        modelBuilder.Entity<Company>(entity =>
        {
            entity.ToTable("Company", "dbo");

            entity.Property(e => e.CompanyId).ValueGeneratedNever();
            entity.Property(e => e.CompanyName).HasMaxLength(150);
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Isdeleted).HasColumnName("ISDeleted");
            entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<Country>(entity =>
        {
            entity.ToTable("Country", "dbo");

            entity.Property(e => e.CountryId).ValueGeneratedNever();
            entity.Property(e => e.CountryName).HasMaxLength(50);
        });

        modelBuilder.Entity<Error>(entity =>
        {
            entity.ToTable("Error", "dbo");

            entity.Property(e => e.Date).HasColumnType("datetime");
            entity.Property(e => e.Error1)
                .HasColumnType("text")
                .HasColumnName("Error");
        });

        modelBuilder.Entity<Groupdl>(entity =>
        {
            entity.HasKey(e => e.GroupId).HasName("PK_Group");

            entity.ToTable("Groupdl", "dbo");

            entity.Property(e => e.GroupId).ValueGeneratedNever();
            entity.Property(e => e.GroupName).HasMaxLength(50);

            entity.HasOne(d => d.Clent).WithMany(p => p.Groupdls)
                .HasForeignKey(d => d.ClentId)
                .HasConstraintName("FK__Groupdl__ClentId__625A9A57");
        });

        modelBuilder.Entity<MstAppRole>(entity =>
        {
            entity.HasKey(e => e.AppRoleId).HasName("PK__MstAppRo__E66DD698E86F9486");

            entity.ToTable("MstAppRole", "dbo");

            entity.Property(e => e.AppRoleName).HasMaxLength(100);
            entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");
            entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");
        });

        modelBuilder.Entity<MstMenuItem>(entity =>
        {
            entity.HasKey(e => e.ItemId).HasName("PK__MstMenuI__727E838B869ADFD2");

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
            entity.HasKey(e => e.SubItemId).HasName("PK__MstMenuS__8A6B7585B9B1DBCF");

            entity.ToTable("MstMenuSubItems", "dbo");

            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");
            entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");
            entity.Property(e => e.ModifiedAt).HasColumnType("datetime");
            entity.Property(e => e.SubItemDisplayName).HasMaxLength(1000);
            entity.Property(e => e.SubItemName).HasMaxLength(100);

            entity.HasOne(d => d.Item).WithMany(p => p.MstMenuSubItems)
                .HasForeignKey(d => d.ItemId)
                .HasConstraintName("FK__MstMenuSu__ItemI__46486B8E");
        });

        modelBuilder.Entity<Question>(entity =>
        {
            entity.ToTable("Question", "dbo");

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

            entity.Property(e => e.OptionImage).HasMaxLength(100);

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
                .HasColumnName("QuestionType");

            entity.HasOne(d => d.Clent).WithMany(p => p.QuestionTypes)
                .HasForeignKey(d => d.ClentId)
                .HasConstraintName("FK__QuestionT__Clent__607251E5");
        });

        modelBuilder.Entity<Section>(entity =>
        {
            entity.HasKey(e => e.SectionId).HasName("PK_ExaminationSection");

            entity.ToTable("Section", "dbo");

            entity.Property(e => e.SectionId).ValueGeneratedNever();
            entity.Property(e => e.Section1)
                .HasMaxLength(50)
                .HasColumnName("Section");
        });

        modelBuilder.Entity<State>(entity =>
        {
            entity.ToTable("State", "dbo");

            entity.Property(e => e.StateId).ValueGeneratedNever();
            entity.Property(e => e.StateName).HasMaxLength(150);

            entity.HasOne(d => d.Country).WithMany(p => p.States)
                .HasForeignKey(d => d.CountryId)
                .HasConstraintName("FK_State_Country");
        });

        modelBuilder.Entity<Status>(entity =>
        {
            entity.ToTable("Status", "dbo");

            entity.Property(e => e.StatusId).ValueGeneratedNever();
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
            entity.Property(e => e.StatusName).HasMaxLength(200);
        });

        modelBuilder.Entity<TblClient>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__tbl_Clie__3214EC078E237C90");

            entity.ToTable("tbl_Client", "dbo");

            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Latitude).HasColumnType("decimal(20, 2)");
            entity.Property(e => e.Longitude).HasColumnType("decimal(20, 2)");
            entity.Property(e => e.ModofiedDate).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Password).HasMaxLength(100);
            entity.Property(e => e.UserName).HasMaxLength(100);

            entity.HasOne(d => d.User).WithMany(p => p.TblClients)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__tbl_Clien__UserI__52E34C9D");
        });

        modelBuilder.Entity<TblContent>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Tbl_Cont__3214EC07BE73DE67");

            entity.ToTable("Tbl_Content", "dbo");

            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.Description).IsUnicode(false);
            entity.Property(e => e.ExternalLink)
                .HasMaxLength(1000)
                .IsUnicode(false)
                .HasColumnName("External_link");
            entity.Property(e => e.FileName)
                .HasMaxLength(1000)
                .IsUnicode(false)
                .HasColumnName("File_name");
            entity.Property(e => e.ModefiedOn).HasColumnType("datetime");
            entity.Property(e => e.Name)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.TagId).HasColumnName("Tag_Id");
            entity.Property(e => e.TopicId).HasColumnName("Topic_Id");
            entity.Property(e => e.Type)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.Client).WithMany(p => p.TblContents)
                .HasForeignKey(d => d.ClientId)
                .HasConstraintName("FK__Tbl_Conte__Clien__2AD55B43");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.TblContentCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("FK__Tbl_Conte__Creat__2BC97F7C");

            entity.HasOne(d => d.Deparment).WithMany(p => p.TblContents)
                .HasForeignKey(d => d.DeparmentId)
                .HasConstraintName("FK__Tbl_Conte__Depar__29E1370A");

            entity.HasOne(d => d.ModefiedByNavigation).WithMany(p => p.TblContentModefiedByNavigations)
                .HasForeignKey(d => d.ModefiedBy)
                .HasConstraintName("FK__Tbl_Conte__Modef__2CBDA3B5");

            entity.HasOne(d => d.Tag).WithMany(p => p.TblContents)
                .HasForeignKey(d => d.TagId)
                .HasConstraintName("FK__Tbl_Conte__Tag_I__28ED12D1");

            entity.HasOne(d => d.Topic).WithMany(p => p.TblContents)
                .HasForeignKey(d => d.TopicId)
                .HasConstraintName("FK__Tbl_Conte__Topic__27F8EE98");
        });

        modelBuilder.Entity<TblDepartment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Tbl_Depa__3214EC0722A741B0");

            entity.ToTable("Tbl_Department", "dbo");

            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.DepartmentName).HasMaxLength(100);
            entity.Property(e => e.Description).IsUnicode(false);
            entity.Property(e => e.ModefiedOn).HasColumnType("datetime");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.TblDepartments)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("FK__Tbl_Depar__Creat__09746778");
        });

        modelBuilder.Entity<TblGoal>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__tbl_Goal__3214EC07F7186BCD");

            entity.ToTable("tbl_Goal", "dbo");

            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.Goal).HasMaxLength(200);
            entity.Property(e => e.ModefiedOn).HasColumnType("datetime");
            entity.Property(e => e.TargetYear).HasMaxLength(10);

            entity.HasOne(d => d.Client).WithMany(p => p.TblGoals)
                .HasForeignKey(d => d.ClientId)
                .HasConstraintName("FK__tbl_Goal__Client__0D44F85C");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.TblGoalCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("FK__tbl_Goal__Create__0E391C95");

            entity.HasOne(d => d.ModefiedByNavigation).WithMany(p => p.TblGoalModefiedByNavigations)
                .HasForeignKey(d => d.ModefiedBy)
                .HasConstraintName("FK__tbl_Goal__Modefi__0F2D40CE");
        });

        modelBuilder.Entity<TblMenuItemUserPermission>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__tblMenuI__3214EC075FC1DFB7");

            entity.ToTable("tblMenuItemUserPermission", "dbo");

            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");
            entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");
            entity.Property(e => e.ModifiedAt).HasColumnType("datetime");

            entity.HasOne(d => d.User).WithMany(p => p.TblMenuItemUserPermissions)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__tblMenuIt__UserI__4B0D20AB");
        });

        modelBuilder.Entity<TblParentSkill>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Tbl_Pare__3214EC07BD3F66C1");

            entity.ToTable("Tbl_ParentSkill", "dbo");

            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.Description).IsUnicode(false);
            entity.Property(e => e.ModefiedOn).HasColumnType("datetime");
            entity.Property(e => e.Name)
                .HasMaxLength(200)
                .IsUnicode(false);

            entity.HasOne(d => d.Client).WithMany(p => p.TblParentSkills)
                .HasForeignKey(d => d.ClientId)
                .HasConstraintName("FK__Tbl_Paren__Clien__16CE6296");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.TblParentSkillCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("FK__Tbl_Paren__Creat__17C286CF");

            entity.HasOne(d => d.ModefiedByNavigation).WithMany(p => p.TblParentSkillModefiedByNavigations)
                .HasForeignKey(d => d.ModefiedBy)
                .HasConstraintName("FK__Tbl_Paren__Modef__18B6AB08");
        });

        modelBuilder.Entity<TblPermission>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Tbl_perm__3214EC07E8B524C3");

            entity.ToTable("Tbl_permission", "dbo");

            entity.Property(e => e.PermissionDisplay).HasMaxLength(100);
            entity.Property(e => e.PermissionName).HasMaxLength(100);
        });

        modelBuilder.Entity<TblRole>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__tbl_Role__3214EC07D7A69E98");

            entity.ToTable("tbl_Role", "dbo");

            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.ModifiedBy).HasColumnName("modifiedBy");
            entity.Property(e => e.ModofiedDate).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(50);

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.TblRoleCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("FK__tbl_Role__Create__1CBC4616");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.TblRoleModifiedByNavigations)
                .HasForeignKey(d => d.ModifiedBy)
                .HasConstraintName("FK__tbl_Role__modifi__1DB06A4F");
        });

        modelBuilder.Entity<TblRolePermission>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__RolePerm__3214EC07E1F91B9B");

            entity.ToTable("tbl_RolePermission", "dbo");

            entity.HasOne(d => d.Permission).WithMany(p => p.TblRolePermissions)
                .HasForeignKey(d => d.PermissionId)
                .HasConstraintName("FK__tbl_RoleP__Permi__3C34F16F");

            entity.HasOne(d => d.Role).WithMany(p => p.TblRolePermissions)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("FK__RolePermi__RoleI__08B54D69");
        });

        modelBuilder.Entity<TblTag>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Tbl_Tags__3214EC07929A02C5");

            entity.ToTable("Tbl_Tags", "dbo");

            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.ModefiedOn).HasColumnType("datetime");
            entity.Property(e => e.Tag)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.Client).WithMany(p => p.TblTags)
                .HasForeignKey(d => d.ClientId)
                .HasConstraintName("FK__Tbl_Tags__Client__1209AD79");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.TblTagCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("FK__Tbl_Tags__Create__12FDD1B2");

            entity.HasOne(d => d.ModefiedByNavigation).WithMany(p => p.TblTagModefiedByNavigations)
                .HasForeignKey(d => d.ModefiedBy)
                .HasConstraintName("FK__Tbl_Tags__Modefi__13F1F5EB");
        });

        modelBuilder.Entity<TblTopicPCChildSkill>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Tbl_Topi__3214EC076DAEA623");

            entity.ToTable("Tbl_Topic_P_C_ChildSkill", "dbo");

            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.Description).IsUnicode(false);
            entity.Property(e => e.ModefiedOn).HasColumnType("datetime");
            entity.Property(e => e.Name)
                .HasMaxLength(200)
                .IsUnicode(false);

            entity.HasOne(d => d.Child).WithMany(p => p.TblTopicPCChildSkills)
                .HasForeignKey(d => d.ChildId)
                .HasConstraintName("FK__Tbl_Topic__Child__22401542");

            entity.HasOne(d => d.Client).WithMany(p => p.TblTopicPCChildSkills)
                .HasForeignKey(d => d.ClientId)
                .HasConstraintName("FK__Tbl_Topic__Clien__2334397B");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.TblTopicPCChildSkillCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("FK__Tbl_Topic__Creat__24285DB4");

            entity.HasOne(d => d.ModefiedByNavigation).WithMany(p => p.TblTopicPCChildSkillModefiedByNavigations)
                .HasForeignKey(d => d.ModefiedBy)
                .HasConstraintName("FK__Tbl_Topic__Modef__251C81ED");
        });

        modelBuilder.Entity<TblTopicPChildSkill>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Tbl_Topi__3214EC074CD55139");

            entity.ToTable("Tbl_Topic_P_ChildSkill", "dbo");

            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.Description).IsUnicode(false);
            entity.Property(e => e.ModefiedOn).HasColumnType("datetime");
            entity.Property(e => e.Name)
                .HasMaxLength(200)
                .IsUnicode(false);

            entity.HasOne(d => d.Client).WithMany(p => p.TblTopicPChildSkills)
                .HasForeignKey(d => d.ClientId)
                .HasConstraintName("FK__Tbl_Topic__Clien__1C873BEC");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.TblTopicPChildSkillCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("FK__Tbl_Topic__Creat__1D7B6025");

            entity.HasOne(d => d.ModefiedByNavigation).WithMany(p => p.TblTopicPChildSkillModefiedByNavigations)
                .HasForeignKey(d => d.ModefiedBy)
                .HasConstraintName("FK__Tbl_Topic__Modef__1E6F845E");

            entity.HasOne(d => d.Parent).WithMany(p => p.TblTopicPChildSkills)
                .HasForeignKey(d => d.ParentId)
                .HasConstraintName("FK__Tbl_Topic__Paren__1B9317B3");
        });

        modelBuilder.Entity<TblUser>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__tbl_User__3214EC0755C239B7");

            entity.ToTable("tbl_User", "dbo");

            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.IsActive).HasDefaultValueSql("('true')");
            entity.Property(e => e.IsDelete).HasDefaultValueSql("('false')");
            entity.Property(e => e.Mobile).HasMaxLength(15);
            entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Password).HasMaxLength(500);

            entity.HasOne(d => d.Client).WithMany(p => p.TblUsers)
                .HasForeignKey(d => d.ClientId)
                .HasConstraintName("FK__tbl_User__Client__3A4CA8FD");
        });

        modelBuilder.Entity<TblUserAppRole>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__tblUserA__3214EC074F60A7E4");

            entity.ToTable("tblUserAppRole", "dbo");

            entity.HasOne(d => d.Role).WithMany(p => p.TblUserAppRoles)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("FK__tblUserAp__RoleI__1D4655FB");

            entity.HasOne(d => d.User).WithMany(p => p.TblUserAppRoles)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__tblUserAp__UserI__1C5231C2");
        });

        modelBuilder.Entity<TblUserRole>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Tbl_User__3214EC072A498D72");

            entity.ToTable("Tbl_UserRole", "dbo");

            entity.HasOne(d => d.Role).WithMany(p => p.TblUserRoles)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("FK__Tbl_UserR__RoleI__0C85DE4D");

            entity.HasOne(d => d.User).WithMany(p => p.TblUserRoles)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Tbl_UserR__UserI__0B91BA14");
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
                .HasColumnName("name");
        });

        modelBuilder.Entity<WalkIn>(entity =>
        {
            entity.HasKey(e => e.WalkInId).HasName("PK_CampusWalkIn");

            entity.ToTable("WalkIn", "dbo");

            entity.Property(e => e.Address1).HasMaxLength(100);
            entity.Property(e => e.Address2).HasMaxLength(100);
            entity.Property(e => e.City).HasMaxLength(50);
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Title).HasMaxLength(250);
            entity.Property(e => e.WalkInDate).HasColumnType("datetime");

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
