using College_managemnt_system.ClientModels;
using College_managemnt_system.models;
using Microsoft.AspNetCore.Mvc;

namespace College_managemnt_system.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly CollegeDBContext _context;

        public WeatherForecastController(CollegeDBContext context)
        {
            _context = context;
        }

        [HttpPost(Name = "GetWeatherForecast")]
        public IActionResult Get([FromBody]SemesterInputModel semesterInputModel)
        {
            StudentCourse studentCourse = new StudentCourse()
            {
                StudentId = 1,
                CourseSemesterId = 6,
                IsFinished = false,
                Grade = "Z"
            };
            _context.StudentCourses.Add(studentCourse);
            try
            {
                _context.SaveChanges();
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
