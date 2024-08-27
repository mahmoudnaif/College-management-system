using AutoMapper;
using College_managemnt_system.ClientModels;
using College_managemnt_system.CustomResponse;
using College_managemnt_system.DTOS;
using College_managemnt_system.Interfaces;
using College_managemnt_system.models;
using Microsoft.EntityFrameworkCore;

namespace College_managemnt_system.Repos
{//remodel the db so that the roomnumber and building are numbers instead of strings (primary key will be the roomnumber).
    public class ClassroomsRepo : IClassroomsRepo
    {
        private readonly CollegeDBContext _context;
        private readonly IMapper _mapper;

        public ClassroomsRepo(CollegeDBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<CustomResponse<List<ClassRoomDTO>>> GetAllClassRooms(TakeSkipModel takeSkipModel)
        {
            if (takeSkipModel.take < 0 || takeSkipModel.skip < 0)
                return new CustomResponse<List<ClassRoomDTO>>(400, "Take and skip must more than or equal 0");

            List<Classroom> classrooms = await _context.Classrooms.OrderBy(C => C.RoomNumber).Skip(takeSkipModel.skip).Take(takeSkipModel.take).ToListAsync();

            if (!classrooms.Any())
                return new CustomResponse<List<ClassRoomDTO>>(404, "Not found");

            List<ClassRoomDTO> classRoomsDTO = _mapper.Map<List<ClassRoomDTO>>(classrooms);

            return new CustomResponse<List<ClassRoomDTO>>(200, "Class rooms retreved successfully", classRoomsDTO);

        }
        public async Task<CustomResponse<List<ClassRoomDTO>>> SearchClassRooms(SearchModel searchModel)
        {
            if (searchModel.takeSkip.take < 0 || searchModel.takeSkip.skip < 0)
                return new CustomResponse<List<ClassRoomDTO>>(400, "Take and skip must more than or equal 0");

            var classrooms = await _context.Classrooms.Where(C => C.RoomNumber.StartsWith(searchModel.searchQuery)).OrderBy(C => C.RoomNumber).Skip(searchModel.takeSkip.skip).Take(searchModel.takeSkip.take).ToListAsync();

            if (!classrooms.Any())
                return new CustomResponse<List<ClassRoomDTO>>(404, "Not found");

            List<ClassRoomDTO> classRoomsDTO = _mapper.Map<List<ClassRoomDTO>>(classrooms);

            return new CustomResponse<List<ClassRoomDTO>>(200, "Class rooms retreved successfully", classRoomsDTO);
        }
        public async Task<CustomResponse<ClassRoomDTO>> AddClassRoom(ClassRoomInputModel classRoomInputModel)
        {
            if (!int.TryParse(classRoomInputModel.RoomNumber, out int roomNumber) || !int.TryParse(classRoomInputModel.Building, out int buildingNumber))
                return new CustomResponse<ClassRoomDTO>(400, "Room and building numbers must be numbers");

            if (roomNumber <= 0 || buildingNumber <= 0 || classRoomInputModel.Capacity < 0)
                return new CustomResponse<ClassRoomDTO>(400, "Room number, building number and capacity must be positive values");


            Classroom classroomDup = await _context.Classrooms.FirstOrDefaultAsync(C => C.RoomNumber == classRoomInputModel.RoomNumber);

            if (classroomDup != null)
                return new CustomResponse<ClassRoomDTO>(400, "Class room already exists");


            Classroom classroom = new Classroom()
            {
                RoomNumber = classRoomInputModel.RoomNumber,
                Building = classRoomInputModel.Building,
                Capacity = classRoomInputModel.Capacity
            };
            try
            {
                _context.Classrooms.Add(classroom);
                await _context.SaveChangesAsync();
                ClassRoomDTO classRoomDTO = _mapper.Map<ClassRoomDTO>(classroom);
                return new CustomResponse<ClassRoomDTO>(201,"Class room added successfully",classRoomDTO);
            }
            catch
            {
                return new CustomResponse<ClassRoomDTO>(500, "Internal server error");
            }


        }
        public async Task<CustomResponse<ClassRoomDTO>> EditClassRoomCapacity(int classRoomId, int capacity)
        {
            if (capacity < 0)
                return new CustomResponse<ClassRoomDTO>(400, "capacity must be more than or equal to 0");

            Classroom classroom = await _context.Classrooms.FirstOrDefaultAsync(C => C.ClassroomId == classRoomId);

            if (classroom == null)
                return new CustomResponse<ClassRoomDTO>(404, "Class room not found");

            if(classroom.Capacity == capacity)
                return new CustomResponse<ClassRoomDTO>(409,$"Class room capacity already set to: {capacity}");

            classroom.Capacity = capacity;

            try
            {
                await _context.SaveChangesAsync();
                ClassRoomDTO classRoomDTO = _mapper.Map<ClassRoomDTO>(classroom);
                return new CustomResponse<ClassRoomDTO>(200, "Class room capacity edited successfully", classRoomDTO);
            }
            catch
            {
                return new CustomResponse<ClassRoomDTO>(500, "Internal server error");
            }
        }
        public async Task<CustomResponse<bool>> RemoveClassRoom(int classRoomId)
        {
            Classroom classroom = await _context.Classrooms.FirstOrDefaultAsync(C => C.ClassroomId == classRoomId);

            if (classroom == null)
                return new CustomResponse<bool>(404, "Class room not found");

            try
            {
                _context.Classrooms.Remove(classroom);
                await _context.SaveChangesAsync();
                return new CustomResponse<bool>(200, "Class room deleted successfully");
            }
            catch
            {
                return new CustomResponse<bool>(500, "Internal server error");
            }
        }
    }
}
