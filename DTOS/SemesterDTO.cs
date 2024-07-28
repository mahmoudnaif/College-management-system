namespace College_managemnt_system.DTOS
{
    public class SemesterDTO
    {
        public int SemesterId { get; set; }
        public string SemesterName { get; set; }
        public int SemesterYear { get; set; }
        public bool isAtctive { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
