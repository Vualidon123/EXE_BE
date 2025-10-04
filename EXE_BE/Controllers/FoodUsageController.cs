using EXE_BE.Controllers.ViewModel;
using EXE_BE.Models;
using EXE_BE.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EXE_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FoodUsageController : ControllerBase
    {
        private readonly FoodUsageService _foodUsageService;
        public FoodUsageController(FoodUsageService foodUsageService)
        {
            _foodUsageService = foodUsageService;
        }
        [HttpGet]
        public async Task<IActionResult> GetFoodUsageById(int id)
        {
            var foodUsage = await _foodUsageService.GetFoodUsageByIdAsync(id);
            if (foodUsage == null)
            {
                return NotFound();
            }
            return Ok(foodUsage);
        }
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetFoodUsageByUserId(int userId)
        {
            var foodUsages = await _foodUsageService.GetFoodUsageByUserId(userId);
            return Ok(foodUsages);
        }
        [HttpPost]
        public async Task<IActionResult> AddFoodUsage([FromBody] FoodUsageDto request)
        {
            var foodUsage = request.ToEntity();
            var createdFoodUsage = await _foodUsageService.AddFoodUsageAsync(foodUsage);
            return CreatedAtAction(nameof(GetFoodUsageById), new { id = createdFoodUsage.Id }, createdFoodUsage);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateFoodUsage(int id, [FromBody] FoodUsageDto request)
        {
            if (id != request.Id)
            {
                return BadRequest();
            }
            var foodUsage= request.ToEntity();
            await _foodUsageService.UpdateFoodUsageAsync(foodUsage);
            return NoContent();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFoodUsage(int id)
        {
            await _foodUsageService.DeleteFoodUsageAsync(id);
            return NoContent();
        }
    }
}
