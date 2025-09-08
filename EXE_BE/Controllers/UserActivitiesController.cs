using EXE_BE.Models;
using EXE_BE.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EXE_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserActivitiesController : ControllerBase
    {
        private readonly UserActivitiesSerivce _userActivitiesService;

        public UserActivitiesController(UserActivitiesSerivce userActivitiesService)
        {
            _userActivitiesService = userActivitiesService;
        }
        [HttpPost]
        public async Task<IActionResult> AddUserActivities( [FromBody] UserActivities userActivities)
        {
            var result = await _userActivitiesService.AddUserActivitiesAsync(userActivities);
            return Ok(result);
        }
    }
}
