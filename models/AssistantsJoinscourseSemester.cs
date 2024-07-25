using System;
using System.Collections.Generic;

namespace College_managemnt_system.models;

public partial class AssistantsJoinscourseSemester
{
    public int AssistantId { get; set; }

    public int CourseSemesterId { get; set; }

    public bool IsActive { get; set; }

    public virtual TeachingAssistance Assistant { get; set; } = null!;

    public virtual Coursesemester CourseSemester { get; set; } = null!;
}
