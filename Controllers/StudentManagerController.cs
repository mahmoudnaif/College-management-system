using College_managemnt_system.ClientModels;
using College_managemnt_system.CustomResponse;
using College_managemnt_system.DTOS;
using College_managemnt_system.Interfaces;
using College_managemnt_system.Repos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace College_managemnt_system.Controllers
{
    [Route("api/admin/student")]
    [ApiController]
    public class StudentManagerController : Controller
    {
        private readonly IRegisterSemesterCoursesRepo _registerSemesterCoursesRepo;
        private readonly PremissionUtilsRepo _premissionUtilsRepo;
        private readonly IStudentCoursesRepo _studentCoursesRepo;
        private readonly IStudentSchedulesRepo _studentSchedulesRepo;

        public StudentManagerController(IRegisterSemesterCoursesRepo registerSemesterCoursesRepo,PremissionUtilsRepo premissionUtilsRepo, IStudentCoursesRepo studentCoursesRepo,IStudentSchedulesRepo studentSchedulesRepo)
        {
            _registerSemesterCoursesRepo = registerSemesterCoursesRepo;
            _premissionUtilsRepo = premissionUtilsRepo;
            _studentCoursesRepo = studentCoursesRepo;
            _studentSchedulesRepo = studentSchedulesRepo;
        }

        [HttpGet("{studentId}/courses/Active")]
        [Authorize(Roles = "root,admin")]

        public async Task<IActionResult> GetActiveStudentCourses(int studentId)
        {
           

            var response = await _studentCoursesRepo.GetActiveStudentCourses(studentId);
            return StatusCode(response.responseCode, response);
        }

        [HttpGet("{studentId}/courses/completed")]
        [Authorize(Roles = "root,admin")]

        public async Task<IActionResult> GetAllFinishedStudentCourses(int studentId, [FromQuery] TakeSkipModel takeSkipModel)
        {
            var response = await _studentCoursesRepo.GetAllFinishedStudentCourses(studentId, takeSkipModel);
            return StatusCode(response.responseCode, response);
        }


        [HttpPut("{studentId}/course/{courseId}/withdrawl")]
        [Authorize(Roles = "root,admin")]

        public async Task<IActionResult> WithDrawlFromCourse(int studentId, int courseId, [FromQuery] bool hasReason = false)
        {
            if (!await _premissionUtilsRepo.CheckWithdrawingCourses())
                return StatusCode(403, new CustomResponse<bool>(403, "Premission denied"));

            var response = await _studentCoursesRepo.WithDrawlFromCourse(studentId,courseId,hasReason);
            return StatusCode(response.responseCode, response);
        }

         [HttpPut("{studentId}/course/{courseId}/Drop")]
        [Authorize(Roles = "root,admin")]

        public async Task<IActionResult> DropCourse(int studentId, int courseId)
        {
            if (!await _premissionUtilsRepo.CheckDroppingCourses())
                return StatusCode(403, new CustomResponse<bool>(403, "Premission denied"));


            var response = await _studentCoursesRepo.DropCourse(studentId,courseId);
            return StatusCode(response.responseCode, response);
        }

        [HttpPut("{studentId}/course/{courseId}/Incomplete")]
        [Authorize(Roles = "root,admin")]

        public async Task<IActionResult> SetCourseIncomplete(int studentId, int courseId)
        {
            if (!await _premissionUtilsRepo.CheckGradingCourses())
                return StatusCode(403, new CustomResponse<bool>(403, "Premission denied"));

            var response = await _studentCoursesRepo.SetCourseIncomplete(studentId, courseId);
            return StatusCode(response.responseCode, response);
        } 
        
        [HttpPut("{studentId}/course/{courseId}/Grade")]
        [Authorize(Roles = "root,admin")]

        public async Task<IActionResult> SetGrade(int studentId, int courseId, [FromBody] String grade)
        {
            if (!await _premissionUtilsRepo.CheckGradingCourses())
                return StatusCode(403, new CustomResponse<bool>(403, "Premission denied"));

            var response = await _studentCoursesRepo.SetGrade(studentId, courseId, grade);
            return StatusCode(response.responseCode, response);
        }
        
        [HttpPut("{studentId}/course/{courseId}/Appeal")]
        [Authorize(Roles = "root,admin")]

        public async Task<IActionResult> AppealGrade(int studentId, int courseId, [FromBody] String grade)
        {


            var response = await _studentCoursesRepo.AppealGrade(studentId, courseId, grade);
            return StatusCode(response.responseCode, response);
        }



        [HttpGet("{studentId}/registration/courses")]
        [Authorize(Roles = "root,admin")]

        public async Task<IActionResult> GetAvailableStudentCourses(int studentId)
        {
            if (!await _premissionUtilsRepo.CheckRegestringCourses())
                return StatusCode(403, new CustomResponse<List<CourseDTO>>(403,"Premission denied"));



            var response = await _registerSemesterCoursesRepo.GetAvailableCourses(studentId);
            return StatusCode(response.responseCode, response);
        }

        [HttpGet("registration/Schedules")]
        [Authorize(Roles = "root,admin")]

        public async Task<IActionResult> GetAvailableSchedule([FromQuery] List<int> courseIds)
        {
            if (!await _premissionUtilsRepo.CheckRegestringCourses())
                return StatusCode(403, new CustomResponse<object>(403, "Premission denied"));

            var response = await _registerSemesterCoursesRepo.GetAvailableSchedule(courseIds);
            return StatusCode(response.responseCode, response);
        }

        [HttpPost("{studentId}/registration/group/{groupId}")]
        [Authorize(Roles = "root,admin")]

        public async Task<IActionResult> RegisterStudenCourses_Schedules(int studentId, int groupId, [FromBody] List<int> courseIds, [FromQuery] bool bypassRules = false)
        {
            if (!await _premissionUtilsRepo.CheckRegestringCourses())
                return StatusCode(403, new CustomResponse<bool>(403, "Premission denied"));

            var response = await _registerSemesterCoursesRepo.RegisterCourses_SchedulesByGroup(studentId,courseIds,groupId,bypassRules);
            return StatusCode(response.responseCode, response);
        }


        [HttpPost("{studentId}/registration/Custom")]
        [Authorize(Roles = "root,admin")]

        public async Task<IActionResult> RegisterStudenCourses_Schedules(int studentId, [FromBody] List<CustomGroupCourseInputModel> groupCourseInputModels, [FromQuery] bool bypassRules = false)
        {
            if (!await _premissionUtilsRepo.CheckRegestringCourses())
                return StatusCode(403, new CustomResponse<bool>(403, "Premission denied"));


            var response = await _registerSemesterCoursesRepo.RegisterCustomCourses_Schedules(studentId, groupCourseInputModels, bypassRules);
            return StatusCode(response.responseCode, response);
        }

        [HttpGet("{studentId}/Schedule/Active")]
        [Authorize(Roles = "root,admin")]

        public async Task<IActionResult> GetStudentActiveSchedule(int studentId)
        {
            var response = await _studentSchedulesRepo.GetStudentActiveSchedule(studentId);
            return StatusCode(response.responseCode, response);
        }

    }
}
