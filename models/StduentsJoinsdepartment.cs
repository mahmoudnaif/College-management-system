using System;
using System.Collections.Generic;

namespace College_managemnt_system.models;

public partial class StduentsJoinsdepartment
{
    public int DepartmentId { get; set; }

    public int StudentId { get; set; }

    public bool IsActive { get; set; }

    public virtual Department Department { get; set; } = null!;

    public virtual Student Student { get; set; } = null!;
}
