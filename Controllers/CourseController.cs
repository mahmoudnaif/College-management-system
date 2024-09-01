using College_managemnt_system.ClientModels;
using College_managemnt_system.CustomResponse;
using College_managemnt_system.DTOS;
using College_managemnt_system.Interfaces;
using College_managemnt_system.models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace College_managemnt_system.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : Controller
    {
        private readonly ICoursesRepo _coursesRepo;
        private readonly IPrereqsCoursesRepo _prereqsCoursesRepo;

        public CourseController(ICoursesRepo coursesRepo, IPrereqsCoursesRepo prereqsCoursesRepo)
        {
            _coursesRepo = coursesRepo;
            _prereqsCoursesRepo = prereqsCoursesRepo;
        }


        [HttpGet("All")]
        [Authorize(Roles = "root,admin")]
        public async Task<IActionResult> GetAllCourses([FromQuery]TakeSkipModel takeSkipModel)
        {
            var result = await _coursesRepo.GetCourses(takeSkipModel);
            return StatusCode(result.responseCode,result);
        }


        [HttpGet("{courseCode}")]
        [Authorize(Roles = "root,admin")]
        public async Task<IActionResult> GetCourseByCode(string courseCode)
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

        [HttpPut("{courseId}/name")]
        [Authorize(Roles ="root")]
        public async Task<IActionResult> EditCourseName(int courseId,[FromBody] string newName)
        {
            
            var result = await _coursesRepo.EditCourseName(courseId,newName);
            return StatusCode(result.responseCode, result);
        }
        [HttpPut("{courseId}/code")]
        [Authorize(Roles = "root")]
        public async Task<IActionResult> EditCourseCode(int courseId, [FromBody] string courseCode)
        {
            var result = await _coursesRepo.EditCourseCode(courseId, courseCode);
            return StatusCode(result.responseCode, result);
        }

        [HttpPut("{courseId}/Credits")]
        [Authorize(Roles = "root")]
        public async Task<IActionResult> EditCourseCredits(int courseId, [FromBody] int credits)
        {
            var result = await _coursesRepo.EditCreditHours(courseId,credits);
            return StatusCode(result.responseCode, result);
        }

        [HttpPut("{courseId}/Deprtment")]
        [Authorize(Roles = "root")]
        public async Task<IActionResult> ChangeDeprtment(int courseId, [FromBody] int departmentId)
        {
            var result = await _coursesRepo.EditDepartment(courseId,departmentId);
            return StatusCode(result.responseCode, result);
        }

        [HttpGet("{courseId}/preqs")]
        [Authorize(Roles = "root,admin")]
        public async Task<IActionResult> GetPrereqs(int courseId)
        {
            var result = await _prereqsCoursesRepo.GetPrereqsCourses(courseId);

            return StatusCode(result.responseCode, result);
        }

        [HttpPost("{courseId}/preqs")]
        [Authorize(Roles = "root,admin")]
        public async Task<IActionResult> AddPrereqs(int courseId, [FromBody] string prereqsCourseIdOrCode )
        {
            CustomResponse<PrereqDTO> respone;
            if(int.TryParse(prereqsCourseIdOrCode, out int prereqsCourseId))
            {
                respone = await _prereqsCoursesRepo.AddPrereqsCourse(courseId, prereqsCourseId);
            }
            else
            {
                respone = await _prereqsCoursesRepo.AddPrereqsCourse(courseId, prereqsCourseIdOrCode);
            }

        

            return StatusCode(respone.responseCode, respone);
        }

        [HttpDelete("{courseId}/preqs/{prereqsCourseId}")]
        [Authorize(Roles = "root,admin")]
        public async Task<IActionResult> DeletePrereqs(int courseId, int prereqsCourseId)
        {
          
            var result = await _prereqsCoursesRepo.RemovePrereqsCourse(courseId,prereqsCourseId);

            return StatusCode(result.responseCode, result);
        }


    }
}
