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

        public CourseSemesterController(ICoursesSemestersRepo coursesSemestersRepo)
        {
            _coursesSemestersRepo = coursesSemestersRepo;
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

        [HttpDelete("{courseSemesterId}")]
        [Authorize(Roles = "admin,root")]
        public async Task<IActionResult> Delete(int courseSemesterId)
        {
            var response = await _coursesSemestersRepo.Delete(courseSemesterId);

            return StatusCode(response.responseCode, response);
        }

        [HttpPut("{courseSemesterId}/prof")]
        [Authorize(Roles = "admin,root")]
        public async Task<IActionResult> ChangeProfessor(int courseSemesterId, [FromBody] int profId)
        {
            var response = await _coursesSemestersRepo.ChangeProfessor(courseSemesterId,profId);

            return StatusCode(response.responseCode, response);
        }

        [HttpPut("{courseSemesterId}/acitve")]
        [Authorize(Roles = "admin,root")]
        public async Task<IActionResult> EditActivationStatus(int courseSemesterId, bool isActive)
        {
            var response = await _coursesSemestersRepo.EditActivationStatus(courseSemesterId, isActive);

            return StatusCode(response.responseCode, response);
        }
    }
}
