using AutoMapper;
using College_managemnt_system.ClientModels;
using College_managemnt_system.CustomResponse;
using College_managemnt_system.DTOS;
using College_managemnt_system.Interfaces;
using College_managemnt_system.models;
using Microsoft.EntityFrameworkCore;

namespace College_managemnt_system.Repos
{
    public class PrereqsCoursesRepo : IPrereqsCoursesRepo
    {
        private readonly CollegeDBContext _context;
        private readonly IMapper _mapper;

        public PrereqsCoursesRepo(CollegeDBContext collegeDBContext, IMapper mapper)
        {
            _context = collegeDBContext;
            _mapper = mapper;
        }
        public async Task<CustomResponse<List<CourseDTO>>> GetPrereqsCourses(int courseId)
        { // you can make another DTO for the prereqs courses and make it less detailed for efficiency
            List<Course> prereqsCourses = await _context.Prereqs.Where(P => P.CourseId == courseId).Select(P => P.PrereqCourse).ToListAsync();
            if(!prereqsCourses.Any())
                return new CustomResponse<List<CourseDTO>>(404,"NOT FOUND");

            List<CourseDTO> prereqsCoursesDTO = _mapper.Map<List<CourseDTO>>(prereqsCourses);

            return new CustomResponse<List<CourseDTO>>(200, "Prereqs courses retreived", prereqsCoursesDTO);

        }
        public async Task<CustomResponse<PrereqDTO>> AddPrereqsCourse(int courseId, int prereqsCourseId)
        {
            Course courseExists = await _context.Courses.FirstOrDefaultAsync(C=> C.CourseId == courseId);

            if (courseExists == null)
                return new CustomResponse<PrereqDTO>(404, "Course does not exist");

            courseExists = await _context.Courses.FirstOrDefaultAsync(C => C.CourseId == prereqsCourseId);

            if (courseExists == null)
                return new CustomResponse<PrereqDTO>(404, "Preqes course does not exist");

            Prereq prereqExists = await _context.Prereqs.FirstOrDefaultAsync(P => P.CourseId == courseId && P.PrereqCourseId ==  prereqsCourseId);

            if(prereqExists != null)
                return new CustomResponse<PrereqDTO>(409, "Preqes already exists");

            Prereq prereq = new Prereq()
            {
                CourseId = courseId,
                PrereqCourseId = prereqsCourseId
            };

            try
            {
                _context.Add(prereq);
                await _context.SaveChangesAsync();
                PrereqDTO prereqDTO = _mapper.Map<PrereqDTO>(prereq);
                return new CustomResponse<PrereqDTO>(201, "Preqes course added successfully", prereqDTO);
            }
            catch
            {
                return new CustomResponse<PrereqDTO>(500, "Internal server error");
            }

        }
        public async Task<CustomResponse<bool>> RemovePrereqsCourse(int courseId, int prereqsCourseId)
        {
            Prereq prereq = await _context.Prereqs.FirstOrDefaultAsync(P => P.CourseId == courseId && P.PrereqCourseId ==  prereqsCourseId);
            if (prereq == null)
                return new CustomResponse<bool>(404, "Prereqs not found");

            try
            {
                _context.Remove(prereq);
                await _context.SaveChangesAsync();
                return new CustomResponse<bool>(200, "Prereqsuit Course deleted successfully");
            }
            catch
            {
                return new CustomResponse<bool>(500, "Internal server error");
            }

        }

        public async Task<CustomResponse<PrereqDTO>> AddPrereqsCourse(int courseId, string prereqsCourseCode)
        {
            Course courseExists = await _context.Courses.FirstOrDefaultAsync(C => C.CourseId == courseId);

            if (courseExists == null)
                return new CustomResponse<PrereqDTO>(404, "Course does not exist");

            Course PrereqcourseExists = await _context.Courses.FirstOrDefaultAsync(C => C.CourseCode == prereqsCourseCode);

            if (PrereqcourseExists == null)
                return new CustomResponse<PrereqDTO>(404, "Preqes course does not exist");

            Prereq prereqExists = await _context.Prereqs.FirstOrDefaultAsync(P => P.CourseId == courseId && P.PrereqCourseId == PrereqcourseExists.CourseId);

            if (prereqExists != null)
                return new CustomResponse<PrereqDTO>(409, "Preqes already exists");

            Prereq prereq = new Prereq()
            {
                CourseId = courseId,
                PrereqCourseId = PrereqcourseExists.CourseId
            };

            try
            {
                _context.Add(prereq);
                await _context.SaveChangesAsync();
                PrereqDTO prereqDTO = _mapper.Map<PrereqDTO>(prereq);
                return new CustomResponse<PrereqDTO>(201, "Preqes course added successfully", prereqDTO);
            }
            catch
            {
                return new CustomResponse<PrereqDTO>(500, "Internal server error");
            }
        }
    }
}
