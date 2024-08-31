using System;
using System.Collections.Generic;

namespace College_managemnt_system.models;

public partial class Classroom
{
    public int RoomNumber { get; set; }

    public int Building { get; set; }

    public int Capacity { get; set; }

    public virtual ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();
}
