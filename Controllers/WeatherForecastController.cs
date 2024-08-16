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
            Semester semesterExists = _context.Semesters.FirstOrDefault(S => S.SemesterName == semesterInputModel.semesterName.ToUpper() && S.SemesterYear == semesterInputModel.semesterYear);
            return Ok(semesterExists);
        }
    }
}
