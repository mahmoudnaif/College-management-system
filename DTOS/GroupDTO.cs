using College_managemnt_system.models;

namespace College_managemnt_system.DTOS
{
    public class GroupDTO
    {
        public int GroupId { get; set; }

        public string GroupName { get; set; } = null!;

        public int StudentsYear { get; set; }

        public int SemesterId { get; set; }
    }

    public class GroupCourseDTO
    {
        public int GroupId { get; set; }

        public string groupName { get; set; } = null!;
        public List<int> CourseIds { get; set; } = [];

        public List<SchedueleDTO> schedules { get; set; } = [];
    }

    public class GroupScheduleDTO
    {
        public int GroupId { get; set; }

        public List<SchedueleDTO> scheduelesDTO { get; set; } = [];
    }

    public class CourseScheduleDTO
    {
        public int courseId { get; set; }
        public List<SchedueleDTO> scheduelesDTO { get; set; } = [];
    }


    public class GroupCustomScheduleDTO
    {
        public int GroupId { get; set; }
        public List<CourseScheduleDTO> courseSchedulesDTO { get; set; } = [];
    }
}
