﻿using System;
using System.Collections.Generic;

namespace College_managemnt_system.models;

public partial class Schedule
{
    public int ScheduleId { get; set; }

    public int CourseSemesterId { get; set; }

    public int ClassroomId { get; set; }

    public int SemesterId { get; set; }

    public string Type { get; set; } = null!;

    public int DayOfWeek { get; set; }

    public int PeriodNumber { get; set; }

    public virtual Classroom Classroom { get; set; } = null!;

    public virtual Coursesemester CourseSemester { get; set; } = null!;

    public virtual ICollection<SchedulesJoinsgroup> SchedulesJoinsgroups { get; set; } = new List<SchedulesJoinsgroup>();

    public virtual Semester Semester { get; set; } = null!;
}
