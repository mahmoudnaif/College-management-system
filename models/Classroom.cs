using System;
using System.Collections.Generic;

namespace College_managemnt_system.models;

public partial class Classroom
{
    public int ClassroomId { get; set; }

    public string? RoomNumber { get; set; }

    public string? Building { get; set; }

    public int? Capacity { get; set; }

    public virtual ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();
}
