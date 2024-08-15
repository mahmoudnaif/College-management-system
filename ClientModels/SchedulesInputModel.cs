namespace College_managemnt_system.ClientModels
{
    public class SchedulesInputModel
    {
        public int CourseSemesterId { get; set; }

        public int ClassroomId { get; set; }

        public string Type { get; set; } = null!;

        public int DayOfWeek { get; set; }

        public int PeriodNumber { get; set; }
    }


    public class EditScheduleTimeandPlace
    {
        public int ScheduleId { get; set; }
        public int DayOfWeek { get; set; }
        public int PeriodNumber { get; set; }
        public int ClassroomId { get; set; }
    }
}
