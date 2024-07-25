using System;
using System.Collections.Generic;

namespace College_managemnt_system.models;

public partial class Coursesemester
{
    public int CourseSemesterId { get; set; }

    public int? ProfessorId { get; set; }

    public int? CourseId { get; set; }

    public int? SemesterId { get; set; }

    public bool? Isactive { get; set; }

    public byte[] SsmaTimeStamp { get; set; } = null!;

    public virtual ICollection<AssistantsJoinscourseSemester> AssistantsJoinscourseSemesters { get; set; } = new List<AssistantsJoinscourseSemester>();

    public virtual Course? Course { get; set; }

    public virtual Professor? Professor { get; set; }

    public virtual ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();

    public virtual Semester? Semester { get; set; }

    public virtual ICollection<StudentCoursesJoinscourseSemester> StudentCoursesJoinscourseSemesters { get; set; } = new List<StudentCoursesJoinscourseSemester>();
}
