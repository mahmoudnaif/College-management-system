using System;
using System.Collections.Generic;

namespace College_managemnt_system.models;

public partial class StudentCourse
{
    public int StudentId { get; set; }

    public int SemesterId { get; set; }

    public int CourseId { get; set; }

    public string Grade { get; set; } = null!;

    public string Status { get; set; } = null!;

    public virtual Coursesemester Coursesemester { get; set; } = null!;

    public virtual Student Student { get; set; } = null!;
}
