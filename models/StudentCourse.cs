﻿using System;
using System.Collections.Generic;

namespace College_managemnt_system.models;

public partial class StudentCourse
{
    public int StudentCourseId { get; set; }

    public int? StudentId { get; set; }

    public string? Grade { get; set; }

    public string? Status { get; set; }

    public virtual Student? Student { get; set; }

    public virtual ICollection<StudentCoursesJoinscourseSemester> StudentCoursesJoinscourseSemesters { get; set; } = new List<StudentCoursesJoinscourseSemester>();
}
