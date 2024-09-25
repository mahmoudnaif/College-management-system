using College_managemnt_system.CustomResponse;
using College_managemnt_system.DTOS;

namespace College_managemnt_system.Interfaces
{
    public interface IStudentSchedulesRepo
    {
        public Task<CustomResponse<List<StudentScheduleDTO>>> GetStudentActiveSchedule(int studentId);

        public Task<CustomResponse<List<StudentSheetByGroup>>> GetStudentsSheet(int scheduleId);

    }
}
