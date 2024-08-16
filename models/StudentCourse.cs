using System;
using System.Collections.Generic;

namespace College_managemnt_system.models;

public partial class StudentCourse
{
    public int StudentId { get; set; }

    public int CourseSemesterId { get; set; }

    public string Grade { get; set; } = null!;

    public bool IsFinished { get; set; }

    public virtual Coursesemester CourseSemester { get; set; } = null!;

    public virtual Student Student { get; set; } = null!;
}
