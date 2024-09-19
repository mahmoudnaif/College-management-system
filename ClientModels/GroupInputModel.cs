﻿namespace College_managemnt_system.ClientModels
{
    public class GroupInputModel
    {
        public string GroupName { get; set; } = null!;

        public int StudentsYear { get; set; }

        public int SemesterId { get; set; }
    }


    public class CustomGroupCourseInputModel
    {
        public int GroupId { get; set; }
        public int CourseId { get; set; }
    }
}
