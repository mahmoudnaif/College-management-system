using College_managemnt_system.ClientModels;
using College_managemnt_system.Interfaces;
using College_managemnt_system.Repos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace College_managemnt_system.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SemesterController : Controller
    {
        private readonly ISemstersRepo _semstersRepo;
        private readonly ICoursesSemestersRepo _coursesSemestersRepo;
        private readonly ISchedulesRepo _schedulesRepo;
        private readonly IGroupsRepo _groupsRepo;
        private readonly IAssistanceCoursesRepo _assistanceCoursesRepo;

        public SemesterController(ISemstersRepo semstersRepo, ICoursesSemestersRepo coursesSemestersRepo, ISchedulesRepo schedulesRepo,IGroupsRepo groupsRepo,IAssistanceCoursesRepo assistanceCoursesRepo)
        {
            _semstersRepo = semstersRepo;
            _coursesSemestersRepo = coursesSemestersRepo;
            _schedulesRepo = schedulesRepo;
            _groupsRepo = groupsRepo;
            _assistanceCoursesRepo = assistanceCoursesRepo;
        }


        [HttpGet("GetSemesters")]
        [Authorize(Roles = "admin,root")]
        public async Task<IActionResult> GetSemesters([FromQuery] TakeSkipModel takeSkipModel)
        {

            var response = await _semstersRepo.GetSemesters(takeSkipModel);

            return StatusCode(response.responseCode, response);
        }


        [HttpGet("{id}")]
        [Authorize(Roles = "admin,root")]
        public async Task<IActionResult> GetSemesterById(int id)
        {

            var response = await _semstersRepo.GetSingleSemester(id);

            return StatusCode(response.responseCode, response);
        }


        [HttpGet("GetByNameYear")]
        [Authorize(Roles = "admin,root")]
        public async Task<IActionResult> GetSemesterByNameYear([FromQuery] GetSemesterModel getSemesterModel)
        {

            var response = await _semstersRepo.GetSemesterByNameYear(getSemesterModel);

            return StatusCode(response.responseCode, response);
        }

        [HttpPost("AddSemester")]
        [Authorize(Roles = "admin,root")]
        public async Task<IActionResult> AddSemester([FromBody] SemesterInputModel semesterInputModel)
        {
            var response = await _semstersRepo.AddSemester(semesterInputModel);

            return StatusCode(response.responseCode, response);
        }



        [HttpPut("EditStartDate")]
        [Authorize(Roles = "admin,root")]
        public async Task<IActionResult> EditStartDate([FromBody] EditDateModel editDateModel)
        {
            var response = await _semstersRepo.EditStartDateSemester(editDateModel);

            return StatusCode(response.responseCode, response);
        }

        [HttpPut("EditEndtDate")]
        [Authorize(Roles = "admin,root")]
        public async Task<IActionResult> EditEndtDate([FromBody] EditDateModel editDateModel)
        {
            var response = await _semstersRepo.EditEndDateSemester(editDateModel);

            return StatusCode(response.responseCode, response);
        }


        [HttpPut("EditActiveStatus")]
        [Authorize(Roles = "admin,root")]
        public async Task<IActionResult> EditActiveStatus([FromBody] EditIsActiveModel editIsActiveModel)
        {
            var response = await _semstersRepo.EditActiveStatus(editIsActiveModel);

            return StatusCode(response.responseCode, response);
        }


        [HttpDelete("DeleteSemester")]
        [Authorize(Roles = "root")]
        public async Task<IActionResult> DeleteSemester([FromQuery] int semesterID)
        {
            var response = await _semstersRepo.DeleteSemester(semesterID);

            return StatusCode(response.responseCode, response);
        }


        [HttpGet("{id}/courses")]
        [Authorize(Roles = "admin,root")]
        public async Task<IActionResult> GetCoursesBySemesterId(int id)
        {
            var response = await _coursesSemestersRepo.GetCoursesBySemester(id);

            return StatusCode(response.responseCode, response);
        }

        [HttpGet("{semesterId}/schedule")]
        [Authorize(Roles = "root,admin")]
        public async Task<IActionResult> GetScheduls(int semesterId, [FromQuery] TakeSkipModel takeSkipModel)
        {
            var response = await _schedulesRepo.GetScheduleBySemester(semesterId,takeSkipModel);

            return StatusCode(response.responseCode, response);

        }



        [HttpGet("{semesterId}/studentYear/{studentYear}/groups")]
        [Authorize(Roles = "root,admin")]
        public async Task<IActionResult> GetGroupsBySemesterId_StudentYear(int semesterId, int studentYear,[FromQuery] TakeSkipModel takeSkipModel)
        {
            var response = await _groupsRepo.GetGroupsBySemesterId_StudentYear(semesterId, studentYear,takeSkipModel);

            return StatusCode(response.responseCode, response);

        }

        [HttpGet("active/studentYear/{studentYear}/groups")]
        [Authorize(Roles = "root,admin")]
        public async Task<IActionResult> GetGroupsByActiveSemester_StudentYear(int studentYear, [FromQuery] TakeSkipModel takeSkipModel)
        {
            var response = await _groupsRepo.GetGroupsByActiveSemester_StudentYear(studentYear, takeSkipModel);

            return StatusCode(response.responseCode, response);

        }


        [HttpGet("active/courses")]
        [Authorize(Roles = "admin,root")]
        public async Task<IActionResult> GetActiveSemesterCourses()
        {
            var response = await _coursesSemestersRepo.GetActiveSemesterCourses();

            return StatusCode(response.responseCode, response);
        }

        [HttpPut("active/courses")]
        [Authorize(Roles = "admin,root")]
        public async Task<IActionResult> EditActiveCourses([FromBody] bool isActive)
        {
            var response = await _coursesSemestersRepo.EditActiveStatusForAllCourses(isActive);

            return StatusCode(response.responseCode, response);
        }


        [HttpDelete("{semesterId}/course/{courseId}")]
        [Authorize(Roles = "admin,root")]
        public async Task<IActionResult> Delete(int courseId, int semesterId) //test me
        {
            var response = await _coursesSemestersRepo.Delete(courseId, semesterId);

            return StatusCode(response.responseCode, response);
        }

        [HttpPut("{semesterId}/course/{courseId}/prof")]
        [Authorize(Roles = "admin,root")]
        public async Task<IActionResult> ChangeProfessor(int courseId, int semesterId, [FromBody] int profId) //test me
        {
            var response = await _coursesSemestersRepo.ChangeProfessor(courseId, semesterId, profId);

            return StatusCode(response.responseCode, response);
        }

        [HttpPut("{semesterId}/course/{courseId}/acitve")]
        [Authorize(Roles = "admin,root")]
        public async Task<IActionResult> EditActivationStatus(int courseId, int semesterId, bool isActive) //test me
        {
            var response = await _coursesSemestersRepo.EditActivationStatus(courseId, semesterId, isActive);

            return StatusCode(response.responseCode, response);
        }



        [HttpPost("{semesterId}/course/{courseId}/Ta")]
        [Authorize(Roles = "root,admin")]
        public async Task<IActionResult> AddTaToCourse(int courseId, int semesterId, [FromBody] int taId) //test me
        {
            var response = await _assistanceCoursesRepo.AddTaToCourse(courseId,  semesterId, taId);

            return StatusCode(response.responseCode, response);
        }

        [HttpDelete("{semesterId}/course/{courseId}/Ta/{taId}")]
        [Authorize(Roles = "root,admin")]
        public async Task<IActionResult> RemoveTaFromCourse(int courseId, int semesterId, int taId) //test me
        {
            var response = await _assistanceCoursesRepo.RemoveTaFromCourse(courseId, semesterId, taId);

            return StatusCode(response.responseCode, response);
        }

        [HttpGet("{semesterId}/course/{courseId}/Ta")]
        [Authorize(Roles = "root,admin")]
        public async Task<IActionResult> GetCourseTas(int courseId, int semesterId) //test me
        {
            var response = await _assistanceCoursesRepo.GetCourseTas(courseId, semesterId);

            return StatusCode(response.responseCode, response);
        }

    }
}
