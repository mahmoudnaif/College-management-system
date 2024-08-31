namespace College_managemnt_system.ClientModels
{
    public class SchedulesInputModel
    {
        public int CourseSemesterId { get; set; }
        public int RoomNumber { get; set; }
        public int SemesterId { get; set; }
        public string Type { get; set; } = null!;
        public int DayOfWeek { get; set; }
        public int PeriodNumber { get; set; }
    }


    public class EditScheduleTimeandPlace
    {
        public int ScheduleId { get; set; }
        public int DayOfWeek { get; set; }
        public int PeriodNumber { get; set; }
        public int RoomNumber { get; set; }
    }

    public class GetSchduelsBySemester
    {
        public int SemesterId { get; set; }

        public int take { get; set; } = 10;
        public int skip { get; set; } = 0;

    }

}
