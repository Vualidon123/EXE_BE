using System.ComponentModel.DataAnnotations;

namespace EXE_BE.Controllers.ViewModel
{
    public class RegisterRequest
    {
        [Required]
        public string Username { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MinLength(6)]
        public string Password { get; set; } = string.Empty;

        [Required]
        public DateOnly DateOfBirth { get; set; }

        public string? PhoneNumber { get; set; }
    }
}
