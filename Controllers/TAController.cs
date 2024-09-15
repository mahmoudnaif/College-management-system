using College_managemnt_system.ClientModels;
using College_managemnt_system.Interfaces;
using College_managemnt_system.Repos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MimeKit.Encodings;

namespace College_managemnt_system.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TAController : Controller
    {
        private readonly ITeachingAssistanceRepo _teachingAssistanceRepo;
        private readonly IAssistanceCoursesRepo _assistanceCoursesRepo;

        public TAController(ITeachingAssistanceRepo teachingAssistanceRepo, IAssistanceCoursesRepo assistanceCoursesRepo)
        {
            _teachingAssistanceRepo = teachingAssistanceRepo;
            _assistanceCoursesRepo = assistanceCoursesRepo;
        }

        [HttpPost]
        [Authorize(Roles = "admin,root")]

        public async Task<IActionResult> AddTa([FromBody] TeachingAssistanceInputModel teachingAssistanceInputModel)
        {
            var response = await _teachingAssistanceRepo.AddTa(teachingAssistanceInputModel);

            return StatusCode(response.responseCode,response);

        }

        [HttpDelete("{taId}")]
        [Authorize(Roles = "admin,root")]

        public async Task<IActionResult> RemoveTa(int taId)
        {
            var response = await _teachingAssistanceRepo.RemoveTa(taId);

            return StatusCode(response.responseCode, response);

        }

        [HttpPut("{taId}/name")]
        [Authorize(Roles = "admin,root")]

        public async Task<IActionResult> EditName(int taId,[FromBody] NameInputModel nameInputModel)
        {
            var response = await _teachingAssistanceRepo.EditName(taId,nameInputModel);

            return StatusCode(response.responseCode, response);

        }

        [HttpPut("{taId}/phone")]
        [Authorize(Roles = "admin,root")]

        public async Task<IActionResult> EditPhone(int taId, [FromBody]string phone)
        {
            var response = await _teachingAssistanceRepo.EditPhone(taId, phone);

            return StatusCode(response.responseCode, response);

        }

        [HttpPut("{taId}/hirignDate")]
        [Authorize(Roles = "admin,root")]

        public async Task<IActionResult> EditHiringDate(int taId, [FromBody] DateTime hiringDate)
        {
            var response = await _teachingAssistanceRepo.EditHiringDate(taId, hiringDate);

            return StatusCode(response.responseCode, response);

        }

        [HttpGet("All")]
        [Authorize(Roles = "root,admin")]
        public async Task<IActionResult> GetAllTas([FromQuery] TakeSkipModel takeSkipModel)
        {
            var response = await _teachingAssistanceRepo.GetAllTas(takeSkipModel);

            return StatusCode(response.responseCode,response);
        }

        [HttpGet("search/{searchQuery}")]
        [Authorize(Roles = "root,admin")]
        public async Task<IActionResult> SearchTas(string searchQuery,[FromQuery] TakeSkipModel takeSkipModel)
        {
            var response = await _teachingAssistanceRepo.SearchTas(searchQuery,takeSkipModel);

            return StatusCode(response.responseCode, response);
        }

        [HttpGet("nationalId/{nationalId}")]
        [Authorize(Roles = "root,admin")]
        public async Task<IActionResult> GetTaByNationalId(string nationalId)
        {
            var response = await _teachingAssistanceRepo.GetTaByNationalId(nationalId);

            return StatusCode(response.responseCode, response);
        }

        [HttpGet("{taId}/semester/{semesterId}")]
        [Authorize(Roles = "root,admin")]
        public async Task<IActionResult> GetTaByNationalId(int semesterId, int taId)
        {
            var response = await _assistanceCoursesRepo.getTaCourses(semesterId, taId);

            return StatusCode(response.responseCode, response);
        }
    }
}
