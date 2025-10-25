using EXE_BE.Services;
using Microsoft.AspNetCore.Mvc;

namespace EXE_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeaderboardController : ControllerBase
    {
        private readonly LeaderboardService _leaderboardService;
        private readonly ILogger<LeaderboardController> _logger;

        public LeaderboardController(
            LeaderboardService leaderboardService,
            ILogger<LeaderboardController> logger)
        {
            _leaderboardService = leaderboardService;
            _logger = logger;
        }

        /// <summary>
        /// Get current leaderboard (real-time calculation)
        /// </summary>
        /// <param name="type">daily, weekly, or monthly</param>
        [HttpGet("current/{type}")]
   
        public async Task<IActionResult> GetCurrentLeaderboard(string type)
        {
            try
            {
                if (!IsValidType(type))
                {
                    return BadRequest(new { error = "Invalid type. Use 'daily', 'weekly', or 'monthly'" });
                }

                var leaderboard = await _leaderboardService.GetCurrentLeaderboardAsync(type);

                return Ok(new
                {
                    type = type,
                    timestamp = DateTime.UtcNow,
                    data = leaderboard
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting current {type} leaderboard");
                return StatusCode(500, new { error = "Internal server error" });
            }
        }

        /// <summary>
        /// Get the latest saved leaderboard snapshot
        /// </summary>
        /// <param name="type">daily, weekly, or monthly</param>
        [HttpGet("snapshot/{type}")]
  
        public async Task<IActionResult> GetLatestSnapshot(string type)
        {
            try
            {
                if (!IsValidType(type))
                {
                    return BadRequest(new { error = "Invalid type. Use 'daily', 'weekly', or 'monthly'" });
                }

                var snapshot = await _leaderboardService.GetLatestSnapshotAsync(type);

                if (snapshot == null)
                {
                    return NotFound(new { error = $"No {type} leaderboard snapshot found" });
                }

                return Ok(snapshot);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting {type} snapshot");
                return StatusCode(500, new { error = "Internal server error" });
            }
        }

        /// <summary>
        /// Save current leaderboard as a snapshot
        /// </summary>
        /// <param name="type">daily, weekly, or monthly</param>
        [HttpPost("snapshot/{type}")]
        
        public async Task<IActionResult> SaveLeaderboardSnapshot(string type)
        {
            try
            {
                if (!IsValidType(type))
                {
                    return BadRequest(new { error = "Invalid type. Use 'daily', 'weekly', or 'monthly'" });
                }

                await _leaderboardService.SaveLeaderboardSnapshotAsync(type);

                return Ok(new
                {
                    message = $"{type} leaderboard snapshot saved successfully",
                    timestamp = DateTime.UtcNow
                });
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, $"No data to save for {type} leaderboard");
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error saving {type} snapshot");
                return StatusCode(500, new { error = "Internal server error" });
            }
        }

        /// <summary>
        /// Clean up old leaderboard snapshots
        /// </summary>
        /// <param name="daysToKeep">Number of days to keep (default: 90)</param>
        [HttpDelete("cleanup")]
       
        public async Task<IActionResult> CleanupOldLeaderboards([FromQuery] int daysToKeep = 90)
        {
            try
            {
                if (daysToKeep < 1)
                {
                    return BadRequest(new { error = "daysToKeep must be greater than 0" });
                }

                var result = await _leaderboardService.CleanupOldLeaderboardsAsync(daysToKeep);

                return Ok(new
                {
                    message = result
                        ? $"Successfully cleaned up leaderboards older than {daysToKeep} days"
                        : "No old leaderboards to clean up",
                    deleted = result
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error cleaning up leaderboards");
                return StatusCode(500, new { error = "Internal server error" });
            }
        }

        private bool IsValidType(string type)
        {
            var validTypes = new[] { "daily", "weekly", "monthly" };
            return validTypes.Contains(type.ToLower());
        }
    }
}