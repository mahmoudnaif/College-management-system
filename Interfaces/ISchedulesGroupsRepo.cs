using College_managemnt_system.CustomResponse;
using College_managemnt_system.DTOS;

namespace College_managemnt_system.Interfaces
{
    public interface ISchedulesGroupsRepo
    {
        public Task<CustomResponse<List<SchedueleDTO>>> GetSchedulsByGroup(int groupId);

        public Task<CustomResponse<ScheduleJoinsGroupDTO>> AddScheduleToGroup(int groupId,int ScheduleId);

        public Task<CustomResponse<bool>> RemoveScheduleFromGroup(int groupId, int ScheduleId);

    }
}
