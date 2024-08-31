using System;
using System.Collections.Generic;

namespace College_managemnt_system.models;

public partial class Department
{
    public int DepartmentId { get; set; }

    public string DepartmentName { get; set; } = null!;

    public virtual ICollection<Course> Courses { get; set; } = new List<Course>();

    public virtual ICollection<Professor> Professors { get; set; } = new List<Professor>();


    public virtual ICollection<Student> Students { get; set; } = new List<Student>();
}
