using System;
using System.Collections.Generic;

namespace College_managemnt_system.models;

public partial class Prereq
{
    public int CourseId { get; set; }

    public int PrereqCourseId { get; set; }

    public bool IsActive { get; set; }

    public virtual Course Course { get; set; } = null!;

    public virtual Course PrereqCourse { get; set; } = null!;
}
