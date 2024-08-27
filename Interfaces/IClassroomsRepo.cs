using College_managemnt_system.ClientModels;
using College_managemnt_system.CustomResponse;
using College_managemnt_system.DTOS;

namespace College_managemnt_system.Interfaces
{
    public interface IClassroomsRepo
    {
        public Task<CustomResponse<List<ClassRoomDTO>>> GetAllClassRooms(TakeSkipModel takeSkipModel);
        public Task<CustomResponse<List<ClassRoomDTO>>> SearchClassRooms(SearchModel searchModel);
        public Task<CustomResponse<ClassRoomDTO>> AddClassRoom(ClassRoomInputModel classRoomInputModel);
        public Task<CustomResponse<bool>> RemoveClassRoom(int classRoonmId);
        public Task<CustomResponse<ClassRoomDTO>> EditClassRoomCapacity(int classRoomId, int capacity);
    }
}
