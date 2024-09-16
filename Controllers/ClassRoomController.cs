using College_managemnt_system.ClientModels;
using College_managemnt_system.Interfaces;
using College_managemnt_system.models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace College_managemnt_system.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClassRoomController : Controller
    {
        private readonly IClassroomsRepo _classroomsRepo;
        private readonly ICSVParser _CSVParser;

        public ClassRoomController(IClassroomsRepo classroomsRepo,ICSVParser CSVParser)
        {
            _classroomsRepo = classroomsRepo;
            _CSVParser = CSVParser;
        }



        [HttpGet("All")]
        [Authorize(Roles = "root,admin")]
        public async Task<IActionResult> GetAllClassRooms([FromQuery]TakeSkipModel takeSkipModel) { 
            var response = await _classroomsRepo.GetAllClassRooms(takeSkipModel);

            return StatusCode(response.responseCode, response);
        }

        [HttpGet("Search")]
        [Authorize(Roles = "root,admin")]
        public async Task<IActionResult> SearchClassRooms([FromQuery] SearchModel searchModel)
        {
            var response = await _classroomsRepo.SearchClassRooms(searchModel);

            return StatusCode(response.responseCode, response);
        }

        [HttpPost()]
        [Authorize(Roles = "root,admin")]
        public async Task<IActionResult> AddClassRooms([FromBody]ClassRoomInputModel classRoomInputModel)
        {
            var response = await _classroomsRepo.AddClassRoom(classRoomInputModel);

            return StatusCode(response.responseCode, response);
        }

        [HttpPut("{classRoomId}/Capacity")]
        [Authorize(Roles = "root,admin")]
        public async Task<IActionResult> EditClassRoomCapacity(int classRoomId, [FromBody]int capacity)
        {
            var response = await _classroomsRepo.EditClassRoomCapacity(classRoomId, capacity);

            return StatusCode(response.responseCode, response);
        }

        [HttpDelete("{classRoomId}")]
        [Authorize(Roles = "root,admin")]
        public async Task<IActionResult> RemoveClassRoom(int classRoomId)
        {
            var response = await _classroomsRepo.RemoveClassRoom(classRoomId);

            return StatusCode(response.responseCode, response);
        }

        [HttpPost("CSV")]
        [Authorize(Roles = "root,admin")]
        public async Task<IActionResult> AddClassRooms(IFormFile file)
        {
            var response = await _CSVParser.AddClassRooms(file);

            return StatusCode(response.responseCode, response);
        }
    }
}
