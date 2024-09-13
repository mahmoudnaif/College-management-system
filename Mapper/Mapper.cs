using AutoMapper;
using College_managemnt_system.ClientModels;
using College_managemnt_system.DTOS;
using College_managemnt_system.models;
using Microsoft.Extensions.Hosting;
using Microsoft.VisualBasic;

namespace College_managemnt_system.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CoursesInputModelCSV, Course>();
            CreateMap<StudentsInputModelCSV, Student>();
            CreateMap<StudentsInputModelCSV, Account>();
            CreateMap<SchedulesJoinsgroup, ScheduleJoinsGroupDTO> ();
            CreateMap<Student, StudentDTO> ();
            CreateMap<Professor, ProfessorDTO> ();
            CreateMap<Coursesemester, CourseSemesterDTO > ();
            CreateMap<Schedule, SchedueleDTO>();
            CreateMap<Group, GroupDTO>();
            CreateMap<Classroom, ClassRoomDTO>();
            CreateMap<Prereq, PrereqDTO>();
            CreateMap<Semester, SemesterDTO>();
            CreateMap<Department, DepartmentDTO>();
            CreateMap<Course, CourseDTO>();
        }
    }
}
