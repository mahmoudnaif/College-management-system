using System;
using System.Collections.Generic;

namespace College_managemnt_system.models;

public partial class Schedule
{
    public int ScheduleId { get; set; }

    public int CourseSemesterId { get; set; }

    public int ClassroomId { get; set; }

    public string Type { get; set; } = null!;

    public string DayOfWeek { get; set; } = null!;

    public int PeriodNumber { get; set; }

    public DateTime? StartTime { get; set; }

    public DateTime? EndTime { get; set; }

    public virtual Classroom Classroom { get; set; } = null!;

    public virtual Coursesemester CourseSemester { get; set; } = null!;

    public virtual ICollection<SchedulesJoinsgroup> SchedulesJoinsgroups { get; set; } = new List<SchedulesJoinsgroup>();
}
