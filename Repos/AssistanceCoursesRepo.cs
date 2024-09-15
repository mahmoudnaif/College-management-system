using AutoMapper;
using College_managemnt_system.CustomResponse;
using College_managemnt_system.DTOS;
using College_managemnt_system.Interfaces;
using College_managemnt_system.models;
using Microsoft.EntityFrameworkCore;

namespace College_managemnt_system.Repos
{
    public class AssistanceCoursesRepo : IAssistanceCoursesRepo
    {
        private readonly CollegeDBContext _context;
        private readonly IMapper _mapper;

        public AssistanceCoursesRepo(CollegeDBContext context,IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<CustomResponse<AssistantsCoursesDTO>> AddTaToCourse(int courseId, int semesterId, int taId)
        {
            Coursesemester coursesemester = await _context.Coursesemesters.FirstOrDefaultAsync(CS => CS.CourseId == courseId && CS.SemesterId == semesterId);
            if (coursesemester == null)
                return new CustomResponse<AssistantsCoursesDTO>(404,"Course not found");

            TeachingAssistance teachingAssistance = await _context.TeachingAssistances.FirstOrDefaultAsync(T => T.AssistantId == taId);

            if (teachingAssistance == null)
                return new CustomResponse<AssistantsCoursesDTO>(404, "Teaching assistance not found");

            AssistantsJoinscourseSemester taCourse = new AssistantsJoinscourseSemester()
            {
                AssistantId  = taId,
                CourseId = courseId,
                SemesterId = semesterId
            };

            try
            {
                await _context.AddAsync(taCourse);
                await _context.SaveChangesAsync();
                AssistantsCoursesDTO taCourseDTO = _mapper.Map<AssistantsCoursesDTO>(taCourse);
                return new CustomResponse<AssistantsCoursesDTO>(201, "Ta added to course successfully", taCourseDTO);
            }
            catch
            {
                return new CustomResponse<AssistantsCoursesDTO>(500, "Internal server error");
            }
          }
        public async Task<CustomResponse<bool>> RemoveTaFromCourse(int courseId, int semesterId, int taId)
        {
            AssistantsJoinscourseSemester taCourse = await _context.AssistantsJoinscourseSemesters.FirstOrDefaultAsync(TC => TC.CourseId == courseId && TC.SemesterId == semesterId && TC.AssistantId == taId);

            if (taCourse == null)
                return new CustomResponse<bool>(404, "Ta does not exist in this course");

            try
            {
                _context.AssistantsJoinscourseSemesters.Remove(taCourse);
                await _context.SaveChangesAsync();
                return new CustomResponse<bool>(200, "Ta removed fromt this course");
            }
            catch
            {
                return new CustomResponse<bool>(500,"Internal server error");
            }
        }

        public async Task<CustomResponse<List<TeachingAssistanceDTO>>> GetCourseTas(int courseId, int semesterId) 
        {
            List<TeachingAssistance> teachingAssistance = await _context.AssistantsJoinscourseSemesters.Where(TC => TC.CourseId == courseId && TC.SemesterId == semesterId).Join(_context.TeachingAssistances,TC => TC.AssistantId,T => T.AssistantId,(TC,T) => T).ToListAsync();

            if (!teachingAssistance.Any())
                return new CustomResponse<List<TeachingAssistanceDTO>>(404,"No tas found for this course");

            List<TeachingAssistanceDTO> teachingAssistancesDTO = _mapper.Map<List<TeachingAssistanceDTO>>(teachingAssistance);

            return new CustomResponse<List<TeachingAssistanceDTO>>(200,"Tas retrieved successfully",teachingAssistancesDTO);
        }

        public async Task<CustomResponse<List<CourseSemesterDTO>>> getTaCourses(int semesterId,int taId) 
        {
            List<CourseSemesterDTO> courses = await (from TC in _context.AssistantsJoinscourseSemesters
                                                     where TC.SemesterId == semesterId && TC.AssistantId == taId
                                                     join CS in _context.Coursesemesters on new { TC.CourseId, TC.SemesterId } equals new { CS.CourseId, CS.SemesterId }
                                                     join C in _context.Courses on CS.CourseId equals C.CourseId
                                                     select new CourseSemesterDTO
                                                     {
                                                         CourseId = CS.CourseId,
                                                         SemesterId = CS.SemesterId,
                                                         CourseName = C.CourseName,
                                                         ProfessorId = CS.ProfessorId,
                                                        Isactive = CS.Isactive

                                                     }).ToListAsync();


            if (!courses.Any())
                return new CustomResponse<List<CourseSemesterDTO>>(404, "No courses found for this course");

            return new CustomResponse<List<CourseSemesterDTO>>(200, "Tas retrieved successfully", courses);
        }
    }
}
