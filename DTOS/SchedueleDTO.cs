namespace College_managemnt_system.DTOS
{
    public class SchedueleDTO
    {
        public int ScheduleId { get; set; }

        public int CourseSemesterId { get; set; }

        public int ClassroomId { get; set; }

        public int SemesterId { get; set; }

        public string Type { get; set; } = null!;

        public int DayOfWeek { get; set; }

        public int PeriodNumber { get; set; }

        public string courseName { get; set; }

        public string roomNumber { get; set; } // TODO. edit the db scheme so that the classroom number is the primary key from now on (int).
    }
}
