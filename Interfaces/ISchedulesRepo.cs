using College_managemnt_system.ClientModels;
using College_managemnt_system.CustomResponse;
using College_managemnt_system.DTOS;
using Microsoft.AspNetCore.Mvc;

namespace College_managemnt_system.Interfaces
{
    public interface ISchedulesRepo
    {
        public Task<CustomResponse<IEnumerable<SchedueleDTO>>> GetScheduls(int semesterId,TakeSkipModel takeSkipModel);
        public Task<CustomResponse<SchedueleDTO>> Add(SchedulesInputModel schdulesInputModel);
        public Task<CustomResponse<bool>> Remove(int scheduleId);
        public Task<CustomResponse<SchedueleDTO>> EditTimeAndPlace(EditScheduleTimeandPlace editScheduleTimeandPlace);
    }
}
