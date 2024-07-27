using System;
using System.Collections.Generic;

namespace College_managemnt_system.models;

public partial class Professor
{
    public int ProfessorId { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? Phone { get; set; }

    public DateTime? HiringDate { get; set; }

    public int? DepartmentId { get; set; }

    public long AccountId { get; set; }

    public virtual Account Account { get; set; } = null!;

    public virtual ICollection<Coursesemester> Coursesemesters { get; set; } = new List<Coursesemester>();

    public virtual Department? Department { get; set; }
}
