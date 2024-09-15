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
        public async Task<CustomResponse<AssistantsCoursesDTO>> AddTaToCourse(int courseSemesterId, int taId)
        {
            Coursesemester coursesemester = await _context.Coursesemesters.FirstOrDefaultAsync(CS => CS.CourseSemesterId == courseSemesterId);
            if (coursesemester == null)
                return new CustomResponse<AssistantsCoursesDTO>(404,"Course not found");

            TeachingAssistance teachingAssistance = await _context.TeachingAssistances.FirstOrDefaultAsync(T => T.AssistantId == taId);

            if (teachingAssistance == null)
                return new CustomResponse<AssistantsCoursesDTO>(404, "Teaching assistance not found");

            AssistantsJoinscourseSemester taCourse = new AssistantsJoinscourseSemester()
            {
                AssistantId  = taId,
                CourseSemesterId = courseSemesterId
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
        public async Task<CustomResponse<bool>> RemoveTaFromCourse(int courseSemesterId, int taId)
        {
            AssistantsJoinscourseSemester taCourse = await _context.AssistantsJoinscourseSemesters.FirstOrDefaultAsync(TC => TC.CourseSemesterId == courseSemesterId && TC.AssistantId == taId);

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

        public async Task<CustomResponse<List<TeachingAssistanceDTO>>> GetCourseTas(int courseSemesterId) //TODO. should be filtered by semesterId too (will do after redesigning the coursesemester table)
        {
            List<TeachingAssistance> teachingAssistance = await _context.AssistantsJoinscourseSemesters.Where(TC => TC.CourseSemesterId == courseSemesterId).Join(_context.TeachingAssistances,TC => TC.AssistantId,T => T.AssistantId,(TC,T) => T).ToListAsync();

            if (!teachingAssistance.Any())
                return new CustomResponse<List<TeachingAssistanceDTO>>(404,"No tas found for this course");

            List<TeachingAssistanceDTO> teachingAssistancesDTO = _mapper.Map<List<TeachingAssistanceDTO>>(teachingAssistance);

            return new CustomResponse<List<TeachingAssistanceDTO>>(200,"Tas retrieved successfully",teachingAssistancesDTO);
        }

        public async Task<CustomResponse<List<CourseSemesterDTO>>> getTaCourses(int taId) //TODO. should be filtered by semesterId too (will do after redesigning the coursesemester table)
        {
            List<Coursesemester> courses = await _context.AssistantsJoinscourseSemesters.Where(TC => TC.AssistantId == taId).Join(_context.Coursesemesters, TC => TC.CourseSemesterId, CS => CS.CourseSemesterId, (TC, CS) => CS).ToListAsync();

            if (!courses.Any())
                return new CustomResponse<List<CourseSemesterDTO>>(404, "No courses found for this course");

            List<CourseSemesterDTO> coursesDTO = _mapper.Map<List<CourseSemesterDTO>>(courses);

            return new CustomResponse<List<CourseSemesterDTO>>(200, "Tas retrieved successfully", coursesDTO);
        }
    }
}
