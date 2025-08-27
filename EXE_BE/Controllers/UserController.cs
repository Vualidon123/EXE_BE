using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using EXE_BE.Services;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using EXE_BE.Controllers.ViewModel;

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
                request.PhoneNumber);

            if (!result.Success)
            {
                return BadRequest(new { Message = result.Message });
            }

            return Ok(new 
            { 
                Message = result.Message,
                User = new
                {
                    result.User!.Id,
                    result.User.UserName,
                    result.User.Email,
                    result.User.PhoneNumber
                }
            });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var result = await _userService.LoginAsync(request.Email, request.Password);

            if (!result.Success)
            {
                return BadRequest(new { Message = result.Message });
            }

            return Ok(new
            {
                Message = result.Message,
                Token = result.Data!.Token,
                User = new
                {
                    result.Data.User.Id,
                    result.Data.User.UserName,
                    result.Data.User.Email,
                    result.Data.User.PhoneNumber
                }
            });
        }

        [Authorize]
        [HttpGet("me")]
        public IActionResult GetCurrentUser()
        {
            return Ok(new { Message = "Authenticated user", Username = User.Identity?.Name });
        }
    }
}
