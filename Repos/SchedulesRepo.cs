using AutoMapper;
using College_managemnt_system.ClientModels;
using College_managemnt_system.CustomResponse;
using College_managemnt_system.DTOS;
using College_managemnt_system.Interfaces;
using College_managemnt_system.models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

            int roomNumber = schdulesInputModel.RoomNumber;

            string Type = schdulesInputModel.Type.ToUpper();

            int DayOfWeek = schdulesInputModel.DayOfWeek;

            int PeriodNumber = schdulesInputModel.PeriodNumber;

            int SemesterId = schdulesInputModel.SemesterId;



            if (DayOfWeek < 1 || DayOfWeek > 7)
                return new CustomResponse<SchedueleDTO>(400, "Day of week must be between 1 and 7");

            if (PeriodNumber < 1 || PeriodNumber > 3) // we should replace the "3" with a dynamic variable sat by the root user
                return new CustomResponse<SchedueleDTO>(400, "Perion number must be between 1 and 3");

            if (Type != "LEC" && Type != "LAB")
                return new CustomResponse<SchedueleDTO>(400, "Type must be Lec or Lab");

            Semester semester = await _context.Semesters.FirstOrDefaultAsync(S => S.SemesterId == SemesterId);
            if (semester == null)
                return new CustomResponse<SchedueleDTO>(404, "Semester does not exist");

            Coursesemester coursesemester = await _context.Coursesemesters.FirstOrDefaultAsync(C => C.CourseSemesterId == CourseSemesterId);
            if (coursesemester == null)
                return new CustomResponse<SchedueleDTO>(404, "Course does not exist");

            Classroom classroom = await _context.Classrooms.FirstOrDefaultAsync(C => C.RoomNumber == roomNumber);
            if (classroom == null)
                return new CustomResponse<SchedueleDTO>(404, "classroom does not exist");

            Schedule scheduleExists = await _context.Schedules.FirstOrDefaultAsync(S => S.RoomNumber == roomNumber && S.DayOfWeek == DayOfWeek && S.PeriodNumber == PeriodNumber && S.SemesterId == SemesterId);
            if (scheduleExists != null)
                return new CustomResponse<SchedueleDTO>(409, "There is a schedule at that time at that palce :> find another spot");


            Schedule schedule = new Schedule()
            {
                CourseSemesterId = CourseSemesterId,
                RoomNumber = roomNumber,
                Type = Type,
                DayOfWeek = DayOfWeek,
                PeriodNumber = PeriodNumber,
                SemesterId = SemesterId
            };

            try
            {
                _context.Schedules.Add(schedule);
                await _context.SaveChangesAsync();
                SchedueleDTO schedueleDTO = _mapper.Map<SchedueleDTO>(schedule);
                schedueleDTO.courseName = (await _context.Courses.FirstOrDefaultAsync(C => C.CourseId == coursesemester.CourseId))?.CourseName;
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
            int roomNumber = editScheduleTimeandPlace.RoomNumber;
            int DayOfWeek = editScheduleTimeandPlace.DayOfWeek;
            int PeriodNumber = editScheduleTimeandPlace.PeriodNumber;

            if (DayOfWeek < 1 || DayOfWeek > 7)
                return new CustomResponse<SchedueleDTO>(400, "Day of week must be between 1 and 7");

            if (PeriodNumber < 1 || PeriodNumber > 3) // we should replace the "3" with a dynamic variable sat by the root user
                return new CustomResponse<SchedueleDTO>(400, "Perion number must be between 1 and 3");

            Classroom classroom = await _context.Classrooms.FirstOrDefaultAsync(C => C.RoomNumber == roomNumber);

            if (classroom == null)
                return new CustomResponse<SchedueleDTO>(404, "classroom does not exist");

            Schedule schedule = await _context.Schedules.FirstOrDefaultAsync(S => S.ScheduleId == ScheduleId);

            if (schedule == null)
                return new CustomResponse<SchedueleDTO>(404, "Schedule does not exist");

            Schedule scheduleExists = await _context.Schedules.FirstOrDefaultAsync(S => S.RoomNumber == roomNumber && S.DayOfWeek == DayOfWeek && S.PeriodNumber == PeriodNumber && S.SemesterId == schedule.SemesterId );

            if (scheduleExists != null)
                return new CustomResponse<SchedueleDTO>(409, "There is a schedule at that time at that palce :> find another spot");



            schedule.RoomNumber = roomNumber;
            schedule.DayOfWeek = DayOfWeek;
            schedule.PeriodNumber = PeriodNumber;

            try
            {
                await _context.SaveChangesAsync();
                SchedueleDTO schedueleDTO = _mapper.Map<SchedueleDTO>(schedule);

                schedueleDTO.courseName = await _context.Coursesemesters.Where(C => C.CourseSemesterId == schedule.CourseSemesterId).Join(_context.Courses,
                    CS => CS.CourseId, C => C.CourseId,
                    (CS,C) => C.CourseName
                    ).FirstAsync();

              
                return new CustomResponse<SchedueleDTO>(200, "Time and place edited successfully", schedueleDTO);
            }
            catch
            {
                return new CustomResponse<SchedueleDTO>(500, "Internal server error");
            }
        }

        public async Task<CustomResponse<List<SchedueleDTO>>> GetScheduls(int semesterId, TakeSkipModel model)
        {

            if (model.take < 0 || model.skip < 0)
                return new CustomResponse<List<SchedueleDTO>>(400, "Take and skip must more than or equal 0");

            Semester semester = await _context.Semesters.FirstOrDefaultAsync(S => S.SemesterId == semesterId);

            if (semester == null)
                return new CustomResponse<List<SchedueleDTO>>(404, "Semester does not exist");


            List<SchedueleDTO> Schedules = await (from s in _context.Schedules.Where(S => S.SemesterId == semesterId).OrderBy(S=>S.SemesterId).Skip(model.skip).Take(model.take)
                        join cs in _context.Coursesemesters on s.CourseSemesterId equals cs.CourseSemesterId
                        join c in _context.Courses on cs.CourseId equals c.CourseId
                        
                        select new SchedueleDTO
                        {
                             ScheduleId = s.ScheduleId,

                              CourseSemesterId = s.CourseSemesterId,

                              RoomNumber = s.RoomNumber,

                              SemesterId = s.SemesterId,

                              Type = s.Type,

                              DayOfWeek = s.DayOfWeek,

                              PeriodNumber = s.PeriodNumber,

                              courseName = c.CourseName,

                             
                        }).ToListAsync();

            if (!Schedules.Any())
                return new CustomResponse<List<SchedueleDTO>>(404, "No schdules were found");

            return new CustomResponse<List<SchedueleDTO>>(200, "Schdeules retreived successfully", Schedules);
        }

        public async Task<CustomResponse<bool>> Remove(int scheduleId)
        {
            Schedule schedule = await _context.Schedules.FirstOrDefaultAsync(S => S.ScheduleId == scheduleId);

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

