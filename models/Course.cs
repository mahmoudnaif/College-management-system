using System;
using System.Collections.Generic;

namespace College_managemnt_system.models;

public partial class Course
{
    public int CourseId { get; set; }

    public string CourseName { get; set; } = null!;

    public string CourseCode { get; set; } = null!;

    public int Credits { get; set; }

    public int DepartmentId { get; set; }

    public virtual ICollection<Coursesemester> Coursesemesters { get; set; } = new List<Coursesemester>();

    public virtual Department Department { get; set; } = null!;

    public virtual ICollection<Course> Courses { get; set; } = new List<Course>();

    public virtual ICollection<Course> PrereqCourses { get; set; } = new List<Course>();
}
