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
        private readonly IStudentCoursesRepo _studentCoursesRepo;

        public WeatherForecastController(CollegeDBContext context, IStudentCoursesRepo studentCoursesRepo)
        {
            _context = context;
            _studentCoursesRepo = studentCoursesRepo;
        }

        [HttpGet("GetWeatherForecast/{studentId}")]
        public async Task<IActionResult> Get(int studentId,[FromQuery]TakeSkipModel takeSkipModel)
        {
            var response = await _studentCoursesRepo.GetActiveStudentCourses(studentId);

            return StatusCode(response.responseCode, response);
        }
    }
}
