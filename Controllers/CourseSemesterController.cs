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


        [HttpGet("ActiveSemester")]
        [Authorize(Roles = "admin,root")]
        public async  Task<IActionResult> GetActiveCourses()
        {
            var response = await _coursesSemestersRepo.GetActiveSemesterCourses();

            return StatusCode(response.responseCode, response);
        }

        [HttpPut("EditActiveAll")]
        [Authorize(Roles = "admin,root")]
        public async Task<IActionResult> EditActiveCourses([FromBody]bool isActive)
        {
            var response = await _coursesSemestersRepo.EditActiveStatusForAllCourses(isActive);

            return StatusCode(response.responseCode, response);
        }

        [HttpPost]
        [Authorize(Roles = "admin,root")]
        public async Task<IActionResult> Add([FromBody]CourseSemesterInputModel  model)
        {
            var response = await _coursesSemestersRepo.Add(model);

            return StatusCode(response.responseCode, response);
        }

        [HttpDelete]
        [Authorize(Roles = "admin,root")]
        public async Task<IActionResult> Delete([FromBody] int courseSemesterId )
        {
            var response = await _coursesSemestersRepo.Delete(courseSemesterId);

            return StatusCode(response.responseCode, response);
        }

        [HttpPut("prof")]
        [Authorize(Roles = "admin,root")]
        public async Task<IActionResult> ChangeProfessor([FromBody] ChangeProfInputModel model)
        {
            var response = await _coursesSemestersRepo.ChangeProfessor(model);

            return StatusCode(response.responseCode, response);
        }

        [HttpPut()]
        [Authorize(Roles = "admin,root")]
        public async Task<IActionResult> EditActivationStatus(EditActivationStatus model)
        {
            var response = await _coursesSemestersRepo.EditActivationStatus(model);

            return StatusCode(response.responseCode, response);
        }
    }
}
