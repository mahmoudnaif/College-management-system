using AutoMapper;
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
            CreateMap<Prereq, PrereqDTO>();
            CreateMap<Semester, SemesterDTO>();
            CreateMap<Department, DepartmentDTO>();
            CreateMap<Course, CourseDTO>();
        }
    }
}
