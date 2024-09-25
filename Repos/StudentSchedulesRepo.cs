using College_managemnt_system.CustomResponse;
using College_managemnt_system.DTOS;
using College_managemnt_system.Interfaces;
using College_managemnt_system.models;
using Microsoft.EntityFrameworkCore;

namespace College_managemnt_system.Repos
{
    public class StudentSchedulesRepo : IStudentSchedulesRepo
    {
        private readonly CollegeDBContext _context;

        public StudentSchedulesRepo(CollegeDBContext context)
        {
            _context = context;
        }
        public async Task<CustomResponse<List<StudentScheduleDTO>>> GetStudentActiveSchedule(int studentId)
        {

            if (!await _context.Students.AnyAsync(S => S.StudentId == studentId))
                return new CustomResponse<List<StudentScheduleDTO>>(404, "Student does not exist");

            int? activeSemesterId = (await _context.Semesters.FirstOrDefaultAsync(S => S.IsActive))?.SemesterId;

            if (activeSemesterId == null)
                return new CustomResponse<List<StudentScheduleDTO>>(400, "No active semester at the moment");

            var schedules = await (from SJG in _context.StudentsJoinsgroups
                                   join G in _context.Groups on SJG.GroupId equals G.GroupId
                                   where G.SemesterId == (int)activeSemesterId
                                   join S in _context.Schedules on SJG.ScheduleId equals S.ScheduleId
                                   join C in _context.Courses on S.CourseId equals C.CourseId
                                   select new StudentScheduleDTO
                                   {
                                       scheduleId = S.ScheduleId,
                                       semesterId = S.SemesterId,
                                       courseId = S.CourseId,
                                       courseName = C.CourseName,
                                       dayOfWeek = S.DayOfWeek,
                                       periodNumber = S.PeriodNumber,
                                       roomNumber = S.RoomNumber,
                                       type = S.Type,
                                       groupId = G.GroupId,
                                       groupName = G.GroupName
                                   }
                                   ).ToListAsync();

            if (!schedules.Any())
                return new CustomResponse<List<StudentScheduleDTO>>(404, "No schedules were found");

            return new CustomResponse<List<StudentScheduleDTO>>(200, "schedules retrieved successfully", schedules);
        }

        public async Task<CustomResponse<List<StudentSheetByGroup>>> GetStudentsSheet(int scheduleId)
        {
            var studentsSheet = await (from SJG in _context.StudentsJoinsgroups
                                       where SJG.ScheduleId == scheduleId
                                       join G in _context.Groups on SJG.GroupId equals G.GroupId
                                       join S in _context.Students on SJG.StudentId equals S.StudentId
                                       group new { studentSheet = new StudentSheet(S.StudentId, S.FirstName, S.FathertName, S.GrandfatherName, S.LastName) } by G into scheduleGroup
                                       select new StudentSheetByGroup
                                       {
                                           groupId = scheduleGroup.Key.GroupId,
                                           groupName = scheduleGroup.Key.GroupName,
                                            studentsSheet = scheduleGroup.Select(S => S.studentSheet).ToList()
                                       }
                                       ).ToListAsync();

            if (!studentsSheet.Any())
                return new CustomResponse<List<StudentSheetByGroup>>(404, "No students were found");

            return new CustomResponse<List<StudentSheetByGroup>>(200, "Students sheet retrieved successfully", studentsSheet);
        }
    }
}
