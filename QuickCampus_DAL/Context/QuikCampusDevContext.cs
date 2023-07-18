using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace QuickCampus_DAL.Context;

public partial class QuikCampusDevContext : DbContext
{
    public QuikCampusDevContext()
    {
    }

    public QuikCampusDevContext(DbContextOptions<QuikCampusDevContext> options)
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

    public virtual DbSet<Country> Countries { get; set; }

    public virtual DbSet<Error> Errors { get; set; }

    public virtual DbSet<Groupdl> Groupdls { get; set; }

    public virtual DbSet<Question> Questions { get; set; }

    public virtual DbSet<QuestionOption> QuestionOptions { get; set; }

    public virtual DbSet<QuestionType> QuestionTypes { get; set; }

    public virtual DbSet<Section> Sections { get; set; }

    public virtual DbSet<State> States { get; set; }

    public virtual DbSet<Status> Statuses { get; set; }

    public virtual DbSet<TblClient> TblClients { get; set; }

    public virtual DbSet<TblPermission> TblPermissions { get; set; }

    public virtual DbSet<TblRole> TblRoles { get; set; }

    public virtual DbSet<TblRolePermission> TblRolePermissions { get; set; }

    public virtual DbSet<TblUser> TblUsers { get; set; }

    public virtual DbSet<TblUserRole> TblUserRoles { get; set; }

    public virtual DbSet<WalkIn> WalkIns { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=103.133.215.42,1434;Database=QuikCampusDev;TrustServerCertificate=true;user id=bt;password=Tech#90@428;Integrated Security=false;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Applicant>(entity =>
        {
            entity.ToTable("Applicant");

            entity.Property(e => e.ApplicantToken).HasMaxLength(50);
            entity.Property(e => e.CollegeName).HasMaxLength(255);
            entity.Property(e => e.EmailAddress).HasMaxLength(100);
            entity.Property(e => e.FirstName).HasMaxLength(100);
            entity.Property(e => e.HigestQualification).HasMaxLength(100);
            entity.Property(e => e.HigestQualificationPercentage).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.IntermediatePercentage).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.LastName).HasMaxLength(100);
            entity.Property(e => e.MatricPercentage).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.PhoneNumber).HasMaxLength(25);
            entity.Property(e => e.RegisteredDate).HasColumnType("datetime");
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

            entity.ToTable("ApplicantAnswerSummary");

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

            entity.ToTable("ApplicantComment");

            entity.Property(e => e.CommentedOn).HasColumnType("datetime");

            entity.HasOne(d => d.Applicant).WithMany(p => p.ApplicantComments)
                .HasForeignKey(d => d.ApplicantId)
                .HasConstraintName("FK_ApplicantComment_Applicant");
        });

        modelBuilder.Entity<ApplicantTest>(entity =>
        {
            entity.HasKey(e => e.TestId).HasName("PK_ApplicantExamination");

            entity.ToTable("ApplicantTest");

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

            entity.ToTable("ApplicantTestSummary");

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
            entity.ToTable("ApplicationRole");

            entity.Property(e => e.ApplicationRoleId).ValueGeneratedNever();
            entity.Property(e => e.Description).HasMaxLength(50);
            entity.Property(e => e.Role).HasMaxLength(50);
        });

        modelBuilder.Entity<ApplicationUser>(entity =>
        {
            entity.ToTable("ApplicationUser");

            entity.HasIndex(e => e.UserName, "UK_UserName_User").IsUnique();

            entity.Property(e => e.ApplicationUserId).ValueGeneratedNever();
            entity.Property(e => e.Password).HasMaxLength(50);
            entity.Property(e => e.Token).HasMaxLength(1);
            entity.Property(e => e.UserName).HasMaxLength(50);
        });

        modelBuilder.Entity<ApplicationUserRole>(entity =>
        {
            entity.ToTable("ApplicationUserRole");

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

            entity.ToTable("CampusWalkInCollege");

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

        modelBuilder.Entity<College>(entity =>
        {
            entity.ToTable("College");

            entity.Property(e => e.Address1).HasMaxLength(50);
            entity.Property(e => e.Address2).HasMaxLength(50);
            entity.Property(e => e.City)
                .HasMaxLength(50)
                .IsFixedLength();
            entity.Property(e => e.CollegeCode).HasMaxLength(50);
            entity.Property(e => e.CollegeName).HasMaxLength(250);
            entity.Property(e => e.ContectEmail).HasMaxLength(100);
            entity.Property(e => e.ContectPerson).HasMaxLength(100);
            entity.Property(e => e.ContectPhone).HasMaxLength(100);
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Logo).HasMaxLength(50);

            entity.HasOne(d => d.Country).WithMany(p => p.Colleges)
                .HasForeignKey(d => d.CountryId)
                .HasConstraintName("FK_College_Country");

            entity.HasOne(d => d.State).WithMany(p => p.Colleges)
                .HasForeignKey(d => d.StateId)
                .HasConstraintName("FK_College_State");
        });

        modelBuilder.Entity<Company>(entity =>
        {
            entity.ToTable("Company");

            entity.Property(e => e.CompanyId).ValueGeneratedNever();
            entity.Property(e => e.CompanyName).HasMaxLength(150);
        });

        modelBuilder.Entity<Country>(entity =>
        {
            entity.ToTable("Country");

            entity.Property(e => e.CountryId).ValueGeneratedNever();
            entity.Property(e => e.CountryName).HasMaxLength(50);
        });

        modelBuilder.Entity<Error>(entity =>
        {
            entity.ToTable("Error");

            entity.Property(e => e.Date).HasColumnType("datetime");
            entity.Property(e => e.Error1)
                .HasColumnType("text")
                .HasColumnName("Error");
        });

        modelBuilder.Entity<Groupdl>(entity =>
        {
            entity.HasKey(e => e.GroupId).HasName("PK_Group");

            entity.ToTable("Groupdl");

            entity.Property(e => e.GroupId).ValueGeneratedNever();
            entity.Property(e => e.GroupName).HasMaxLength(50);

            entity.HasOne(d => d.Clent).WithMany(p => p.Groupdls)
                .HasForeignKey(d => d.ClentId)
                .HasConstraintName("FK__Groupdl__ClentId__625A9A57");
        });

        modelBuilder.Entity<Question>(entity =>
        {
            entity.ToTable("Question");

            entity.HasOne(d => d.Clent).WithMany(p => p.Questions)
                .HasForeignKey(d => d.ClentId)
                .HasConstraintName("FK__Question__ClentI__4D5F7D71");

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

            entity.ToTable("QuestionOption");

            entity.Property(e => e.OptionImage).HasMaxLength(100);

            entity.HasOne(d => d.Question).WithMany(p => p.QuestionOptions)
                .HasForeignKey(d => d.QuestionId)
                .HasConstraintName("FK_Answer_Question");
        });

        modelBuilder.Entity<QuestionType>(entity =>
        {
            entity.ToTable("QuestionType");

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

            entity.ToTable("Section");

            entity.Property(e => e.SectionId).ValueGeneratedNever();
            entity.Property(e => e.Section1)
                .HasMaxLength(50)
                .HasColumnName("Section");

            entity.HasOne(d => d.Clent).WithMany(p => p.Sections)
                .HasForeignKey(d => d.ClentId)
                .HasConstraintName("FK__Section__ClentId__6166761E");
        });

        modelBuilder.Entity<State>(entity =>
        {
            entity.ToTable("State");

            entity.Property(e => e.StateId).ValueGeneratedNever();
            entity.Property(e => e.StateName).HasMaxLength(150);

            entity.HasOne(d => d.Country).WithMany(p => p.States)
                .HasForeignKey(d => d.CountryId)
                .HasConstraintName("FK_State_Country");
        });

        modelBuilder.Entity<Status>(entity =>
        {
            entity.ToTable("Status");

            entity.Property(e => e.StatusId).ValueGeneratedNever();
            entity.Property(e => e.Status1)
                .HasMaxLength(50)
                .HasColumnName("Status");
        });

        modelBuilder.Entity<TblClient>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__tbl_Clie__3214EC078E237C90");

            entity.ToTable("tbl_Client");

            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Latitude).HasColumnType("decimal(20, 2)");
            entity.Property(e => e.Longitude).HasColumnType("decimal(20, 2)");
            entity.Property(e => e.ModofiedDate).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Password).HasMaxLength(100);
            entity.Property(e => e.UserName).HasMaxLength(100);

            entity.HasOne(d => d.CraetedByNavigation).WithMany(p => p.TblClientCraetedByNavigations)
                .HasForeignKey(d => d.CraetedBy)
                .HasConstraintName("FK__tbl_Clien__Craet__19DFD96B");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.TblClientModifiedByNavigations)
                .HasForeignKey(d => d.ModifiedBy)
                .HasConstraintName("FK__tbl_Clien__Modif__1AD3FDA4");
        });

        modelBuilder.Entity<TblPermission>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Tbl_perm__3214EC07E8B524C3");

            entity.ToTable("Tbl_permission");

            entity.Property(e => e.PermissionDisplay).HasMaxLength(100);
            entity.Property(e => e.PermissionName).HasMaxLength(100);
        });

        modelBuilder.Entity<TblRole>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__tbl_Role__3214EC07D7A69E98");

            entity.ToTable("tbl_Role");

            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.ModifiedBy).HasColumnName("modifiedBy");
            entity.Property(e => e.ModofiedDate).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(50);

            entity.HasOne(d => d.Client).WithMany(p => p.TblRoles)
                .HasForeignKey(d => d.ClientId)
                .HasConstraintName("FK__tbl_Role__Client__3B40CD36");

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

            entity.ToTable("tbl_RolePermission");

            entity.Property(e => e.DisplayName).HasMaxLength(500);
            entity.Property(e => e.PermissionName).HasMaxLength(500);

            entity.HasOne(d => d.Permission).WithMany(p => p.TblRolePermissions)
                .HasForeignKey(d => d.PermissionId)
                .HasConstraintName("FK__tbl_RoleP__Permi__3C34F16F");

            entity.HasOne(d => d.Role).WithMany(p => p.TblRolePermissions)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("FK__RolePermi__RoleI__08B54D69");
        });

        modelBuilder.Entity<TblUser>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__tbl_User__3214EC0755C239B7");

            entity.ToTable("tbl_User");

            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.IsActive).HasDefaultValueSql("('true')");
            entity.Property(e => e.IsDelete).HasDefaultValueSql("('false')");
            entity.Property(e => e.Mobile).HasMaxLength(15);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Password).HasMaxLength(500);
            entity.Property(e => e.UserName).HasMaxLength(100);

            entity.HasOne(d => d.Client).WithMany(p => p.TblUsers)
                .HasForeignKey(d => d.ClientId)
                .HasConstraintName("FK__tbl_User__Client__3A4CA8FD");
        });

        modelBuilder.Entity<TblUserRole>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Tbl_User__3214EC072A498D72");

            entity.ToTable("Tbl_UserRole");

            entity.HasOne(d => d.Role).WithMany(p => p.TblUserRoles)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("FK__Tbl_UserR__RoleI__0C85DE4D");

            entity.HasOne(d => d.User).WithMany(p => p.TblUserRoles)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Tbl_UserR__UserI__0B91BA14");
        });

        modelBuilder.Entity<WalkIn>(entity =>
        {
            entity.HasKey(e => e.WalkInId).HasName("PK_CampusWalkIn");

            entity.ToTable("WalkIn");

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
