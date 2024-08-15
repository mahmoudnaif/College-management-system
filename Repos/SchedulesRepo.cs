using AutoMapper;
using College_managemnt_system.ClientModels;
using College_managemnt_system.CustomResponse;
using College_managemnt_system.DTOS;
using College_managemnt_system.Interfaces;
using College_managemnt_system.models;

namespace College_managemnt_system.Repos
{
    public class SchedulesRepo : ISchedulesRepo
    {
        private readonly CollegeDBContext _context;
        private readonly IMapper _mapper;

        public SchedulesRepo(CollegeDBContext context,IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        public async Task<CustomResponse<SchedueleDTO>> Add(SchedulesInputModel schdulesInputModel)
        {

            int CourseSemesterId = schdulesInputModel.CourseSemesterId;

            int ClassroomId = schdulesInputModel.ClassroomId;

            string Type = schdulesInputModel.Type.ToUpper();

            int DayOfWeek = schdulesInputModel.DayOfWeek;

            int PeriodNumber = schdulesInputModel.PeriodNumber;



            if (DayOfWeek < 1 || DayOfWeek > 7)
                return new CustomResponse<SchedueleDTO>(400, "Day of week must be between 1 and 7");

            if (PeriodNumber < 1 || PeriodNumber > 3) // we should replace the "3" with a dynamic variable sat by the root user
                return new CustomResponse<SchedueleDTO>(400, "Perion number must be between 1 and 3");

            if (Type != "LEC" && Type != "LAB")
                return new CustomResponse<SchedueleDTO>(400, "Type must be Lec or Lab");

            Coursesemester coursesemester = _context.Coursesemesters.FirstOrDefault(C => C.CourseSemesterId == CourseSemesterId);

            if (coursesemester == null)
                return new CustomResponse<SchedueleDTO>(404, "Course does not exist");

            Classroom classroom = _context.Classrooms.SingleOrDefault(C => C.ClassroomId == ClassroomId);

            if (classroom == null)
                return new CustomResponse<SchedueleDTO>(404, "classroom does not exist");

            Schedule schedule = new Schedule()
            {
                CourseSemesterId = CourseSemesterId,
                ClassroomId = ClassroomId,
                Type = Type,
                DayOfWeek = DayOfWeek,
                PeriodNumber = PeriodNumber
            };

            try
            {
                _context.Schedules.Add(schedule);
                await _context.SaveChangesAsync();
                SchedueleDTO schedueleDTO = _mapper.Map<SchedueleDTO>(schedule);
                schedueleDTO.courseName = _context.Courses.SingleOrDefault(C => C.CourseId == coursesemester.CourseId)?.CourseName;
                schedueleDTO.roomNumber = classroom.RoomNumber;
                return new CustomResponse<SchedueleDTO>(201, "Schedule added successfully", schedueleDTO);
            }
            catch
            {
                return new CustomResponse<SchedueleDTO>(500, "Internal server error");
            }
        }   

        public async Task<CustomResponse<SchedueleDTO>> EditTimeAndPlace(EditScheduleTimeandPlace editScheduleTimeandPlace)
        {
            int ScheduleId = editScheduleTimeandPlace.ScheduleId;
            int ClassroomId = editScheduleTimeandPlace.ClassroomId;
            int DayOfWeek = editScheduleTimeandPlace.DayOfWeek;
            int PeriodNumber = editScheduleTimeandPlace.PeriodNumber;

            if (DayOfWeek < 1 || DayOfWeek > 7)
                return new CustomResponse<SchedueleDTO>(400, "Day of week must be between 1 and 7");

            if (PeriodNumber < 1 || PeriodNumber > 3) // we should replace the "3" with a dynamic variable sat by the root user
                return new CustomResponse<SchedueleDTO>(400, "Perion number must be between 1 and 3");

            Classroom classroom = _context.Classrooms.SingleOrDefault(C => C.ClassroomId == ClassroomId);

            if (classroom == null)
                return new CustomResponse<SchedueleDTO>(404, "classroom does not exist");

            Schedule scheduleExists = _context.Schedules.SingleOrDefault(S => S.ClassroomId == ClassroomId && S.DayOfWeek == DayOfWeek && S.PeriodNumber == PeriodNumber);

            if (scheduleExists != null)
                return new CustomResponse<SchedueleDTO>(409, "There is a schedule at that time at that palce :> find another spot");

            Schedule schedule = _context.Schedules.SingleOrDefault(S => S.ScheduleId == ScheduleId);

            if (schedule == null)
                return new CustomResponse<SchedueleDTO>(404, "Schedule does not exist");

            schedule.ClassroomId = ClassroomId;
            schedule.DayOfWeek = DayOfWeek;
            schedule.PeriodNumber = PeriodNumber;

            try
            {
                await _context.SaveChangesAsync();
                SchedueleDTO schedueleDTO = _mapper.Map<SchedueleDTO>(schedule);

                schedueleDTO.courseName = _context.Coursesemesters.Where(C => C.CourseSemesterId == schedule.CourseSemesterId).Join(_context.Courses,
                    CS => CS.CourseId, C => C.CourseId,
                    (CS,C) => C.CourseName
                    ).First();

                schedueleDTO.roomNumber = classroom.RoomNumber;
                return new CustomResponse<SchedueleDTO>(200, "Time and place edited successfully", schedueleDTO);
            }
            catch
            {
                return new CustomResponse<SchedueleDTO>(500, "Internal server error");
            }
        }

        public async Task<CustomResponse<IEnumerable<SchedueleDTO>>> GetScheduls(TakeSkipModel takeSkipModel)
        {
            if (takeSkipModel.take < 0 || takeSkipModel.skip < 0)
                return new CustomResponse<IEnumerable<SchedueleDTO>>(400, "Take and skip must more than or equal 0");


            IEnumerable<SchedueleDTO> Schedules = from s in _context.Schedules.Skip(takeSkipModel.skip).Take(takeSkipModel.take)
                        join cs in _context.Coursesemesters on s.CourseSemesterId equals cs.CourseSemesterId
                        join c in _context.Courses on cs.CourseId equals c.CourseId
                        join r in _context.Classrooms on s.ClassroomId equals r.ClassroomId
                        select new SchedueleDTO
                        {
                             ScheduleId = s.ScheduleId,

                              CourseSemesterId = s.CourseSemesterId,

                              ClassroomId = s.ClassroomId,

                              Type = s.Type,

                              DayOfWeek = s.DayOfWeek,

                              PeriodNumber = s.PeriodNumber,

                              courseName = c.CourseName,

                              roomNumber = r.RoomNumber
                        };

            return new CustomResponse<IEnumerable<SchedueleDTO>>(200, "Schdeules retreived successfully", Schedules);
        }

        public async Task<CustomResponse<bool>> Remove(int scheduleId)
        {
            Schedule schedule = _context.Schedules.SingleOrDefault(S => S.ScheduleId == scheduleId);

            if (schedule == null)
                return new CustomResponse<bool>(404, "Schedule does not exist");

            try
            {
                _context.Schedules.Remove(schedule);
                await _context.SaveChangesAsync();
                return new CustomResponse<bool>(200, "Deleted successfully");
            }
            catch
            {
                return new CustomResponse<bool>(500, "Internal server error");
            }
        }
    }
}

