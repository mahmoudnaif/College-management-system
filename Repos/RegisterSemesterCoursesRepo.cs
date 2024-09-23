using AutoMapper;
using College_managemnt_system.ClientModels;
using College_managemnt_system.CustomResponse;
using College_managemnt_system.DTOS;
using College_managemnt_system.Interfaces;
using College_managemnt_system.models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Reflection.Metadata.Ecma335;

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
      
        public async Task<CustomResponse<bool>> RegisterCourses_SchedulesByGroup(int studentId,List<int> courseIds, int groupId,bool bypassrules = false)
        {
            if (!courseIds.Any())
                return new CustomResponse<bool>(400, "Courses needs to be specifed");


            courseIds = courseIds.Distinct().ToList();

            Student student = await _context.Students.FirstOrDefaultAsync(S => S.StudentId == studentId);

            if (student == null)
                return new CustomResponse<bool>(404, "Student does not exist");

            var avialableCoursesResponse = await GetAvailableCourses(studentId);

            if (avialableCoursesResponse.responseCode != 200)
                return new CustomResponse<bool>(avialableCoursesResponse.responseCode, $"An error occoured while fethcing student's elgible courses: {avialableCoursesResponse.responseMessage}");

            var availableCourses = avialableCoursesResponse.data;

           /* if (!courseIds.All(id => availableCourses.Any(C => C.CourseId == id)))
                return new CustomResponse<bool>(400, "Some courses are not elgible for this student");*/


            var courses = availableCourses.Where(avC => courseIds.Contains(avC.CourseId));

            if(courses.Count() != courseIds.Count())
                return new CustomResponse<bool>(400, "Some courses are not elgible for this student");

            if (!bypassrules)
            {
                int sumOfCreditHours = courses.Sum(C => C.Credits);
                if (sumOfCreditHours < 9)
                    return new CustomResponse<bool>(403, "You need to register at lease 9 hours");
                
                if (sumOfCreditHours > 18)
                    return new CustomResponse<bool>(403, "You can't register more than 18 hours");

            }



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


                return new CustomResponse<bool>(201, "Courses and schedule registered successfully");
            }
            catch
            {
                transaction.Rollback();
                return new CustomResponse<bool>(500, "Internal server error");
            }





            }
        public async Task<CustomResponse<bool>> RegisterCustomCourses_Schedules(int studentId,List<CustomGroupCourseInputModel> groupCourseInputModels, bool bypassrules = false)
        {
            if (!groupCourseInputModels.Any())
                return new CustomResponse<bool>(400, "Courses and groups needs to be specifed");


            if (groupCourseInputModels.GroupBy(g => g.CourseId).Any(group => group.Count() > 1))
                return new CustomResponse<bool>(400, "You must specify one group per course not more");

            Student student = await _context.Students.FirstOrDefaultAsync(S => S.StudentId == studentId);

            if (student == null)
                return new CustomResponse<bool>(404, "Student does not exist");

            var avialableCoursesResponse = await GetAvailableCourses(studentId);

            if (avialableCoursesResponse.responseCode != 200)
                return new CustomResponse<bool>(avialableCoursesResponse.responseCode, $"An error occoured while fethcing student's elgible courses: {avialableCoursesResponse.responseMessage}");

            var availableCourses = avialableCoursesResponse.data;


            var courses = availableCourses.Where(avC => groupCourseInputModels.Any(GC=> GC.CourseId ==  avC.CourseId));

            if (courses.Count() != groupCourseInputModels.Count())
                return new CustomResponse<bool>(400, "Some courses are not elgible for this student");

            if (!bypassrules)
            {
                int sumOfCreditHours = courses.Sum(C => C.Credits);
                if (sumOfCreditHours < 9)
                    return new CustomResponse<bool>(403, "You need to register at lease 9 hours");

                if (sumOfCreditHours > 18)
                    return new CustomResponse<bool>(403, "You can't register more than 18 hours");

            }

         
      

            int? activeSemesterId = (await _context.Semesters.FirstOrDefaultAsync(S => S.IsActive))?.SemesterId;

            if (activeSemesterId == null)
                return new CustomResponse<bool>(404, "No active semester");

            var groupIds = groupCourseInputModels.Select(GC => GC.GroupId).Distinct().ToList();
            var courseIds = groupCourseInputModels.Select(GC => GC.CourseId).ToList();


            var courseInOneGroupResponse = await coursesExistInOneGroup(courseIds, (int)activeSemesterId);

            if (courseInOneGroupResponse.responseCode == 200)
                return new CustomResponse<bool>(403, courseInOneGroupResponse.responseMessage);
                


            var Schedules = await (from SJG in _context.SchedulesJoinsgroups
                                                where groupIds.Contains(SJG.GroupId)
                                                join S in _context.Schedules on SJG.ScheduleId equals S.ScheduleId
                                                where courseIds.Contains(S.CourseId)
                                                select new
                                                {
                                                    groupId = SJG.GroupId,
                                                    schedule = S
                                                }
                                                ).ToListAsync();


            var groupsSchedules = from GCIM in groupCourseInputModels
                                  join S in Schedules on new { GCIM.GroupId, GCIM.CourseId } equals new { GroupId = S.groupId, S.schedule.CourseId }
                                  /*  group new {  S.schedule } by S.groupId into SG
                                    select new
                                    {
                                        groupId = SG.Key,
                                        schedules = SG.Select(S => S.schedule).ToList(),
                                    };*/
                                  select S;
                                        

            var scheduleCourseIds = groupsSchedules.Select(GS => GS.schedule.CourseId).Distinct().ToList();

            if (scheduleCourseIds.Count() != courseIds.Count())
                return new CustomResponse<bool>(400,"some courses do not exist in their corresponding groups");

            if (!SchedulesDoNotOverLap(groupsSchedules.Select(GS => GS.schedule).ToList()))
                return new CustomResponse<bool>(409, "Schedules conflict please find another schedule");

            var transaction = _context.Database.BeginTransaction();

            try
            {
                List<StudentCourse> studentCourse = new List<StudentCourse>();

                foreach(var cId in courseIds)
                {
                    studentCourse.Add(new StudentCourse { StudentId = studentId, CourseId = cId , SemesterId = (int)activeSemesterId});
                }

                await _context.StudentCourses.AddRangeAsync(studentCourse);


                List<StudentsJoinsgroup> studentsJoinsgroups= new List<StudentsJoinsgroup>();
                foreach (var GS in groupsSchedules)
                {
                    studentsJoinsgroups.Add(new StudentsJoinsgroup() { StudentId = studentId, GroupId = GS.groupId, ScheduleId = GS.schedule.ScheduleId});
                }

                await _context.StudentsJoinsgroups.AddRangeAsync(studentsJoinsgroups);

                await _context.SaveChangesAsync();

                await transaction.CommitAsync();
                return new CustomResponse<bool>(201, "Courses and schedule registered successfully");
            }
            catch
            {
                transaction.Rollback();
                return new CustomResponse<bool>(500, "Internal server errror");
            }

        }


        public async Task<CustomResponse<bool>> coursesExistInOneGroup(List<int> courseIds, int activeSemesterId)
        {

            var groups = await (from S in _context.Schedules
                                where S.SemesterId == activeSemesterId && courseIds.Contains(S.CourseId)
                                join SG in _context.SchedulesJoinsgroups on S.ScheduleId equals SG.ScheduleId
                                join C in _context.Courses on S.CourseId equals C.CourseId
                                join G in _context.Groups on SG.GroupId equals G.GroupId
                                group new { schedules = S, course = C } by G into groupSchedule
                                select new GroupCourseDTO
                                {
                                    GroupId = groupSchedule.Key.GroupId,
                                    CourseIds = groupSchedule.Select(gc => gc.course.CourseId).Distinct().ToList()


                                }
                                ).ToListAsync();

            if (!groups.Any()) //no schedules at all
                return new CustomResponse<bool>(200, "No schedules at all");


            int count = 0;

            foreach (var group in groups)
            {
                if (courseIds.All(cId => group.CourseIds.Contains(cId)))
                    count++;
            }

            if (count != 0)
                return new CustomResponse<bool>(200,"Courses exist in one group");


            return new CustomResponse<bool>(400,"Courses does not exsit in one group");


        }

        /*public async Task<bool> elgibleCourses(int studentId,int activeSemesterId,List<int> courseIds)
        {
         

            List<int> finishedStudentCoursesIds = await (from SC in _context.StudentCourses
                                                         where SC.StudentId == studentId && SC.Status == "Completed"
                                                         select SC.CourseId
                                                           ).ToListAsync();

            var coursesAndPrereqs = await (from CS in _context.Coursesemesters
                                           where CS.SemesterId == activeSemesterId
                                           join C in _context.Courses on CS.CourseId equals C.CourseId
                                           join PR in _context.Prereqs on C.CourseId equals PR.CourseId into prereqsGroup
                                           from PR in prereqsGroup.DefaultIfEmpty() // Left join
                                           group new { prereqs = PR } by C into groupedCourses
                                           select new
                                           {
                                               groupedCourses.Key.CourseId,
                                               prereqCourseIds = groupedCourses
                                                   .Where(g => g.prereqs != null) // Exclude nulls 
                                                   .Select(G => G.prereqs.PrereqCourseId)
                                           }).ToListAsync();





            if (!coursesAndPrereqs.Any())
                return false;



            List<int> elgibleCourseIds = [];
            foreach (var courseAndPrereqs in coursesAndPrereqs)
            {


                if (!finishedStudentCoursesIds.Contains(courseAndPrereqs.CourseId))
                {

                    if (!courseAndPrereqs.prereqCourseIds.Any())
                    {
                        elgibleCourseIds.Add(courseAndPrereqs.CourseId);
                    }
                    else if (finishedStudentCoursesIds.Any() && courseAndPrereqs.prereqCourseIds.All(id => finishedStudentCoursesIds.Contains(id)))
                    {
                        elgibleCourseIds.Add(courseAndPrereqs.CourseId);
                    }
                }
            }

            if (!elgibleCourseIds.Any())
                return false;


            var courses = elgibleCourseIds.Where(cId => courseIds.Contains(cId));

            if (courses.Count() != courseIds.Count())
                return false;

            return true;

        }*/

        public bool SchedulesDoNotOverLap(List<Schedule> schedules)
        {
            HashSet<(int, int)> scheduleTracker = new HashSet<(int, int)>();

            foreach (var schedule in schedules)
            {
                if (!scheduleTracker.Add( (schedule.DayOfWeek, schedule.PeriodNumber) ))
                    return false;
            }

            return true;
        }


    }
}
