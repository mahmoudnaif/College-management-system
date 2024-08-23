namespace College_managemnt_system.ClientModels
{
    public class SemesterInputModel
    {

        public string semesterName { get; set; }

        public int semesterYear { get; set; }

        public DateTime startDate { get; set; }

        public DateTime endDate { get; set; }
    }

    public class GetSemesterModel
    {

        public string semesterName { get; set; }

        public int SemesterYear { get; set; }

    }

    public class EditDateModel // duplicate can be deleted later
    {

        public int semesterId { get; set; }

        public DateTime newDate { get; set; }

    }

    public class EditIsActiveModel
    {

        public int semesterId { get; set; }

        public bool isActive{ get; set; }

    }





}
