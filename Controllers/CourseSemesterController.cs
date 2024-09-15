using College_managemnt_system.ClientModels;
using College_managemnt_system.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace College_managemnt_system.Controllers
{
    [Route("api/semester/course")]
    [ApiController]
    public class CourseSemesterController : Controller
    {
        private readonly ICoursesSemestersRepo _coursesSemestersRepo;
        private readonly IAssistanceCoursesRepo _assistanceCoursesRepo;

        public CourseSemesterController(ICoursesSemestersRepo coursesSemestersRepo,IAssistanceCoursesRepo assistanceCoursesRepo)
        {
            _coursesSemestersRepo = coursesSemestersRepo;
            _assistanceCoursesRepo = assistanceCoursesRepo;
        }


        [HttpGet("Active")]
        [Authorize(Roles = "admin,root")]
        public async Task<IActionResult> GetActiveSemesterCourses() //migrated to semester controller
        {
            var response = await _coursesSemestersRepo.GetActiveSemesterCourses();

            return StatusCode(response.responseCode, response);
        }

        [HttpPut("Active")]
        [Authorize(Roles = "admin,root")]
        public async Task<IActionResult> EditActiveCourses([FromBody] bool isActive) //migrated to semester controller
        {
            var response = await _coursesSemestersRepo.EditActiveStatusForAllCourses(isActive);

            return StatusCode(response.responseCode, response);
        }

        [HttpPost]
        [Authorize(Roles = "admin,root")]
        public async Task<IActionResult> Add([FromBody] CourseSemesterInputModel model)
        {
            var response = await _coursesSemestersRepo.Add(model);

            return StatusCode(response.responseCode, response);
        }

   
    }
}
