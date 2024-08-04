using College_managemnt_system.ClientModels;
using College_managemnt_system.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace College_managemnt_system.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupController : Controller
    {
        private readonly IGroupsRepo _groupsRepo;

        public GroupController(IGroupsRepo groupsRepo)
        {
            _groupsRepo = groupsRepo;
        }

        [HttpPost]
        [Authorize(Roles = "admin,root")]
        public async Task<IActionResult> AddGroup([FromBody]GroupInputModel groupInputModel)
        {
            var result = await _groupsRepo.AddGroup(groupInputModel);

            return StatusCode(result.responseCode, result);
        }


        [HttpDelete]
        [Authorize(Roles = "root")]
        public async Task<IActionResult> DeleteGroup([FromBody]int groupId)
        {
            var result = await _groupsRepo.DeleteGroup(groupId);

            return StatusCode(result.responseCode, result);
        }
    }
}
