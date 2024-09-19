using AutoMapper;
using College_managemnt_system.ClientModels;
using College_managemnt_system.CustomResponse;
using College_managemnt_system.DTOS;
using College_managemnt_system.Interfaces;
using College_managemnt_system.models;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using System.Linq.Expressions;

namespace College_managemnt_system.Repos
{
    public class RegisterSemesterCoursesRepo : IRegisterSemesterCoursesRepo
    {
        private readonly CollegeDBContext _context;
        private readonly IMapper _mapper;

        public RegisterSemesterCoursesRepo(CollegeDBContext context,IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<CustomResponse<List<CourseDTO>>> GetAvailableCourses(int studentId)
        {
            int? activeSemesterId = (await _context.Semesters.FirstOrDefaultAsync(S => S.IsActive))?.SemesterId;

            if (activeSemesterId == null)
                return new CustomResponse<List<CourseDTO>>(400, "No active semester");

            
            List<int> finishedStudentCoursesIds = await (from SC in _context.StudentCourses
                                                           where SC.StudentId == studentId && SC.Status == "Completed"
                                                           select SC.CourseId
                                                           ).ToListAsync();

            var coursesAndPrereqs = await (from CS in _context.Coursesemesters
                                           where CS.SemesterId == (int)activeSemesterId
                                           join C in _context.Courses on CS.CourseId equals C.CourseId
                                           join PR in _context.Prereqs on C.CourseId equals PR.CourseId into prereqsGroup
                                           from PR in prereqsGroup.DefaultIfEmpty() // Left join
                                           group new { prereqs = PR } by C into groupedCourses
                                           select new
                                           {
                                               course = groupedCourses.Key,
                                               prereqCourseIds = groupedCourses
                                                   .Where(g => g.prereqs != null) // Exclude nulls 
                                                   .Select(G => G.prereqs.PrereqCourseId)
                                           }).ToListAsync();
                   




            if (!coursesAndPrereqs.Any())
                return new CustomResponse<List<CourseDTO>>(404,"No courses found");

        

            List<Course> elgibleCourses = [];
            foreach (var courseAndPrereqs in coursesAndPrereqs)
            {

            
                if (!finishedStudentCoursesIds.Contains(courseAndPrereqs.course.CourseId))
                {

                    if (!courseAndPrereqs.prereqCourseIds.Any())
                    {
                        elgibleCourses.Add(courseAndPrereqs.course);
                    }
                    else if (finishedStudentCoursesIds.Any() && courseAndPrereqs.prereqCourseIds.All(id => finishedStudentCoursesIds.Contains(id)))
                    {
                        elgibleCourses.Add(courseAndPrereqs.course);
                    }   
                }
            }

            if (!elgibleCourses.Any())
                return new CustomResponse<List<CourseDTO>>(404, "No availavle courses found");

            List<CourseDTO> coursesDTO = _mapper.Map <List<CourseDTO>>(elgibleCourses);

            return new CustomResponse<List<CourseDTO>>(200,"Retreived successfully", coursesDTO);
        }
        public async Task<CustomResponse<object>> GetAvailableSchedule(List<int> courseIds)
        {
            if (!courseIds.Any())
                return new CustomResponse<object>(400, "Please select the courses you want to register");


            int? activeSemesterId = (await _context.Semesters.FirstOrDefaultAsync(S => S.IsActive))?.SemesterId;

            if (activeSemesterId == null)
                return new CustomResponse<object>(404, "No active semester");

            /* var groups = await _context.Schedules
              .Where(s => s.SemesterId == (int)activeSemesterId && courseIds.Contains(s.CourseId)) // Only consider the provided course IDs
              .Join(_context.SchedulesJoinsgroups,
                  schedule => schedule.ScheduleId,
                  SJG => SJG.ScheduleId,
                  (schedule, SJG) => new { SJG.GroupId, schedule.CourseId })
              .GroupBy(x => x.GroupId).Select(g => new GroupCourseDTO
              {
                  GroupId = g.Key,
                  CourseIds = g.Select(gc => gc.CourseId).Distinct().ToList()
              })
              .ToListAsync();*/

            var groups = await (from S in _context.Schedules
                                where S.SemesterId == (int)activeSemesterId && courseIds.Contains(S.CourseId)
                                join SG in _context.SchedulesJoinsgroups on S.ScheduleId equals SG.ScheduleId
                                join C in _context.Courses on S.CourseId equals C.CourseId
                                join G in _context.Groups on SG.GroupId equals G.GroupId
                                group new { schedules = S,course = C  } by G into groupSchedule
                                select new GroupCourseDTO{
                                   GroupId =  groupSchedule.Key.GroupId,
                                    groupName = groupSchedule.Key.GroupName,
                                    schedules = groupSchedule.Select(SG => 
                                    new SchedueleDTO
                                    {
                                        ScheduleId = SG.schedules.ScheduleId,
                                        SemesterId = SG.schedules.SemesterId,
                                        CourseId = SG.schedules.CourseId,
                                        courseName = SG.course.CourseName,
                                        DayOfWeek = SG.schedules.DayOfWeek,
                                        PeriodNumber = SG.schedules.PeriodNumber,
                                        Type = SG.schedules.Type,
                                        RoomNumber = SG.schedules.RoomNumber,
                                               
                                    }
                                    ).ToList(),
                                    CourseIds = groupSchedule.Select(gc => gc.course.CourseId).Distinct().ToList()


                                }
                                ).ToListAsync();

            if (!groups.Any())
                return new CustomResponse<object>(404, "Not found");


            foreach(var group in groups)
            {
                Console.WriteLine("start");
                Console.WriteLine($"{group.GroupId}: {group.groupName}");

                Console.WriteLine("schedules: ");

                foreach (var schedule  in group.schedules)
                {
                    Console.WriteLine($"{schedule.ScheduleId}");
                } 
                    Console.WriteLine("courses: ");
                foreach (var courseId  in group.CourseIds)
                {
                    Console.WriteLine(courseId);
                }

                Console.WriteLine("end");
            }


            List<GroupCourseDTO> elgibleGroups = [];

            foreach (var group in groups)
            {
                if (courseIds.All(cId => group.CourseIds.Contains(cId)))
                    elgibleGroups.Add(group);
            }

            if (elgibleGroups.Any()) { 

               /* List<GroupScheduleDTO> groupSchedulesDTO = [];

                foreach (var groupCourse in elgibleGroups) {

                    GroupScheduleDTO groupScheduleDTO = new GroupScheduleDTO();

                    groupScheduleDTO.GroupId = groupCourse.GroupId;

                    groupScheduleDTO.scheduelesDTO = await (from SG in _context.SchedulesJoinsgroups
                                                            where SG.GroupId == groupCourse.GroupId
                                                            join S in _context.Schedules on SG.ScheduleId equals S.ScheduleId
                                                            where groupCourse.CourseIds.Contains(S.CourseId)
                                                            join c in _context.Courses on S.CourseId equals c.CourseId
                                                            select new SchedueleDTO
                                                            {
                                                                ScheduleId = S.ScheduleId,
                                                                SemesterId = S.SemesterId,
                                                                CourseId = S.CourseId,
                                                                Type = S.Type,
                                                                DayOfWeek = S.DayOfWeek,
                                                                PeriodNumber = S.PeriodNumber,
                                                                courseName = c.CourseName,
                                                                RoomNumber = S.RoomNumber

                                                            }).ToListAsync();

                    groupSchedulesDTO.Add(groupScheduleDTO);
                }

                return new CustomResponse<object>(200, "Groups retreived successfully", groupSchedulesDTO);*/
                
                return new CustomResponse<object>(200, "Groups retreived successfully", elgibleGroups);
            }


            /* List<GroupCustomScheduleDTO> groupCustomSchedulesDTO = [];

             foreach(var groupCourse in groups) //must be simplified
             {
                 GroupCustomScheduleDTO groupCustomScheduleDTO = new (); // new simplfied approact in .net instead of new GroupCustomScheduleDTO();

                 groupCustomScheduleDTO.GroupId = groupCourse.GroupId;
                 List<SchedueleDTO> schedueleDTOs = await (from SG in _context.SchedulesJoinsgroups
                                                           where SG.GroupId == groupCourse.GroupId
                                                           join S in _context.Schedules on SG.ScheduleId equals S.ScheduleId
                                                           where groupCourse.CourseIds.Contains(S.CourseId)
                                                           join c in _context.Courses on S.CourseId equals c.CourseId
                                                           select new SchedueleDTO
                                                           {
                                                               ScheduleId = S.ScheduleId,
                                                               SemesterId = S.SemesterId,
                                                               CourseId = S.CourseId,
                                                               Type = S.Type,
                                                               DayOfWeek = S.DayOfWeek,
                                                               PeriodNumber = S.PeriodNumber,
                                                               courseName = c.CourseName,
                                                               RoomNumber = S.RoomNumber

                                                           }).ToListAsync();

                 foreach(var courseId in groupCourse.CourseIds)
                 {
                     CourseScheduleDTO courseScheduleDTO = new () {  courseId = courseId };

                     courseScheduleDTO.scheduelesDTO = schedueleDTOs.Where(S => S.CourseId == courseId).ToList();

                     groupCustomScheduleDTO.courseSchedulesDTO.Add(courseScheduleDTO);

                 }


                 groupCustomSchedulesDTO.Add(groupCustomScheduleDTO);
             }*/

         



            return new CustomResponse<object>(201, "Schedules retreived successfully", groups);

        }
      
        public async Task<CustomResponse<bool>> RegisterCourses_SchedulesByGroup(int studentId,List<int> courseIds, int groupId)
        {
            if (!courseIds.Any())
                return new CustomResponse<bool>(400, "Courses needs to be specifed");

            var avialableCoursesResponse = await GetAvailableCourses(studentId);

            if (avialableCoursesResponse.responseCode != 200)
                return new CustomResponse<bool>(avialableCoursesResponse.responseCode, $"An error occoured while fethcing student's elgible courses: {avialableCoursesResponse.responseMessage}");

            var availableCourses = avialableCoursesResponse.data;

            if (!courseIds.All(id => availableCourses.Any(C => C.CourseId == id)))
                return new CustomResponse<bool>(400, "Some courses are not elgible for this student");

            int? activeSemesterId = (await _context.Semesters.FirstOrDefaultAsync(S => S.IsActive))?.SemesterId;

            if (activeSemesterId == null)
                return new CustomResponse<bool>(404, "No active semester");

            var groupSchedules = await (from SJG in _context.SchedulesJoinsgroups
                                                   where SJG.GroupId == groupId
                                                   join S in _context.Schedules on SJG.ScheduleId equals S.ScheduleId
                                                   where S.SemesterId == (int)activeSemesterId && courseIds.Contains(S.CourseId) //join the group to check for semeser id or check the semester for each schedules like this 
                                                   select new { courseId = S.CourseId, SJG.ScheduleId, groupId = SJG.GroupId}
                             
                             ).ToListAsync();



            var groupCourseIds = groupSchedules.Select(S => S.courseId).Distinct();

            if (!courseIds.All(id => groupCourseIds.Contains(id)))
                return new CustomResponse<bool>(400, "Group does not have all the courses");




            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
           
                List<StudentCourse> studentCourses = new List<StudentCourse>();
                foreach (var courseId in courseIds)
                {
                   
                    studentCourses.Add(new StudentCourse() { StudentId = studentId, CourseId = courseId ,SemesterId = (int)activeSemesterId});
                }

                await _context.StudentCourses.AddRangeAsync(studentCourses);


                List<StudentsJoinsgroup> studentsJoinsgroups= [];
                foreach (var S in groupSchedules)
                {
                    studentsJoinsgroups.Add(new StudentsJoinsgroup() {StudentId = studentId,ScheduleId = S.ScheduleId, GroupId = S.groupId });
                }

                await _context.StudentsJoinsgroups.AddRangeAsync(studentsJoinsgroups);
                await _context.SaveChangesAsync();


                await transaction.CommitAsync();


                return new CustomResponse<bool>(200, "Success");
            }
            catch
            {
                transaction.Rollback();
                return new CustomResponse<bool>(500, "Internal server error");
            }





            }
        public Task<CustomResponse<bool>> RegisterCustomCourses_Schedules(int studentId,List<int> courseIds, List<int> scheduleIds)
        {
            throw new NotImplementedException();
        }

    
    }
}
