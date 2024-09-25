using College_managemnt_system.ClientModels;
using College_managemnt_system.CustomResponse;
using College_managemnt_system.DTOS;
using College_managemnt_system.Interfaces;
using College_managemnt_system.models;
using Microsoft.EntityFrameworkCore;

namespace College_managemnt_system.Repos
{
    public class StudentCoursesRepo : IStudentCoursesRepo
    {
        private readonly CollegeDBContext _context;

        public StudentCoursesRepo(CollegeDBContext context)
        {
            _context = context;
        }
        public async Task<CustomResponse<List<StudentCoursesDTO>>> GetActiveStudentCourses(int studentId)
        {
            Semester semester = await _context.Semesters.FirstOrDefaultAsync(S => S.IsActive);

            if (semester == null)
                return new CustomResponse<List<StudentCoursesDTO>>(400, "No active semester at the moment");

            var activeCourses = await (from SC in _context.StudentCourses
                                       where SC.StudentId == studentId && SC.Status == "InProgress"
                                       join CS in _context.Coursesemesters on new { SC.CourseId, SC.SemesterId } equals new { CS.CourseId, CS.SemesterId }
                                       join C in _context.Courses on CS.CourseId equals C.CourseId
                                       join P in _context.Professors on CS.ProfessorId equals P.ProfessorId
                                       select new StudentCoursesDTO
                                       {
                                          courseId = SC.CourseId,
                                          status = SC.Status,
                                          courseName = C.CourseName,
                                          professorName = P.FirstName +" " +P.LastName,
                                          courseCode = C.CourseCode,
                                          credits = C.Credits,
                                          grade = SC.Grade,
                                       }
                                       ).ToListAsync();

            if (!activeCourses.Any())
                return new CustomResponse<List<StudentCoursesDTO>>(404, "No registered courses at the moment");

            return new CustomResponse<List<StudentCoursesDTO>>(200, "courses retrieved successfully", activeCourses);
        }

        public async Task<CustomResponse<List<StudentCoursesDTO>>> GetAllFinishedStudentCourses(int studentId, TakeSkipModel takeSkipModel)
        {

            if (takeSkipModel.take <= 0 || takeSkipModel.skip < 0)
                return new CustomResponse<List<StudentCoursesDTO>>(400, "Take must be more than 0 and skip must be more than or equal 0");


            var studentCoursesQuery = _context.StudentCourses
                                   .Where(SC => SC.StudentId == studentId && SC.Status != "InProgress")
                                   .Skip(takeSkipModel.skip)
                                   .Take(takeSkipModel.take);


            var courses = await(from SC in studentCoursesQuery
                                      join CS in _context.Coursesemesters on new { SC.CourseId, SC.SemesterId } equals new { CS.CourseId, CS.SemesterId }
                                      join C in _context.Courses on CS.CourseId equals C.CourseId
                                      join P in _context.Professors on CS.ProfessorId equals P.ProfessorId
                                      orderby SC.SemesterId
                                      select new StudentCoursesDTO //semester id can be added too
                                      {
                                          courseId = SC.CourseId,
                                          status = SC.Status,
                                          courseName = C.CourseName,
                                          professorName = P.FirstName + " " + P.LastName,
                                          courseCode = C.CourseCode,
                                          credits = C.Credits,
                                          grade = SC.Grade,
                                      }
                                       ).ToListAsync();

            if (!courses.Any())
                return new CustomResponse<List<StudentCoursesDTO>>(404, "No finished courses at the moment");

            return new CustomResponse<List<StudentCoursesDTO>>(200, "courses retrieved successfully", courses);
        }
        public async Task<CustomResponse<bool>> DropCourse(int studentId, int courseId)
        {

            Semester semester = await _context.Semesters.FirstOrDefaultAsync(S => S.IsActive);
            if (semester == null)
                return new CustomResponse<bool>(404, "No active semester at the moment");

            StudentCourse studentCourse = await _context.StudentCourses.FirstOrDefaultAsync(SC => SC.StudentId == studentId && SC.CourseId == courseId && SC.SemesterId == semester.SemesterId);

            if (studentCourse == null)
                return new CustomResponse<bool>(404, "Course is not registered for this student");

            if (studentCourse.Status != "InProgress")
                return new CustomResponse<bool>(403, $"Course is set to: {studentCourse.Status} you can't edit the status");

            studentCourse.Grade = "DR";
            studentCourse.Status = "Dropped";

            try
            {
                await _context.SaveChangesAsync();
                return new CustomResponse<bool>(200, "Course dropped successfully");
            }
            catch
            {
                return new CustomResponse<bool>(500, "Internal server error");
            }
        }
        public async Task<CustomResponse<bool>> WithDrawlFromCourse(int studentId, int courseId, bool hasReason)
        {

            Semester semester = await _context.Semesters.FirstOrDefaultAsync(S => S.IsActive);
            if (semester == null)
                return new CustomResponse<bool>(404, "No active semester at the moment");

            StudentCourse studentCourse = await _context.StudentCourses.FirstOrDefaultAsync(SC => SC.StudentId == studentId && SC.CourseId == courseId && SC.SemesterId == semester.SemesterId);

            if (studentCourse == null)
                return new CustomResponse<bool>(404, "Course is not registered for this student");

            if (studentCourse.Status != "Inprogress")
                return new CustomResponse<bool>(403, $"Course is set to: {studentCourse.Status} you can't edit the status");

            studentCourse.Grade = hasReason ? "W" : "F";
            studentCourse.Status = "Withdrawn";

            try
            {
                await _context.SaveChangesAsync();
                return new CustomResponse<bool>(200, "Course withdrawed successfully");
            }
            catch
            {
                return new CustomResponse<bool>(500, "Internal server error");
            }
        }
        public async Task<CustomResponse<bool>> SetCourseIncomplete(int studentId, int courseId)
        {
            Semester semester = await _context.Semesters.FirstOrDefaultAsync(S => S.IsActive);
            if (semester == null)
                return new CustomResponse<bool>(404, "No active semester at the moment");

            StudentCourse studentCourse = await _context.StudentCourses.FirstOrDefaultAsync(SC => SC.StudentId == studentId && SC.CourseId == courseId && SC.SemesterId == semester.SemesterId);

            if (studentCourse == null)
                return new CustomResponse<bool>(404, "Course is not registered for this student");

            if (studentCourse.Status != "InProgress")
                return new CustomResponse<bool>(403, $"Course is set to: {studentCourse.Status} you can't edit the status");

            studentCourse.Status = "InComplete";

            try
            {
                await _context.SaveChangesAsync();
                return new CustomResponse<bool>(200, "Course set to incomplete successfully");
            }
            catch
            {
                return new CustomResponse<bool>(500, "Internal server error");
            }
        }
        public async Task<CustomResponse<bool>> SetGrade(int studentId, int courseId, string grade)
        {
            var validGrades = new List<string> { "A", "A-", "B+", "B", "B-", "C+", "C", "C-", "D+", "D", "D-", "F" };

            if (!validGrades.Contains(grade))
                return new CustomResponse<bool>(400, "Invalid grade");

            Semester semester = await _context.Semesters.FirstOrDefaultAsync(S => S.IsActive);
            if (semester == null)
                return new CustomResponse<bool>(404, "No active semester at the moment");

            StudentCourse studentCourse = await _context.StudentCourses.FirstOrDefaultAsync(SC => SC.StudentId == studentId && SC.CourseId == courseId && SC.SemesterId == semester.SemesterId);

            if (studentCourse == null)
                return new CustomResponse<bool>(404, "Course is not registered for this student");

            if (studentCourse.Status != "InProgress" && studentCourse.Status != "InComplete")
                return new CustomResponse<bool>(403, $"Course is set to: {studentCourse.Status} you can't set grade for this status");

            studentCourse.Grade = grade;
            studentCourse.Status = grade != "F" ? "Completed" : "Failed";


            try
            {
                await _context.SaveChangesAsync();
                return new CustomResponse<bool>(200, "Grade set successfully");
            }
            catch
            {
                return new CustomResponse<bool>(500, "Internal server error");
            }
        }

        public async Task<CustomResponse<bool>> AppealGrade(int studentId, int courseId, string grade)
        {
            var validGrades = new List<string> { "A", "A-", "B+", "B", "B-", "C+", "C", "C-", "D+", "D", "D-", "F" };

            if (!validGrades.Contains(grade))
                return new CustomResponse<bool>(400, "Invalid grade");

            Semester semester = await _context.Semesters.FirstOrDefaultAsync(S => S.IsActive);
            if (semester == null)
                return new CustomResponse<bool>(404, "No active semester at the moment");

            StudentCourse studentCourse = await _context.StudentCourses.FirstOrDefaultAsync(SC => SC.StudentId == studentId && SC.CourseId == courseId && SC.SemesterId == semester.SemesterId);

            if (studentCourse == null)
                return new CustomResponse<bool>(404, "Course is not registered for this student");

            if (studentCourse.Status != "Completed" && studentCourse.Status != "Failed")
                return new CustomResponse<bool>(403, $"Course is set to: {studentCourse.Status} you can't set grade for this status");

            if (studentCourse.Grade == grade)
                return new CustomResponse<bool>(409, $"Grade is already set to: {grade}");

            studentCourse.Grade = grade;
            studentCourse.Status = "Completed";

            try
            {
                await _context.SaveChangesAsync();
                return new CustomResponse<bool>(200, "Grade edited successfully");
            }
            catch
            {
                return new CustomResponse<bool>(500, "Internal server error");
            }
        }
    }
}
