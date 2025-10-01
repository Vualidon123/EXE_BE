using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EXE_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecommendController : ControllerBase
    {
        private readonly Services.RecommendService _recommendService;
        public RecommendController(Services.RecommendService recommendService)
        {
            _recommendService = recommendService;
        }
        [HttpGet("{userActivityId}")]
        public async Task<IActionResult> GetRecommendation(int userActivityId)
        {
            var recommendation = await _recommendService.GetRecommend(userActivityId);
            return Ok(new { recommendation });
        }
    }
}
