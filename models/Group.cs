using System;
using System.Collections.Generic;

namespace College_managemnt_system.models;

public partial class Group
{
    public int GroupId { get; set; }

    public string? GroupName { get; set; }

    public int? StudentsYear { get; set; }

    public int? SemesterId { get; set; }

    public virtual ICollection<SchedulesJoinsgroup> SchedulesJoinsgroups { get; set; } = new List<SchedulesJoinsgroup>();

    public virtual Semester? Semester { get; set; }

    public virtual ICollection<StudentsJoinsgroup> StudentsJoinsgroups { get; set; } = new List<StudentsJoinsgroup>();
}
