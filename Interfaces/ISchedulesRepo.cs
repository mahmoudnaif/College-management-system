using College_managemnt_system.ClientModels;
using College_managemnt_system.CustomResponse;
using College_managemnt_system.DTOS;

namespace College_managemnt_system.Interfaces
{
    public interface ISchedulesRepo
    {
        public Task<CustomResponse<List<SchedueleDTO>>> GetScheduleBySemester(int semesterId,TakeSkipModel takeSkipModel);
        public Task<CustomResponse<SchedueleDTO>> Add(SchedulesInputModel schdulesInputModel);
        public Task<CustomResponse<bool>> Remove(int scheduleId);
        public Task<CustomResponse<SchedueleDTO>> EditTimeAndPlace(EditScheduleTimeandPlace editScheduleTimeandPlace);
    }
}
