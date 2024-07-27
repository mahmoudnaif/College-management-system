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

    public virtual DbSet<Professor> Professors { get; set; }

    public virtual DbSet<Schedule> Schedules { get; set; }

    public virtual DbSet<SchedulesJoinsgroup> SchedulesJoinsgroups { get; set; }

    public virtual DbSet<Semester> Semesters { get; set; }

    public virtual DbSet<StduentsJoinsdepartment> StduentsJoinsdepartments { get; set; }

    public virtual DbSet<Student> Students { get; set; }

    public virtual DbSet<StudentCourse> StudentCourses { get; set; }

    public virtual DbSet<StudentCoursesJoinscourseSemester> StudentCoursesJoinscourseSemesters { get; set; }

    public virtual DbSet<StudentsJoinsgroup> StudentsJoinsgroups { get; set; }

    public virtual DbSet<TeachingAssistance> TeachingAssistances { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\learning\\collegeSystem\\College managemnt system\\CollegeDB\\CollegeDB.mdf;Integrated Security=True;Connect Timeout=30");

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
            entity.HasKey(e => new { e.AssistantId, e.CourseSemesterId }).HasName("AssistantsJOINSCourseSemesters$PrimaryKey");

            entity.ToTable("AssistantsJOINSCourseSemesters");

            entity.HasIndex(e => e.AssistantId, "AssistantsJOINSCourseSemesters$AssistantID");

            entity.HasIndex(e => e.CourseSemesterId, "AssistantsJOINSCourseSemesters$CourseSemesterID");

            entity.Property(e => e.AssistantId).HasColumnName("AssistantID");
            entity.Property(e => e.CourseSemesterId).HasColumnName("CourseSemesterID");
            entity.Property(e => e.IsActive).HasDefaultValue(true);

            entity.HasOne(d => d.Assistant).WithMany(p => p.AssistantsJoinscourseSemesters)
                .HasForeignKey(d => d.AssistantId)
                .HasConstraintName("AssistantsJOINSCourseSemesters$TeachingAssistancesAssistantsJOINSCourseSemesters");

            entity.HasOne(d => d.CourseSemester).WithMany(p => p.AssistantsJoinscourseSemesters)
                .HasForeignKey(d => d.CourseSemesterId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("AssistantsJOINSCourseSemesters$CoursesemestersAssistantsJOINSCourseSemesters");
        });

        modelBuilder.Entity<Classroom>(entity =>
        {
            entity.HasKey(e => e.ClassroomId).HasName("Classrooms$PrimaryKey");

            entity.HasIndex(e => e.ClassroomId, "Classrooms$ClassroomID");

            entity.Property(e => e.ClassroomId).HasColumnName("ClassroomID");
            entity.Property(e => e.Building)
                .HasMaxLength(255)
                .HasColumnName("building");
            entity.Property(e => e.Capacity).HasDefaultValue(0);
            entity.Property(e => e.RoomNumber).HasMaxLength(255);
        });

        modelBuilder.Entity<Course>(entity =>
        {
            entity.HasKey(e => e.CourseId).HasName("Courses$PrimaryKey");

            entity.HasIndex(e => e.CourseCode, "Courses$CourseCode").IsUnique();

            entity.HasIndex(e => e.CourseId, "Courses$CourseID");

            entity.HasIndex(e => e.DepartmentId, "Courses$DepartmentID");

            entity.Property(e => e.CourseId).HasColumnName("CourseID");
            entity.Property(e => e.CourseCode).HasMaxLength(255);
            entity.Property(e => e.CourseName).HasMaxLength(255);
            entity.Property(e => e.Credits).HasDefaultValue(0);
            entity.Property(e => e.DepartmentId)
                .HasDefaultValue(0)
                .HasColumnName("DepartmentID");

            entity.HasOne(d => d.Department).WithMany(p => p.Courses)
                .HasForeignKey(d => d.DepartmentId)
                .HasConstraintName("Courses$DepartmentsCourses");

            entity.HasMany(d => d.Courses).WithMany(p => p.PrereqCourses)
                .UsingEntity<Dictionary<string, object>>(
                    "Prereq",
                    r => r.HasOne<Course>().WithMany()
                        .HasForeignKey("CourseId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("Prereqs$CoursesPrereqs1"),
                    l => l.HasOne<Course>().WithMany()
                        .HasForeignKey("PrereqCourseId")
                        .HasConstraintName("Prereqs$CoursesPrereqs"),
                    j =>
                    {
                        j.HasKey("CourseId", "PrereqCourseId").HasName("Prereqs$PrimaryKey");
                        j.ToTable("Prereqs");
                        j.HasIndex(new[] { "CourseId" }, "Prereqs$CourseID");
                        j.IndexerProperty<int>("CourseId").HasColumnName("CourseID");
                        j.IndexerProperty<int>("PrereqCourseId").HasColumnName("PrereqCourseID");
                    });

            entity.HasMany(d => d.PrereqCourses).WithMany(p => p.Courses)
                .UsingEntity<Dictionary<string, object>>(
                    "Prereq",
                    r => r.HasOne<Course>().WithMany()
                        .HasForeignKey("PrereqCourseId")
                        .HasConstraintName("Prereqs$CoursesPrereqs"),
                    l => l.HasOne<Course>().WithMany()
                        .HasForeignKey("CourseId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("Prereqs$CoursesPrereqs1"),
                    j =>
                    {
                        j.HasKey("CourseId", "PrereqCourseId").HasName("Prereqs$PrimaryKey");
                        j.ToTable("Prereqs");
                        j.HasIndex(new[] { "CourseId" }, "Prereqs$CourseID");
                        j.IndexerProperty<int>("CourseId").HasColumnName("CourseID");
                        j.IndexerProperty<int>("PrereqCourseId").HasColumnName("PrereqCourseID");
                    });
        });

        modelBuilder.Entity<Coursesemester>(entity =>
        {
            entity.HasKey(e => e.CourseSemesterId).HasName("Coursesemesters$PrimaryKey");

            entity.HasIndex(e => e.CourseId, "Coursesemesters$CourseID");

            entity.HasIndex(e => e.CourseSemesterId, "Coursesemesters$CourseSemesterID");

            entity.HasIndex(e => e.ProfessorId, "Coursesemesters$ProfessorID");

            entity.Property(e => e.CourseSemesterId).HasColumnName("CourseSemesterID");
            entity.Property(e => e.CourseId)
                .HasDefaultValue(0)
                .HasColumnName("CourseID");
            entity.Property(e => e.Isactive)
                .HasDefaultValue(false)
                .HasColumnName("ISActive");
            entity.Property(e => e.ProfessorId)
                .HasDefaultValue(0)
                .HasColumnName("ProfessorID");
            entity.Property(e => e.SemesterId)
                .HasDefaultValue(0)
                .HasColumnName("SemesterID");
            entity.Property(e => e.SsmaTimeStamp)
                .IsRowVersion()
                .IsConcurrencyToken()
                .HasColumnName("SSMA_TimeStamp");

            entity.HasOne(d => d.Course).WithMany(p => p.Coursesemesters)
                .HasForeignKey(d => d.CourseId)
                .HasConstraintName("Coursesemesters$CoursesCoursesemesters");

            entity.HasOne(d => d.Professor).WithMany(p => p.Coursesemesters)
                .HasForeignKey(d => d.ProfessorId)
                .HasConstraintName("Coursesemesters$ProfessorsCoursesemesters");

            entity.HasOne(d => d.Semester).WithMany(p => p.Coursesemesters)
                .HasForeignKey(d => d.SemesterId)
                .HasConstraintName("Coursesemesters$SemestersCoursesemesters");
        });

        modelBuilder.Entity<Department>(entity =>
        {
            entity.HasKey(e => e.DepartmentId).HasName("Departments$PrimaryKey");

            entity.Property(e => e.DepartmentId).HasColumnName("DepartmentID");
            entity.Property(e => e.DepartmentName).HasMaxLength(255);
        });

        modelBuilder.Entity<Group>(entity =>
        {
            entity.HasKey(e => e.GroupId).HasName("Groups$PrimaryKey");

            entity.HasIndex(e => e.GroupId, "Groups$GroupID");

            entity.HasIndex(e => e.SemesterId, "Groups$SemesterID");

            entity.Property(e => e.GroupId).HasColumnName("GroupID");
            entity.Property(e => e.GroupName).HasMaxLength(255);
            entity.Property(e => e.SemesterId)
                .HasDefaultValue(0)
                .HasColumnName("SemesterID");
            entity.Property(e => e.StudentsYear).HasDefaultValue(0);

            entity.HasOne(d => d.Semester).WithMany(p => p.Groups)
                .HasForeignKey(d => d.SemesterId)
                .HasConstraintName("Groups$SemestersGroups");
        });

        modelBuilder.Entity<Professor>(entity =>
        {
            entity.HasKey(e => e.ProfessorId).HasName("Professors$PrimaryKey");

            entity.HasIndex(e => e.DepartmentId, "Professors$DepartmentID");

            entity.HasIndex(e => e.ProfessorId, "Professors$ProfessorID");

            entity.HasIndex(e => e.AccountId, "UQ__Professo__F267253F89A41818").IsUnique();

            entity.Property(e => e.ProfessorId).HasColumnName("ProfessorID");
            entity.Property(e => e.AccountId).HasColumnName("accountID");
            entity.Property(e => e.DepartmentId)
                .HasDefaultValue(0)
                .HasColumnName("DepartmentID");
            entity.Property(e => e.FirstName).HasMaxLength(255);
            entity.Property(e => e.HiringDate).HasPrecision(0);
            entity.Property(e => e.LastName).HasMaxLength(255);
            entity.Property(e => e.Phone).HasMaxLength(255);

            entity.HasOne(d => d.Account).WithOne(p => p.Professor)
                .HasForeignKey<Professor>(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Professors_Accounts");

            entity.HasOne(d => d.Department).WithMany(p => p.Professors)
                .HasForeignKey(d => d.DepartmentId)
                .HasConstraintName("Professors$DepartmentsProfessors");
        });

        modelBuilder.Entity<Schedule>(entity =>
        {
            entity.HasKey(e => e.ScheduleId).HasName("Schedules$PrimaryKey");

            entity.HasIndex(e => e.ClassroomId, "Schedules$ClassroomID");

            entity.HasIndex(e => e.CourseSemesterId, "Schedules$CourseSemesterID");

            entity.HasIndex(e => e.ScheduleId, "Schedules$ScheduleID");

            entity.Property(e => e.ScheduleId).HasColumnName("ScheduleID");
            entity.Property(e => e.ClassroomId)
                .HasDefaultValue(0)
                .HasColumnName("ClassroomID");
            entity.Property(e => e.CourseSemesterId)
                .HasDefaultValue(0)
                .HasColumnName("CourseSemesterID");
            entity.Property(e => e.DayOfWeek).HasMaxLength(255);
            entity.Property(e => e.EndTime).HasPrecision(0);
            entity.Property(e => e.PeriodNumber).HasDefaultValue(0);
            entity.Property(e => e.StartTime).HasPrecision(0);
            entity.Property(e => e.Type)
                .HasMaxLength(255)
                .HasColumnName("type");

            entity.HasOne(d => d.Classroom).WithMany(p => p.Schedules)
                .HasForeignKey(d => d.ClassroomId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("Schedules$ClassroomsSchedules");

            entity.HasOne(d => d.CourseSemester).WithMany(p => p.Schedules)
                .HasForeignKey(d => d.CourseSemesterId)
                .HasConstraintName("Schedules$CoursesemestersSchedules");
        });

        modelBuilder.Entity<SchedulesJoinsgroup>(entity =>
        {
            entity.HasKey(e => new { e.ScheduleId, e.GroupId }).HasName("SchedulesJOINSGroups$PrimaryKey");

            entity.ToTable("SchedulesJOINSGroups");

            entity.HasIndex(e => e.GroupId, "SchedulesJOINSGroups$GroupID");

            entity.HasIndex(e => e.ScheduleId, "SchedulesJOINSGroups$ScheduleID");

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

            entity.HasIndex(e => e.SemesterId, "Semesters$SemesterID");

            entity.Property(e => e.SemesterId).HasColumnName("SemesterID");
            entity.Property(e => e.EndDate).HasPrecision(0);
            entity.Property(e => e.IsActive).HasColumnName("isActive");
            entity.Property(e => e.SemesterName).HasMaxLength(255);
            entity.Property(e => e.StartDate).HasPrecision(0);
        });

        modelBuilder.Entity<StduentsJoinsdepartment>(entity =>
        {
            entity.HasKey(e => new { e.DepartmentId, e.StudentId }).HasName("StduentsJOINSDepartment$PrimaryKey");

            entity.ToTable("StduentsJOINSDepartment");

            entity.HasIndex(e => e.StudentId, "StduentsJOINSDepartment$StudentID");

            entity.Property(e => e.DepartmentId).HasColumnName("DepartmentID");
            entity.Property(e => e.StudentId).HasColumnName("StudentID");
            entity.Property(e => e.IsActive).HasDefaultValue(true);

            entity.HasOne(d => d.Department).WithMany(p => p.StduentsJoinsdepartments)
                .HasForeignKey(d => d.DepartmentId)
                .HasConstraintName("StduentsJOINSDepartment$DepartmentsStduentsJOINSDepartment");

            entity.HasOne(d => d.Student).WithMany(p => p.StduentsJoinsdepartments)
                .HasForeignKey(d => d.StudentId)
                .HasConstraintName("StduentsJOINSDepartment$StudentsStduentsJOINSDepartment");
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity.HasKey(e => e.StudentId).HasName("Students$PrimaryKey");

            entity.HasIndex(e => e.StudentId, "Students$StudentID");

            entity.HasIndex(e => e.AccountId, "UQ__Students__F267253F253690E5").IsUnique();

            entity.Property(e => e.StudentId).HasColumnName("StudentID");
            entity.Property(e => e.AccountId).HasColumnName("accountID");
            entity.Property(e => e.Cgpa)
                .HasDefaultValue(0)
                .HasColumnName("CGPA");
            entity.Property(e => e.EnrollmentDate).HasPrecision(0);
            entity.Property(e => e.FirstName).HasMaxLength(255);
            entity.Property(e => e.LastName).HasMaxLength(255);
            entity.Property(e => e.Phone).HasMaxLength(255);

            entity.HasOne(d => d.Account).WithOne(p => p.Student)
                .HasForeignKey<Student>(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Student_Accounts");
        });

        modelBuilder.Entity<StudentCourse>(entity =>
        {
            entity.HasKey(e => e.StudentCourseId).HasName("StudentCourses$PrimaryKey");

            entity.HasIndex(e => e.StudentCourseId, "StudentCourses$StudentCourseID");

            entity.HasIndex(e => e.StudentId, "StudentCourses$StudentID");

            entity.Property(e => e.StudentCourseId).HasColumnName("StudentCourseID");
            entity.Property(e => e.Grade).HasMaxLength(255);
            entity.Property(e => e.Status).HasMaxLength(255);
            entity.Property(e => e.StudentId)
                .HasDefaultValue(0)
                .HasColumnName("StudentID");

            entity.HasOne(d => d.Student).WithMany(p => p.StudentCourses)
                .HasForeignKey(d => d.StudentId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("StudentCourses$StudentsStudentCourses");
        });

        modelBuilder.Entity<StudentCoursesJoinscourseSemester>(entity =>
        {
            entity.HasKey(e => new { e.StudentCourseId, e.CourseSemesterId }).HasName("StudentCoursesJOINSCourseSemesters$PrimaryKey");

            entity.ToTable("StudentCoursesJOINSCourseSemesters");

            entity.HasIndex(e => e.CourseSemesterId, "StudentCoursesJOINSCourseSemesters$CourseSemesterID");

            entity.HasIndex(e => e.StudentCourseId, "StudentCoursesJOINSCourseSemesters$StudentCourseID");

            entity.Property(e => e.StudentCourseId).HasColumnName("StudentCourseID");
            entity.Property(e => e.CourseSemesterId).HasColumnName("CourseSemesterID");
            entity.Property(e => e.IsActive).HasDefaultValue(true);

            entity.HasOne(d => d.CourseSemester).WithMany(p => p.StudentCoursesJoinscourseSemesters)
                .HasForeignKey(d => d.CourseSemesterId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("StudentCoursesJOINSCourseSemesters$CoursesemestersStudentCourseJOINSCourseSemesterID");

            entity.HasOne(d => d.StudentCourse).WithMany(p => p.StudentCoursesJoinscourseSemesters)
                .HasForeignKey(d => d.StudentCourseId)
                .HasConstraintName("StudentCoursesJOINSCourseSemesters$StudentCoursesStudentCourseJOINSCourseSemesterID");
        });

        modelBuilder.Entity<StudentsJoinsgroup>(entity =>
        {
            entity.HasKey(e => new { e.StudentId, e.GroupId }).HasName("StudentsJOINSGroups$PrimaryKey");

            entity.ToTable("StudentsJOINSGroups");

            entity.HasIndex(e => e.StudentId, "StudentsJOINSGroups$StudentID");

            entity.Property(e => e.StudentId).HasColumnName("StudentID");
            entity.Property(e => e.GroupId).HasColumnName("GroupID");
            entity.Property(e => e.IsActive).HasDefaultValue(true);

            entity.HasOne(d => d.Group).WithMany(p => p.StudentsJoinsgroups)
                .HasForeignKey(d => d.GroupId)
                .HasConstraintName("StudentsJOINSGroups$GroupsStudentsJOINSGroups");

            entity.HasOne(d => d.Student).WithMany(p => p.StudentsJoinsgroups)
                .HasForeignKey(d => d.StudentId)
                .HasConstraintName("StudentsJOINSGroups$StudentsStudentsJOINSGroups");
        });

        modelBuilder.Entity<TeachingAssistance>(entity =>
        {
            entity.HasKey(e => e.AssistantId).HasName("TeachingAssistances$PrimaryKey");

            entity.HasIndex(e => e.AssistantId, "TeachingAssistances$AssistantID");

            entity.HasIndex(e => e.AccountId, "UQ__Teaching__F267253FFF770244").IsUnique();

            entity.Property(e => e.AssistantId).HasColumnName("AssistantID");
            entity.Property(e => e.AccountId).HasColumnName("accountID");
            entity.Property(e => e.FirstName).HasMaxLength(255);
            entity.Property(e => e.HiringDate).HasPrecision(0);
            entity.Property(e => e.LastName).HasMaxLength(255);
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
