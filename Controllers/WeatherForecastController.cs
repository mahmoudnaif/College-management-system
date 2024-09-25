using College_managemnt_system.ClientModels;
using College_managemnt_system.Interfaces;
using College_managemnt_system.models;
using College_managemnt_system.Repos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace College_managemnt_system.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly CollegeDBContext _context;
        private readonly IStudentSchedulesRepo _studentSchedulesRepo;

        public WeatherForecastController(CollegeDBContext context, IStudentSchedulesRepo studentSchedulesRepo)
        {
            _context = context;
            _studentSchedulesRepo = studentSchedulesRepo;
        }

        [HttpGet("GetWeatherForecast/{studentId}")]
        public async Task<IActionResult> Get(int studentId,[FromQuery]TakeSkipModel takeSkipModel)
        {
            var response = await _studentSchedulesRepo.GetStudentActiveSchedule(studentId);

            return StatusCode(response.responseCode, response);
        }
    }
}
