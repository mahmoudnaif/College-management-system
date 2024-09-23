using College_managemnt_system.ClientModels;
using College_managemnt_system.CustomResponse;
using College_managemnt_system.DTOS;
using College_managemnt_system.Interfaces;
using College_managemnt_system.Repos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace College_managemnt_system.Controllers
{
    [Route("api/admins/students")]
    [ApiController]
    public class RegistrationController : Controller
    {
        private readonly IRegisterSemesterCoursesRepo _registerSemesterCoursesRepo;
        private readonly PremissionUtilsRepo _premissionUtilsRepo;

        public RegistrationController(IRegisterSemesterCoursesRepo registerSemesterCoursesRepo,PremissionUtilsRepo premissionUtilsRepo)
        {
            _registerSemesterCoursesRepo = registerSemesterCoursesRepo;
            _premissionUtilsRepo = premissionUtilsRepo;
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


    }
}
