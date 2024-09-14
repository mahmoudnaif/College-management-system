using College_managemnt_system.ClientModels;
using College_managemnt_system.Interfaces;
using College_managemnt_system.models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace College_managemnt_system.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly CollegeDBContext _context;
        private readonly ICSVParser _CSVParser;

        public WeatherForecastController(CollegeDBContext context, ICSVParser CSVParser)
        {
            _context = context;
            _CSVParser = CSVParser;
        }

        [HttpPost(Name = "GetWeatherForecast")]
        public async Task<IActionResult> Get(IFormFile file)
        {
            var response = await _CSVParser.AddProfessors(file);

            return StatusCode(response.responseCode,response);
        }
    }
}
