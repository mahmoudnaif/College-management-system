using AutoMapper;
using College_managemnt_system.ClientModels;
using College_managemnt_system.CustomResponse;
using College_managemnt_system.DTOS;
using College_managemnt_system.Interfaces;
using College_managemnt_system.models;

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
        public async Task<CustomResponse<IEnumerable<CourseDTO>>> GetPrereqsCourses(int courseId)
        { // you can make another DTO for the prereqs courses and make it less detailed for efficiency
            IEnumerable<Course> prereqsCourses = _context.Prereqs.Where(P => P.CourseId == courseId).Select(P => P.PrereqCourse);
            if(prereqsCourses.Count() == 0)
                return new CustomResponse<IEnumerable<CourseDTO>>(404,"NOT FOUND");

            IEnumerable<CourseDTO> prereqsCoursesDTO = _mapper.Map<IEnumerable<CourseDTO>>(prereqsCourses);

            return new CustomResponse<IEnumerable<CourseDTO>>(200, "Prereqs courses retreived", prereqsCoursesDTO);

        }
        public async Task<CustomResponse<PrereqDTO>> AddPrereqsCourse(PrereqsInputModel prereqsInputModel)
        {
            Course courseExists = _context.Courses.SingleOrDefault(C=> C.CourseId == prereqsInputModel.CourseId);

            if (courseExists == null)
                return new CustomResponse<PrereqDTO>(404, "Course does not exist");

            courseExists = _context.Courses.SingleOrDefault(C => C.CourseId == prereqsInputModel.PrereqsCourseId);

            if (courseExists == null)
                return new CustomResponse<PrereqDTO>(404, "Preqes course does not exist");

            Prereq prereqExists = _context.Prereqs.SingleOrDefault(P => P.CourseId == prereqsInputModel.CourseId && P.PrereqCourseId ==  prereqsInputModel.PrereqsCourseId);

            if(prereqExists != null)
                return new CustomResponse<PrereqDTO>(409, "Preqes already exists");

            Prereq prereq = new Prereq()
            {
                CourseId = prereqsInputModel.CourseId,
                PrereqCourseId = prereqsInputModel.PrereqsCourseId
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
        public async Task<CustomResponse<bool>> RemovePrereqsCourse(PrereqsInputModel prereqsInputModel)
        {
            Prereq prereq = _context.Prereqs.SingleOrDefault(P => P.CourseId == prereqsInputModel.CourseId && P.PrereqCourseId ==  prereqsInputModel.PrereqsCourseId);
            if (prereq == null)
                return new CustomResponse<bool>(404, "Prereqs not found");

            try
            {
                _context.Remove(prereq);
                await _context.SaveChangesAsync();
                return new CustomResponse<bool>(200, "Course deleted successfully");
            }
            catch
            {
                return new CustomResponse<bool>(500, "Internal server error");
            }

        }
    }
}
