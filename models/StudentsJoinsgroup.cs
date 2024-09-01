using System;
using System.Collections.Generic;

namespace College_managemnt_system.models;

public partial class StudentsJoinsgroup
{
    public int StudentId { get; set; }

    public int GroupId { get; set; }

    public bool IsActive { get; set; }

    public virtual Group Group { get; set; } = null!;

    public virtual Student Student { get; set; } = null!;
}