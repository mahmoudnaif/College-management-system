using AutoMapper;
using College_managemnt_system.ClientModels;
using College_managemnt_system.CustomResponse;
using College_managemnt_system.DTOS;
using College_managemnt_system.Interfaces;
using College_managemnt_system.models;

namespace College_managemnt_system.Repos
{
    public class GroupsRepo : IGroupsRepo
    {
        private readonly CollegeDBContext _context;
        private readonly IMapper _mapper;

        public GroupsRepo(CollegeDBContext collegeDBContext,IMapper mapper)
        {
            _context = collegeDBContext;
            _mapper = mapper;
        }
        public async Task<CustomResponse<GroupDTO>> AddGroup(GroupInputModel groupInputModel)
        {
            if (!(groupInputModel.StudentsYear > 0 && groupInputModel.StudentsYear < 5))
                return new CustomResponse<GroupDTO>(400,"Student year must be between 1 and 4");

            if (groupInputModel.GroupName.Length != 2)
                return new CustomResponse<GroupDTO>(400, "Group name must contain one char and one number. ex: A1,B1, etc");

            if(!char.IsLetter(groupInputModel.GroupName[0])  || !char.IsNumber(groupInputModel.GroupName[1]))
                return new CustomResponse<GroupDTO>(400, "Group name must contain one char and one number. ex: A1,B1, etc");

            Semester semester = _context.Semesters.SingleOrDefault(S => S.SemesterId == groupInputModel.SemesterId);

            if (semester == null)
                return new CustomResponse<GroupDTO>(404, "Smester was not found");


            Group groupExists = _context.Groups.SingleOrDefault(G => G.GroupName == groupInputModel.GroupName && G.StudentsYear == groupInputModel.StudentsYear && G.SemesterId == groupInputModel.SemesterId );

            if (groupExists != null)
                return new CustomResponse<GroupDTO>(409, "Group with the name: " + groupInputModel.GroupName + " and year: " + groupInputModel.StudentsYear + " already exists for this semester");

            Group group = new Group()
            {
                GroupName = groupInputModel.GroupName,
                StudentsYear= groupInputModel.StudentsYear,
                SemesterId = groupInputModel.SemesterId
            };

            try
            {
                _context.Groups.Add(group);
                await _context.SaveChangesAsync();
                GroupDTO groupDTO = _mapper.Map<GroupDTO>(group);
                return new CustomResponse<GroupDTO>(201, "Group added successfully", groupDTO);
            }
            catch
            {
                return new CustomResponse<GroupDTO>(500, "Internal server error");
            }
        }

        public async Task<CustomResponse<bool>> DeleteGroup(int groupId)
        {
            Group group= _context.Groups.SingleOrDefault(G => G.GroupId == groupId);

            if (group == null)
                return new CustomResponse<bool>(404, "Group was not found");

            try
            {
                _context.Groups.Remove(group);
                await _context.SaveChangesAsync();
                GroupDTO groupDTO = _mapper.Map<GroupDTO>(group);
                return new CustomResponse<bool>(200, "Group deleted successfully");
            }
            catch
            {
                return new CustomResponse<bool>(500, "Internal server error");
            }

        }
    }
}
