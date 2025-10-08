using EXE_BE.Controllers.ViewModel;
using EXE_BE.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EXE_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            var result = await _userService.RegisterAsync(
                request.Username,
                request.Email,
                request.Password,
                request.PhoneNumber,
                request.DateOfBirth);

            if (!result.Success)
            {
                return BadRequest(new { Message = result.Message });
            }

            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var result = await _userService.LoginAsync(request.Email, request.Password);

            if (!result.Success)
            {
                return BadRequest(new { result.Message });
            }

            return Ok(result);
        }

        [Authorize]
        [HttpGet("me")]
        public async Task<IActionResult> GetCurrentUser()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var result = await _userService.GetUserByIdAsync(userId);
            
            if (!result.Success)
            {
                return NotFound(new { Message = result.Message });
            }

            return Ok(result);
        }

        [Authorize(Roles = "Admin, Staff")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var result = await _userService.GetUserByIdAsync(id);
            
            if (!result.Success)
            {
                return NotFound(new { Message = result.Message });
            }

            return Ok(result);
        }

        [Authorize(Roles = "Admin, Staff")]
        [HttpGet]
        public async Task<IActionResult> GetAllUsers([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var result = await _userService.GetUsersPagedAsync(page, pageSize);
            return Ok(result);
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UpdateUserRequest request)
        {
            // Users can only update their own profile unless they're admin
            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
            
            if (currentUserId != id && userRole != "Admin")
            {
                return Forbid("You can only update your own profile");
            }

            var result = await _userService.UpdateUserAsync(id, request);
            
            if (!result.Success)
            {
                return BadRequest(new { Message = result.Message });
            }

            return Ok(result);
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            // Only admins can delete users, or users can delete their own account
            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
            
            if (currentUserId != id && userRole != "Admin")
            {
                return Forbid();
            }

            var result = await _userService.DeleteUserAsync(id);
            
            if (!result.Success)
            {
                return BadRequest(new { Message = result.Message });
            }

            return Ok(result);
        }

        [Authorize]
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var result = await _userService.ChangePasswordAsync(userId, request.CurrentPassword, request.NewPassword);
            
            if (!result.Success)
            {
                return BadRequest(new { Message = result.Message });
            }

            return Ok(result);
        }

        [Authorize]
        [HttpPost("upgrade")]
        public async Task<IActionResult> UpgradeAccount([FromBody] UpgradeRequest request)
        {
            request.UserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

            var result = await _userService.UpgradeAccount(request);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}/role")]
        public async Task<IActionResult> UpdateUserRole(int id, [FromBody] UpdateUserRoleRequest request)
        {
            var result = await _userService.UpdateUserRoleAsync(id, request.Role);
            
            if (!result.Success)
            {
                return BadRequest(new { Message = result.Message });
            }

            return Ok(result);
        }

    }
}
