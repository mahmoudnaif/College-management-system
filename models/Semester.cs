using System;
using System.Collections.Generic;

namespace College_managemnt_system.models;

public partial class Semester
{
    public int SemesterId { get; set; }

    public string SemesterName { get; set; } = null!;

    public int SemesterYear { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public bool IsActive { get; set; }

    public virtual ICollection<Coursesemester> Coursesemesters { get; set; } = new List<Coursesemester>();

    public virtual ICollection<Group> Groups { get; set; } = new List<Group>();
}
