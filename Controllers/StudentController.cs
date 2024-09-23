using College_managemnt_system.ClientModels;
using College_managemnt_system.CustomResponse;
using College_managemnt_system.DTOS;
using College_managemnt_system.Interfaces;
using College_managemnt_system.models;
using College_managemnt_system.Repos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;

namespace College_managemnt_system.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : Controller 
    {
        private readonly IStudentRepo _studentRepo;
        private readonly IStudetnsDepartmentsRepo _studetnsDepartmentsRepo;
        private readonly ICSVParser _CSVParser;
        private readonly PremissionUtilsRepo _premissionUtilsRepo;

        public StudentController(IStudentRepo studentRepo,IStudetnsDepartmentsRepo studetnsDepartmentsRepo, ICSVParser CSVParser,PremissionUtilsRepo premissionUtilsRepo)
        {
            _studentRepo = studentRepo;
            _studetnsDepartmentsRepo = studetnsDepartmentsRepo;
            _CSVParser = CSVParser;
            _premissionUtilsRepo = premissionUtilsRepo;
        }

        [HttpGet("year/{year}")]
        [Authorize(Roles = "root,admin")]
        public async Task<IActionResult> GetStudentsByYear(int year, [FromQuery] TakeSkipModel takeSkipModel)
        {
            var result = await _studentRepo.GetStudentSByYear(year, takeSkipModel);

            return StatusCode(result.responseCode, result);
        }

        [HttpGet("email/{email}")]
        [Authorize(Roles = "root,admin")]
        public async Task<IActionResult> GetStudentByEmail(string email)
        {
            var result = await _studentRepo.GetStudentByEmail(email);

            return StatusCode(result.responseCode, result);
        }

        [HttpGet("search/{searchQuery}")]
        [Authorize(Roles = "root,admin")]
        public async Task<IActionResult> SearchStudents(string searchQuery, [FromQuery] TakeSkipModel takeSkipModel)
        {
            var result = await _studentRepo.SearchStudents(searchQuery, takeSkipModel);

            return StatusCode(result.responseCode, result);
        }

        [HttpPost]
        [Authorize(Roles = "root,admin")]
        public async Task<IActionResult> Add([FromBody] StudentInputModel model)
        {
            if (!await _premissionUtilsRepo.CheckRegestringStudetns())
                return StatusCode(403, new CustomResponse<StudentDTO>(403,"Premission denied"));

           var result = await _studentRepo.Add(model);

            return StatusCode(result.responseCode, result);
        }

        [HttpDelete("{studentId}")]
        [Authorize(Roles = "root,admin")]
        public async Task<IActionResult> Delete(int studentId)
        {
            var result = await _studentRepo.Delete(studentId);

            return StatusCode(result.responseCode, result);
        }

        [HttpPut("{studentId}/name")]
        [Authorize(Roles = "root,admin")]
        public async Task<IActionResult> EditName(int studentId, [FromBody] FullNameInputModel model)
        {
            var result = await _studentRepo.EditName(studentId,model);

            return StatusCode(result.responseCode, result);
        }
        
        [HttpPut("{studentId}/phone")]
        [Authorize(Roles = "root,admin")]
        public async Task<IActionResult> EditPhone(int studentId, [FromBody] string phoneNumber)
        {
            var result = await _studentRepo.EditPhone(studentId,phoneNumber);

            return StatusCode(result.responseCode, result);
        }


        [HttpPut("All/gpa_totalhours")]
        [Authorize(Roles = "root")]
        public async Task<IActionResult> calculateALLStudentsCGPA_totalHours()
        {
            var result = await _studentRepo.calculateALLStudentsCGPA_totalHours();

            return StatusCode(result.responseCode, result);
        }


        [HttpPut("{studentId}/gpa_totalhours")]
        [Authorize(Roles = "root")]
        public async Task<IActionResult> Upda(int studentId)
        {
            var result = await _studentRepo.calculateStudentCGPA_totalHours(studentId);

            return StatusCode(result.responseCode, result);
        }

        [HttpPost("{studentId}/department")]
        [Authorize(Roles ="root,admin")]
        public async Task<IActionResult> AddDepartment(int studentId,[FromBody] int departmentId)
        {
            var response = await _studetnsDepartmentsRepo.AddStudentDepartment(studentId, departmentId);

            return StatusCode(response.responseCode, response);

        }

        [HttpPut("{studentId}/department")]
        [Authorize(Roles = "root")]
        public async Task<IActionResult> EditDepartment(int studentId, [FromBody] int departmentId)
        {
            var response = await _studetnsDepartmentsRepo.changeStudentDepartment(studentId, departmentId);

            return StatusCode(response.responseCode, response);
        }

        [HttpPost("CSV")]
        [Authorize(Roles = "root,admin")]
        public async Task<IActionResult> AddStudetnsCSV(IFormFile file)
        {
            if (!await _premissionUtilsRepo.CheckRegestringStudetns())
            return StatusCode(403, new CustomResponse<List<StudentErrorSheet>>(403, "Premission denied"));

            var response = await _CSVParser.AddStudents(file);
            return StatusCode(response.responseCode, response);
        }

        [HttpGet("nationalId/{nationalId}")]
        [Authorize(Roles = "root,admin")]
        public async Task<IActionResult> GetStudentByNationalId(string nationalId)
        {
            var result = await _studentRepo.GetStudentByNationalId(nationalId);

            return StatusCode(result.responseCode, result);
        }
    }
}
