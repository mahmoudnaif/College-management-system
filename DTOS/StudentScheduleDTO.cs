namespace College_managemnt_system.DTOS
{
    public class StudentScheduleDTO
    {
        public int scheduleId { get; set; }
        public int roomNumber { get; set; }
        public int semesterId { get; set; }
        public int courseId { get; set; }
        public string type { get; set; } = null!;
        public int dayOfWeek { get; set; }
        public int periodNumber { get; set; }
        public string courseName { get; set; } = null!;
        public int groupId { get; set; }
        public string groupName { get; set; } = null!;
    }
}
