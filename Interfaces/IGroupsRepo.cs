using College_managemnt_system.ClientModels;
using College_managemnt_system.CustomResponse;
using College_managemnt_system.DTOS;

namespace College_managemnt_system.Interfaces
{
    public interface IGroupsRepo
    {
        public Task<CustomResponse<GroupDTO>> AddGroup(GroupInputModel groupInputModel);
        public Task<CustomResponse<bool>> DeleteGroup(int groupId);
    }
}
