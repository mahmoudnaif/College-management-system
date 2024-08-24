using College_managemnt_system.ClientModels;
using College_managemnt_system.Interfaces;
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

        public SemesterController(ISemstersRepo semstersRepo, ICoursesSemestersRepo coursesSemestersRepo, ISchedulesRepo schedulesRepo)
        {
            _semstersRepo = semstersRepo;
            _coursesSemestersRepo = coursesSemestersRepo;
            _schedulesRepo = schedulesRepo;
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
            var response = await _schedulesRepo.GetScheduls(semesterId,takeSkipModel);

            return StatusCode(response.responseCode, response);

        }
    }
}
