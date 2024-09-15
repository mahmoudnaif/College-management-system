using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace College_managemnt_system.models;

public partial class CollegeDBContext : DbContext
{
    public CollegeDBContext()
    {
    }

    public CollegeDBContext(DbContextOptions<CollegeDBContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<AssistantsJoinscourseSemester> AssistantsJoinscourseSemesters { get; set; }

    public virtual DbSet<Classroom> Classrooms { get; set; }

    public virtual DbSet<Course> Courses { get; set; }

    public virtual DbSet<Coursesemester> Coursesemesters { get; set; }

    public virtual DbSet<Department> Departments { get; set; }

    public virtual DbSet<Group> Groups { get; set; }

    public virtual DbSet<Prereq> Prereqs { get; set; }

    public virtual DbSet<Professor> Professors { get; set; }

    public virtual DbSet<Schedule> Schedules { get; set; }

    public virtual DbSet<SchedulesJoinsgroup> SchedulesJoinsgroups { get; set; }

    public virtual DbSet<Semester> Semesters { get; set; }

    public virtual DbSet<Student> Students { get; set; }

    public virtual DbSet<StudentCourse> StudentCourses { get; set; }

    public virtual DbSet<StudentsJoinsgroup> StudentsJoinsgroups { get; set; }

    public virtual DbSet<TeachingAssistance> TeachingAssistances { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\learning\\collegeSystem\\college managment system testing area\\College-managemnt-system\\CollegeDB\\CollegeDB.mdf;Integrated Security=True;Connect Timeout=30");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.AccountId).HasName("PK__Accounts__F267253EB1AEB8B9");

            entity.HasIndex(e => e.Email, "UQ__Accounts__AB6E61649F1E9960").IsUnique();

            entity.Property(e => e.AccountId).HasColumnName("accountID");
            entity.Property(e => e.DateCreated)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("dateCreated");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("email");
            entity.Property(e => e.Password).HasColumnName("password");
            entity.Property(e => e.Role)
                .HasMaxLength(50)
                .HasColumnName("role");
        });

        modelBuilder.Entity<AssistantsJoinscourseSemester>(entity =>
        {
            entity.HasKey(e => new { e.AssistantId, e.CourseId, e.SemesterId }).HasName("AssistantsJOINSCourseSemesters$PrimaryKey");

            entity.ToTable("AssistantsJOINSCourseSemesters");

            entity.Property(e => e.AssistantId).HasColumnName("AssistantID");
            entity.Property(e => e.CourseId).HasColumnName("CourseID");
            entity.Property(e => e.SemesterId).HasColumnName("SemesterID");
            entity.Property(e => e.IsActive).HasDefaultValue(true);

            entity.HasOne(d => d.Assistant).WithMany(p => p.AssistantsJoinscourseSemesters)
                .HasForeignKey(d => d.AssistantId)
                .HasConstraintName("AssistantsJOINSCourseSemesters$TeachingAssistancesAssistantsJOINSCourseSemesters");

            entity.HasOne(d => d.Coursesemester).WithMany(p => p.AssistantsJoinscourseSemesters)
                .HasForeignKey(d => new { d.CourseId, d.SemesterId })
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Assistant_coursesemester");
        });

        modelBuilder.Entity<Classroom>(entity =>
        {
            entity.HasKey(e => e.RoomNumber).HasName("Classrooms$PrimaryKey");

            entity.HasIndex(e => e.RoomNumber, "UQ__tmp_ms_x__AE10E07A7154D592").IsUnique();

            entity.Property(e => e.RoomNumber).ValueGeneratedNever();
            entity.Property(e => e.Building).HasColumnName("building");
        });

        modelBuilder.Entity<Course>(entity =>
        {
            entity.HasKey(e => e.CourseId).HasName("Courses$PrimaryKey");

            entity.HasIndex(e => e.CourseCode, "Courses$CourseCode").IsUnique();

            entity.Property(e => e.CourseId).HasColumnName("CourseID");
            entity.Property(e => e.CourseCode).HasMaxLength(255);
            entity.Property(e => e.CourseName).HasMaxLength(255);
            entity.Property(e => e.DepartmentId).HasColumnName("DepartmentID");

            entity.HasOne(d => d.Department).WithMany(p => p.Courses)
                .HasForeignKey(d => d.DepartmentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Courses$DepartmentsCourses");
        });

        modelBuilder.Entity<Coursesemester>(entity =>
        {
            entity.HasKey(e => new { e.CourseId, e.SemesterId }).HasName("Coursesemesters$PrimaryKey");

            entity.HasIndex(e => e.SemesterId, "Coursesemesters$SemesterID");

            entity.Property(e => e.CourseId).HasColumnName("CourseID");
            entity.Property(e => e.SemesterId).HasColumnName("SemesterID");
            entity.Property(e => e.Isactive).HasColumnName("ISActive");
            entity.Property(e => e.ProfessorId).HasColumnName("ProfessorID");

            entity.HasOne(d => d.Course).WithMany(p => p.Coursesemesters)
                .HasForeignKey(d => d.CourseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Coursesemesters$CoursesCoursesemesters");

            entity.HasOne(d => d.Professor).WithMany(p => p.Coursesemesters)
                .HasForeignKey(d => d.ProfessorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Coursesemesters$ProfessorsCoursesemesters");

            entity.HasOne(d => d.Semester).WithMany(p => p.Coursesemesters)
                .HasForeignKey(d => d.SemesterId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Coursesemesters$SemestersCoursesemesters");
        });

        modelBuilder.Entity<Department>(entity =>
        {
            entity.HasKey(e => e.DepartmentId).HasName("Departments$PrimaryKey");

            entity.HasIndex(e => e.DepartmentName, "IX_Department_Name").IsUnique();

            entity.HasIndex(e => e.DepartmentName, "UQ__Departme__D949CC34BD0F849F").IsUnique();

            entity.Property(e => e.DepartmentId).HasColumnName("DepartmentID");
            entity.Property(e => e.DepartmentName).HasMaxLength(255);
        });

        modelBuilder.Entity<Group>(entity =>
        {
            entity.HasKey(e => e.GroupId).HasName("Groups$PrimaryKey");

            entity.HasIndex(e => e.SemesterId, "GroupsIndex$SemesterId");

            entity.HasIndex(e => e.StudentsYear, "GroupsIndex$StudentYear");

            entity.HasIndex(e => new { e.GroupName, e.StudentsYear, e.SemesterId }, "UQ_Groups_GroupName_StudentsYear_SemesterID").IsUnique();

            entity.Property(e => e.GroupId).HasColumnName("GroupID");
            entity.Property(e => e.GroupName).HasMaxLength(255);
            entity.Property(e => e.SemesterId).HasColumnName("SemesterID");

            entity.HasOne(d => d.Semester).WithMany(p => p.Groups)
                .HasForeignKey(d => d.SemesterId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Groups$SemestersGroups");
        });

        modelBuilder.Entity<Prereq>(entity =>
        {
            entity.HasKey(e => new { e.CourseId, e.PrereqCourseId }).HasName("Prereqs$PrimaryKey");

            entity.HasIndex(e => e.CourseId, "Prereqs$CourseID");

            entity.Property(e => e.CourseId).HasColumnName("CourseID");
            entity.Property(e => e.PrereqCourseId).HasColumnName("PrereqCourseID");
            entity.Property(e => e.IsActive).HasDefaultValue(true);

            entity.HasOne(d => d.Course).WithMany(p => p.PrereqCourses)
                .HasForeignKey(d => d.CourseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Prereqs$CoursesPrereqs1");

            entity.HasOne(d => d.PrereqCourse).WithMany(p => p.PrereqPrereqCourses)
                .HasForeignKey(d => d.PrereqCourseId)
                .HasConstraintName("Prereqs$CoursesPrereqs");
        });

        modelBuilder.Entity<Professor>(entity =>
        {
            entity.HasKey(e => e.ProfessorId).HasName("Professors$PrimaryKey");

            entity.HasIndex(e => e.NationalNumber, "UQ__Professo__FEA173C287218C2A").IsUnique();

            entity.HasIndex(e => e.AccountId, "UQ__tmp_ms_x__F267253FC3E2BDE3").IsUnique();

            entity.Property(e => e.ProfessorId).HasColumnName("ProfessorID");
            entity.Property(e => e.AccountId).HasColumnName("accountID");
            entity.Property(e => e.DepartmentId).HasColumnName("DepartmentID");
            entity.Property(e => e.FirstName).HasMaxLength(255);
            entity.Property(e => e.HiringDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.LastName).HasMaxLength(255);
            entity.Property(e => e.NationalNumber).HasMaxLength(255);
            entity.Property(e => e.Phone).HasMaxLength(255);

            entity.HasOne(d => d.Account).WithOne(p => p.Professor)
                .HasForeignKey<Professor>(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Professors_Accounts");

            entity.HasOne(d => d.Department).WithMany(p => p.Professors)
                .HasForeignKey(d => d.DepartmentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Professors$DepartmentsProfessors");
        });

        modelBuilder.Entity<Schedule>(entity =>
        {
            entity.HasKey(e => e.ScheduleId).HasName("Schedules$PrimaryKey");

            entity.HasIndex(e => new { e.RoomNumber, e.DayOfWeek, e.PeriodNumber, e.SemesterId }, "Schedules$UNIQUE").IsUnique();

            entity.Property(e => e.ScheduleId).HasColumnName("ScheduleID");
            entity.Property(e => e.CourseId).HasColumnName("CourseID");
            entity.Property(e => e.SemesterId).HasColumnName("SemesterID");
            entity.Property(e => e.Type)
                .HasMaxLength(255)
                .HasColumnName("type");

            entity.HasOne(d => d.RoomNumberNavigation).WithMany(p => p.Schedules)
                .HasForeignKey(d => d.RoomNumber)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Schedules$ClassroomsSchedules");

            entity.HasOne(d => d.Coursesemester).WithMany(p => p.Schedules)
                .HasForeignKey(d => new { d.CourseId, d.SemesterId })
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Schedule_coursesemester");
        });

        modelBuilder.Entity<SchedulesJoinsgroup>(entity =>
        {
            entity.HasKey(e => new { e.ScheduleId, e.GroupId }).HasName("SchedulesJOINSGroups$PrimaryKey");

            entity.ToTable("SchedulesJOINSGroups");

            entity.HasIndex(e => e.GroupId, "SchedulesJOINSGroups$GroupID");

            entity.HasIndex(e => e.ScheduleId, "SchedulesJOINSGroups$Scheduleid");

            entity.Property(e => e.ScheduleId).HasColumnName("ScheduleID");
            entity.Property(e => e.GroupId).HasColumnName("GroupID");
            entity.Property(e => e.IsActive).HasDefaultValue(true);

            entity.HasOne(d => d.Group).WithMany(p => p.SchedulesJoinsgroups)
                .HasForeignKey(d => d.GroupId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("SchedulesJOINSGroups$GroupsSchedulesJOINSGroups");

            entity.HasOne(d => d.Schedule).WithMany(p => p.SchedulesJoinsgroups)
                .HasForeignKey(d => d.ScheduleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("SchedulesJOINSGroups$SchedulesSchedulesJOINSGroups");
        });

        modelBuilder.Entity<Semester>(entity =>
        {
            entity.HasKey(e => e.SemesterId).HasName("Semesters$PrimaryKey");

            entity.HasIndex(e => new { e.SemesterName, e.SemesterYear }, "UQ_Semesters_SemesterName_SemesterYear").IsUnique();

            entity.Property(e => e.SemesterId).HasColumnName("SemesterID");
            entity.Property(e => e.EndDate).HasPrecision(0);
            entity.Property(e => e.IsActive).HasColumnName("isActive");
            entity.Property(e => e.SemesterName).HasMaxLength(255);
            entity.Property(e => e.StartDate).HasPrecision(0);
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity.HasKey(e => e.StudentId).HasName("Students$PrimaryKey");

            entity.HasIndex(e => e.AccountId, "UQ__tmp_ms_x__F267253F7BB38717").IsUnique();

            entity.Property(e => e.StudentId).HasColumnName("StudentID");
            entity.Property(e => e.AccountId).HasColumnName("accountID");
            entity.Property(e => e.Cgpa).HasColumnName("CGPA");
            entity.Property(e => e.DepartmentId).HasColumnName("DepartmentID");
            entity.Property(e => e.EnrollmentDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.FathertName).HasMaxLength(255);
            entity.Property(e => e.FirstName).HasMaxLength(255);
            entity.Property(e => e.GrandfatherName).HasMaxLength(255);
            entity.Property(e => e.LastName).HasMaxLength(255);
            entity.Property(e => e.NationalNumber).HasMaxLength(255);
            entity.Property(e => e.Phone).HasMaxLength(255);
            entity.Property(e => e.TotalHours).HasColumnName("totalHours");

            entity.HasOne(d => d.Account).WithOne(p => p.Student)
                .HasForeignKey<Student>(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Student_Accounts");

            entity.HasOne(d => d.Department).WithMany(p => p.Students)
                .HasForeignKey(d => d.DepartmentId)
                .HasConstraintName("FK_Students_Departments");
        });

        modelBuilder.Entity<StudentCourse>(entity =>
        {
            entity.HasKey(e => new { e.StudentId, e.SemesterId, e.CourseId }).HasName("StudentCourses$PrimaryKey");

            entity.HasIndex(e => e.StudentId, "StudentCourses");

            entity.HasIndex(e => e.Status, "StudentCourses$Status");

            entity.Property(e => e.StudentId).HasColumnName("StudentID");
            entity.Property(e => e.SemesterId).HasColumnName("SemesterID");
            entity.Property(e => e.CourseId).HasColumnName("CourseID");
            entity.Property(e => e.Grade).HasMaxLength(255);
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasDefaultValue("inprogress");

            entity.HasOne(d => d.Student).WithMany(p => p.StudentCourses)
                .HasForeignKey(d => d.StudentId)
                .HasConstraintName("StudentCourses$StudentsStudentCourses");

            entity.HasOne(d => d.Coursesemester).WithMany(p => p.StudentCourses)
                .HasForeignKey(d => new { d.CourseId, d.SemesterId })
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Student_coursesemester");
        });

        modelBuilder.Entity<StudentsJoinsgroup>(entity =>
        {
            entity.HasKey(e => new { e.StudentId, e.ScheduleId }).HasName("PK_Student_Schedules");

            entity.ToTable("StudentsJOINSGroups");

            entity.HasIndex(e => e.GroupId, "StudentsJOINSGroups$GroupID");

            entity.HasIndex(e => e.ScheduleId, "StudentsJOINSGroups$ScheduleId");

            entity.HasIndex(e => e.StudentId, "StudentsJOINSGroups$StudentID");

            entity.Property(e => e.StudentId).HasColumnName("StudentID");
            entity.Property(e => e.ScheduleId).HasColumnName("ScheduleID");
            entity.Property(e => e.GroupId).HasColumnName("GroupID");
            entity.Property(e => e.IsActive).HasDefaultValue(true);

            entity.HasOne(d => d.Student).WithMany(p => p.StudentsJoinsgroups)
                .HasForeignKey(d => d.StudentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Student_Schedules_Student");

            entity.HasOne(d => d.SchedulesJoinsgroup).WithMany(p => p.StudentsJoinsgroups)
                .HasForeignKey(d => new { d.ScheduleId, d.GroupId })
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Student_Schedules_SchedulesJOINSGroups");
        });

        modelBuilder.Entity<TeachingAssistance>(entity =>
        {
            entity.HasKey(e => e.AssistantId).HasName("TeachingAssistances$PrimaryKey");

            entity.HasIndex(e => e.NationalNumber, "UQ__Teaching__FEA173C245005A77").IsUnique();

            entity.HasIndex(e => e.AccountId, "UQ__tmp_ms_x__F267253F3E55EA9D").IsUnique();

            entity.Property(e => e.AssistantId).HasColumnName("AssistantID");
            entity.Property(e => e.AccountId).HasColumnName("accountID");
            entity.Property(e => e.FirstName).HasMaxLength(255);
            entity.Property(e => e.HiringDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.LastName).HasMaxLength(255);
            entity.Property(e => e.NationalNumber).HasMaxLength(255);
            entity.Property(e => e.Phone)
                .HasMaxLength(255)
                .HasColumnName("phone");

            entity.HasOne(d => d.Account).WithOne(p => p.TeachingAssistance)
                .HasForeignKey<TeachingAssistance>(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TeachingAssistances_Accounts");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
