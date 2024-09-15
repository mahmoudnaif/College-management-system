using College_managemnt_system.ClientModels;
using College_managemnt_system.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace College_managemnt_system.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfessorController : Controller
    {
        private readonly IProfessorsRepo _professorsRepo;
        private readonly ICSVParser _CSVParser;

        public ProfessorController(IProfessorsRepo professorsRepo,ICSVParser CSVParser) {
            _professorsRepo = professorsRepo;
            _CSVParser = CSVParser;
        }

        [HttpGet("All")]
        [Authorize(Roles = "root,admin")]
        public async Task<IActionResult> GetAllProfessors([FromQuery] TakeSkipModel takeSkipModel)
        {
            var response = await _professorsRepo.GetAllProfessors(takeSkipModel);

            return StatusCode(response.responseCode,response);
        }

        [HttpPost]
        [Authorize(Roles ="root,admin")]
        public async Task<IActionResult> Add(ProfessorInputModel model)
        {
            var response = await _professorsRepo.Add(model);

            return StatusCode(response.responseCode, response);
        }

        [HttpDelete("{profId}")]
        [Authorize(Roles ="root,admin")]
        public async Task<IActionResult> Delete(int profId)
        {
            var response = await _professorsRepo.Delete(profId);

            return StatusCode(response.responseCode, response);
        }

        [HttpPut("{profId}/Name")]
        [Authorize(Roles = "root,admin")]
        public async Task<IActionResult> EditName(int profId, [FromBody] NameInputModel model)
        {
          
            var response = await _professorsRepo.EditName(profId,model);

            return StatusCode(response.responseCode, response);
        }

        [HttpPut("{profId}/hiringdate")]
        [Authorize(Roles = "root,admin")]
        public async Task<IActionResult> HiringDate(int profId,[FromBody] DateTime date)
        {
            
            
            var response = await _professorsRepo.EditHiringDate(profId,date);

            return StatusCode(response.responseCode, response);
        }

        [HttpPut("{profId}/phone")]
        [Authorize(Roles = "root,admin")]
        public async Task<IActionResult> Phone(int profId,[FromBody]string phoneNumber)
        {
            var response = await _professorsRepo.EditPhoneNumber(profId,phoneNumber);

            return StatusCode(response.responseCode, response);
        }

        [HttpPut("{profId}/department")]
        [Authorize(Roles = "root,admin")]
        public async Task<IActionResult> Department(int profId,[FromBody]int departmentId)
        {
            var response = await _professorsRepo.EditDepartment(profId,departmentId);

            return StatusCode(response.responseCode, response);
        }

        [HttpPost("CSV")]
        [Authorize(Roles = "root,admin")]
        public async Task<IActionResult> AddProfessorsCSV(IFormFile file)
        {
            var response = await _CSVParser.AddProfessors(file);

            return StatusCode(response.responseCode, response);
        }

        [HttpGet("nationalId/{nationalId}")]
        [Authorize(Roles = "root,admin")]
        public async Task<IActionResult> GetProfByNationalId(string nationalId)
        {
            var response = await _professorsRepo.GetProfByNationalId(nationalId);

            return StatusCode(response.responseCode, response);
        }

        [HttpGet("search/{searchQuery}")]
        [Authorize(Roles = "root,admin")]
        public async Task<IActionResult> SearchProfessors(string searchQuery, [FromQuery] TakeSkipModel takeSkipModel)
        {
            var response = await _professorsRepo.SearchProfessors(searchQuery,takeSkipModel);

            return StatusCode(response.responseCode, response);
        }
    }
}
