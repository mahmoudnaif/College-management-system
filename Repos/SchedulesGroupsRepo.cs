using AutoMapper;
using College_managemnt_system.CustomResponse;
using College_managemnt_system.DTOS;
using College_managemnt_system.Interfaces;
using College_managemnt_system.models;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
namespace College_managemnt_system.Repos
{
    public class SchedulesGroupsRepo : ISchedulesGroupsRepo
    {
        private readonly CollegeDBContext _context;
        private readonly IMapper _mapper;

        public SchedulesGroupsRepo(CollegeDBContext context,IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<CustomResponse<ScheduleJoinsGroupDTO>> AddScheduleToGroup(int groupId, int ScheduleId)
        {
            Group group = await _context.Groups.FirstOrDefaultAsync(G => G.GroupId == groupId);

            if (group == null)
                return new CustomResponse<ScheduleJoinsGroupDTO>(404, "Group does not exist");

            Schedule schedule = await _context.Schedules.FirstOrDefaultAsync(S => S.ScheduleId == ScheduleId);

            if (schedule == null)
                return new CustomResponse<ScheduleJoinsGroupDTO>(404, "Schedule does not exist");

            SchedulesJoinsgroup schedulesJoinsgroup = new SchedulesJoinsgroup()
            {
                ScheduleId = ScheduleId,
                GroupId = groupId,
            };

            try
            {
                _context.SchedulesJoinsgroups.Add(schedulesJoinsgroup);
                await _context.SaveChangesAsync();
                ScheduleJoinsGroupDTO scheduleJoinsGroupDTO = new ScheduleJoinsGroupDTO() { GroupId = groupId, ScheduleId = ScheduleId };
                return new CustomResponse<ScheduleJoinsGroupDTO>(201, "Schedule added to group successfully", scheduleJoinsGroupDTO);
            }
            catch
            {
                return new CustomResponse<ScheduleJoinsGroupDTO>(500, "Internal server error");
            }
        }

        public async Task<CustomResponse<List<SchedueleDTO>>> GetSchedulsByGroup(int groupId)
        {
            List<SchedueleDTO> scheduelesDTO = await (from SG in _context.SchedulesJoinsgroups
                                              where SG.GroupId == groupId
                                              join S in _context.Schedules on SG.ScheduleId equals S.ScheduleId
                                              join CS in _context.Coursesemesters on S.CourseSemesterId equals CS.CourseSemesterId
                                              join C in _context.Courses on CS.CourseId equals C.CourseId
                                              
                                                  select new SchedueleDTO
                                                  {
                                                      ScheduleId = S.ScheduleId,

                                                      CourseSemesterId = S.CourseSemesterId,

                                                      RoomNumber = S.RoomNumber,

                                                      SemesterId = S.SemesterId,

                                                      Type = S.Type,

                                                      DayOfWeek = S.DayOfWeek,

                                                      PeriodNumber = S.PeriodNumber,

                                                      courseName = C.CourseName,

                                                   
                                                  }).ToListAsync();


            if (!scheduelesDTO.Any())
                return new CustomResponse<List<SchedueleDTO>>(404, "No schedules where found");

            return new CustomResponse<List<SchedueleDTO>>(200, "Schedules retreived successfully", scheduelesDTO);
        }

        public async Task<CustomResponse<bool>> RemoveScheduleFromGroup(int groupId, int ScheduleId)
        {
            SchedulesJoinsgroup schedulesJoinsgroup = await _context.SchedulesJoinsgroups.FirstOrDefaultAsync(SG=>SG.GroupId == groupId && SG.ScheduleId == ScheduleId);

            if (schedulesJoinsgroup == null)
                return new CustomResponse<bool>(404, "Schedule does not exist in this group");

            try
            {
                _context.SchedulesJoinsgroups.Remove(schedulesJoinsgroup);
                await _context.SaveChangesAsync();
                return new CustomResponse<bool>(200, "Schedule removed succesffully from this group");
            }
            catch
            {
                return new CustomResponse<bool>(500, "Internal server error");
            }
        }
    }
}
