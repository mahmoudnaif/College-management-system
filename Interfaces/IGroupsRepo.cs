using College_managemnt_system.ClientModels;
using College_managemnt_system.CustomResponse;
using College_managemnt_system.DTOS;

namespace College_managemnt_system.Interfaces
{
    public interface IGroupsRepo
    {
        public Task<CustomResponse<GroupDTO>> AddGroup(GroupInputModel groupInputModel);
        public Task<CustomResponse<bool>> DeleteGroup(int groupId);

        public Task<CustomResponse<List<GroupDTO>>> GetGroupsBySemesterId_StudentYear(int semesterId, int studentYear,TakeSkipModel model);

        public Task<CustomResponse<List<GroupDTO>>> GetGroupsByActiveSemester_StudentYear(int studentYear,TakeSkipModel model);
    }
}
