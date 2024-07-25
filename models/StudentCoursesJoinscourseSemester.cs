using System;
using System.Collections.Generic;

namespace College_managemnt_system.models;

public partial class StudentCoursesJoinscourseSemester
{
    public int StudentCourseId { get; set; }

    public int CourseSemesterId { get; set; }

    public bool IsActive { get; set; }

    public virtual Coursesemester CourseSemester { get; set; } = null!;

    public virtual StudentCourse StudentCourse { get; set; } = null!;
}
