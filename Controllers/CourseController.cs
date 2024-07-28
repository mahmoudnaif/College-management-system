using College_managemnt_system.ClientModels;
using College_managemnt_system.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace College_managemnt_system.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : Controller
    {
        private readonly ICoursesRepo _coursesRepo;

        public CourseController(ICoursesRepo coursesRepo)
        {
            _coursesRepo = coursesRepo;
        }


        [HttpGet("All")]
        [Authorize(Roles = "root,admin")]
        public async Task<IActionResult> GetAllCourses([FromQuery]TakeSkipModel takeSkipModel)
        {
            var result = await _coursesRepo.GetCourses(takeSkipModel);
            return StatusCode(result.responseCode,result);
        }


        [HttpGet()]
        [Authorize(Roles = "root,admin")]
        public async Task<IActionResult> GetCourseByCode([FromQuery] string courseCode)
        {
            var result = await _coursesRepo.GetCourseByCourseCode(courseCode);
            return StatusCode(result.responseCode, result);
        }

        [HttpPost]
        [Authorize(Roles = "root,admin")]
        public async Task<IActionResult> AddCourse([FromBody]CourseInputModel courseInputModel)
        {
            var result = await _coursesRepo.AddCourse(courseInputModel);
            return StatusCode(result.responseCode, result);
        }

        [HttpPut("EditCourseName")]
        [Authorize(Roles ="root")]
        public async Task<IActionResult> EditCourseName(CourseEditModel courseEditModel)
        {
            var result = await _coursesRepo.EditCourseName(courseEditModel);
            return StatusCode(result.responseCode, result);
        }
        [HttpPut("EditCourseCode")]
        [Authorize(Roles = "root")]
        public async Task<IActionResult> EditCourseCode(CourseEditModel courseEditModel)
        {
            var result = await _coursesRepo.EditCourseCode(courseEditModel);
            return StatusCode(result.responseCode, result);
        }

        [HttpPut("EditCourseCredits")]
        [Authorize(Roles = "root")]
        public async Task<IActionResult> EditCourseCredits(CourseEditModel courseEditModel)
        {
            var result = await _coursesRepo.EditCreditHours(courseEditModel);
            return StatusCode(result.responseCode, result);
        }

        [HttpPut("ChangeDeprtment")]
        [Authorize(Roles = "root")]
        public async Task<IActionResult> ChangeDeprtment(CourseEditModel courseEditModel)
        {
            var result = await _coursesRepo.EditDepartment(courseEditModel);
            return StatusCode(result.responseCode, result);
        }



    }
}
