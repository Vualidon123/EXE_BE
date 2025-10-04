using EXE_BE.Controllers.ViewModel;
using EXE_BE.Models;
using EXE_BE.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Security.Claims;

namespace EXE_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotifyController : ControllerBase
    {
        private readonly NotifyService _notifyService;

        public NotifyController(NotifyService notifyService)
        {
            _notifyService = notifyService;
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> CreateNotification([FromBody] NotifyRequest request)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            {
                return Unauthorized(new { Message = "Invalid user token" });
            }

            var result = await _notifyService.CreateNotificationAsync(request, userId);

            if (!result.Success)
            {
                return BadRequest(new { Message = result.Message });
            }

            return Ok(result);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> GetNotificationById(int id)
        {
            var result = await _notifyService.GetNotificationByIdAsync(id);

            if (!result.Success)
            {
                return NotFound(new { Message = result.Message });
            }

            return Ok(result);
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> GetAllNotifications()
        {
            var result = await _notifyService.GetAllNotificationsAsync();
            return Ok(result);
        }

        [HttpGet("active")]
        [AllowAnonymous] // Public endpoint for users to see active notifications
        public async Task<IActionResult> GetActiveNotifications()
        {
            var result = await _notifyService.GetActiveNotificationsAsync();
            return Ok(result);
        }

        [HttpGet("reason/{reason}")]
        [AllowAnonymous] // Public endpoint to filter by reason
        public async Task<IActionResult> GetNotificationsByReason(NotifyReason reason)
        {
            var result = await _notifyService.GetNotificationsByReasonAsync(reason);
            return Ok(result);
        }

        [HttpGet("paged")]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> GetPagedNotifications([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var result = await _notifyService.GetPagedNotificationsAsync(page, pageSize);

            if (!result.Success)
            {
                return BadRequest(new { Message = result.Message });
            }

            return Ok(result);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> UpdateNotification(int id, [FromBody] NotifyUpdateRequest request)
        {
            var result = await _notifyService.UpdateNotificationAsync(id, request);

            if (!result.Success)
            {
                return BadRequest(new { Message = result.Message });
            }

            return Ok(result);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteNotification(int id)
        {
            var result = await _notifyService.DeleteNotificationAsync(id);

            if (!result.Success)
            {
                return BadRequest(new { Message = result.Message });
            }

            return Ok(result);
        }

        [HttpPatch("{id}/deactivate")]
        [Authorize(Roles = "Admin,Staff")] // Both admin and staff can deactivate
        public async Task<IActionResult> DeactivateNotification(int id)
        {
            var result = await _notifyService.SoftDeleteNotificationAsync(id);

            if (!result.Success)
            {
                return BadRequest(new { Message = result.Message });
            }

            return Ok(result);
        }

        [HttpGet("reasons")]
        [AllowAnonymous] // Public endpoint to get available reasons
        public IActionResult GetNotifyReasons()
        {
            var reasons = Enum.GetValues<NotifyReason>()
                .Select(r => new { 
                    Value = (int)r, 
                    Name = r.ToString(),
                    DisplayName = GetReasonDisplayName(r)
                })
                .ToList();

            return Ok(new { Success = true, Data = reasons });
        }

        private string GetReasonDisplayName(NotifyReason reason)
        {
            return reason switch
            {
                NotifyReason.General => "General",
                NotifyReason.SystemMaintenance => "System Maintenance",
                NotifyReason.FeatureUpdate => "Feature Update",
                NotifyReason.SecurityAlert => "Security Alert",
                NotifyReason.PromotionalOffer => "Promotional Offer",
                NotifyReason.PolicyUpdate => "Policy Update",
                NotifyReason.UserWarning => "User Warning",
                NotifyReason.ChallengeAnnouncement => "Challenge Announcement",
                NotifyReason.EventNotification => "Event Notification",
                NotifyReason.SystemAlert => "System Alert",
                _ => "Unknown"
            };
        }
    }
}