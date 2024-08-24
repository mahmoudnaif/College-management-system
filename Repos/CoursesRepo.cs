using AutoMapper;
using College_managemnt_system.ClientModels;
using College_managemnt_system.CustomResponse;
using College_managemnt_system.DTOS;
using College_managemnt_system.Interfaces;
using College_managemnt_system.models;
using System.Linq.Expressions;

namespace College_managemnt_system.Repos
{
    public class CoursesRepo : ICoursesRepo
    {
        private readonly CollegeDBContext _context;
        private readonly IMapper _mapper;

        public CoursesRepo(CollegeDBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<CustomResponse<IEnumerable<CourseDTO>>> GetCourses(TakeSkipModel takeSkipModel)
        {
            if (takeSkipModel.take < 0 ||  takeSkipModel.skip < 0)
                return new CustomResponse<IEnumerable<CourseDTO>>(400, "Take and skip must more than or equal 0");


            IEnumerable<Course> courses = _context.Courses.Skip(takeSkipModel.skip)
                                                          .Take(takeSkipModel.take);

            if (courses.Count() == 0)
                return new CustomResponse<IEnumerable<CourseDTO>>(404, "Not found");

            IEnumerable<CourseDTO> coursesDTO = _mapper.Map<IEnumerable<CourseDTO>>(courses);

            return new CustomResponse<IEnumerable<CourseDTO>>(200,"Courses Retreived", coursesDTO);

        }
        public async Task<CustomResponse<CourseDTO>> GetCourseByCourseCode(string courseCode)
        {
            Course course = _context.Courses.FirstOrDefault(c => c.CourseCode == courseCode);

            if (course == null)
                return new CustomResponse<CourseDTO>(404, "Course Not found");

            CourseDTO courseDTO = _mapper.Map<CourseDTO>(course);

            return new CustomResponse<CourseDTO>(200, "Course retreived", courseDTO);
        }
        public async Task<CustomResponse<CourseDTO>> AddCourse(CourseInputModel courseInputModel)
        {
            if (courseInputModel.Credits < 0)
                return new CustomResponse<CourseDTO>(400, "Credit hour must be greater than or equal to 0");

            if (courseInputModel.CourseName.Trim() == "" || courseInputModel.CourseCode.Trim() == "")
                return new CustomResponse<CourseDTO>(400, "Course name and code must be specified");


            Course courseDuplication = _context.Courses.FirstOrDefault(C => C.CourseCode == courseInputModel.CourseCode);

            if (courseDuplication != null)
                return new CustomResponse<CourseDTO>(409, "Course Code already exists");

            Department departmentExists = _context.Departments.FirstOrDefault(D => D.DepartmentId == courseInputModel.DepartmentId);

            if(departmentExists == null)
                return new CustomResponse<CourseDTO>(404,"Department does not exist");


            Course course = new Course() { CourseName = courseInputModel.CourseName, CourseCode = courseInputModel.CourseCode 
                                          ,Credits = courseInputModel.Credits, DepartmentId = courseInputModel.DepartmentId };
            try
            {
                _context.Courses.Add(course);
                await _context.SaveChangesAsync();
                CourseDTO courseDTO = _mapper.Map<CourseDTO>(course);
                return new CustomResponse<CourseDTO>(201, "Course added successfully",courseDTO);
            }
            catch
            {
                return new CustomResponse<CourseDTO>(500, "Internal server error");
            }
        }

        public async Task<CustomResponse<bool>> DeleteCourse(int courseId)
        {
            Course course = _context.Courses.FirstOrDefault(c => c.CourseId == courseId);

            if (course == null)
                return new CustomResponse<bool>(404, "Course Not found");

            try
            {
                _context.Courses.Remove(course);
                await _context.SaveChangesAsync();
                return new CustomResponse<bool>(200, "Course Deleted successfully");
            }
            catch
            {
                return new CustomResponse<bool>(500, "Internal server error");
            }
        }
        public async Task<CustomResponse<CourseDTO>> EditCourseName(int courseId,string courseName)
        {
            if (courseName.Trim() == "")
                return new CustomResponse<CourseDTO>(400, "Course name must be specified");

            Course course = _context.Courses.FirstOrDefault(c => c.CourseId == courseId);

            if (course == null)
                return new CustomResponse<CourseDTO>(404, "Course Not found");

            if(course.CourseName == courseName)
                return new CustomResponse<CourseDTO>(409,"Name is already: "+courseName);

            course.CourseName = courseName;

            try
            {
                await _context.SaveChangesAsync();
                CourseDTO courseDTO = _mapper.Map<CourseDTO>(course);
                return new CustomResponse<CourseDTO>(200, "Course name edited successfully", courseDTO);
            }
            catch
            {
                return new CustomResponse<CourseDTO>(500, "Internal server error");
            }

        }
        public async Task<CustomResponse<CourseDTO>> EditCourseCode(int courseId, string courseCode)
        {
            if (courseCode.Trim() == "")
                return new CustomResponse<CourseDTO>(400, "Course code must be specified");

            Course courseDuplication = _context.Courses.FirstOrDefault(C => C.CourseCode == courseCode);

            if (courseDuplication != null)
                return new CustomResponse<CourseDTO>(409, "Course Code already exists");

            Course course = _context.Courses.FirstOrDefault(c => c.CourseId == courseId);

            if (course == null)
                return new CustomResponse<CourseDTO>(404, "Course Not found");

            course.CourseCode = courseCode;

            try
            {
                await _context.SaveChangesAsync();
                CourseDTO courseDTO = _mapper.Map<CourseDTO>(course);
                return new CustomResponse<CourseDTO>(200, "Course code edited successfully", courseDTO);
            }
            catch
            {
                return new CustomResponse<CourseDTO>(500, "Internal server error");
            }

        }
        public async Task<CustomResponse<CourseDTO>> EditCreditHours(int courseId, int credits)
        {
            if(credits < 0)
                return new CustomResponse<CourseDTO>(400, "Credit hour must be greater than or equal to 0");

            Course course = _context.Courses.FirstOrDefault(c => c.CourseId == courseId);

            if (course == null)
                return new CustomResponse<CourseDTO>(404, "Course Not found");

            if (course.Credits == credits)
                return new CustomResponse<CourseDTO>(409, "Credit hours is already: " + credits);

            course.Credits = credits;

            try
            {
                await _context.SaveChangesAsync();
                CourseDTO courseDTO = _mapper.Map<CourseDTO>(course);
                return new CustomResponse<CourseDTO>(200, "Credit hours edited successfully", courseDTO);
            }
            catch
            {
                return new CustomResponse<CourseDTO>(500, "Internal server error");
            }

        }
        public async Task<CustomResponse<CourseDTO>> EditDepartment(int courseId, int deprtmentID)
        {
            Department departmentExists = _context.Departments.FirstOrDefault(D => D.DepartmentId == deprtmentID);

            if (departmentExists == null)
                return new CustomResponse<CourseDTO>(404, "Department does not exist");


            Course course = _context.Courses.FirstOrDefault(c => c.CourseId == courseId);

            if (course == null)
                return new CustomResponse<CourseDTO>(404, "Course Not found");

            if (course.DepartmentId == deprtmentID)
                return new CustomResponse<CourseDTO>(409, "Department is already: " + departmentExists.DepartmentName);

            course.DepartmentId = deprtmentID;

            try
            {
                await _context.SaveChangesAsync();
                CourseDTO courseDTO = _mapper.Map<CourseDTO>(course);
                return new CustomResponse<CourseDTO>(200, "Department changed successfully", courseDTO);
            }
            catch
            {
                return new CustomResponse<CourseDTO>(500, "Internal server error");
            }

        }
    }
}
