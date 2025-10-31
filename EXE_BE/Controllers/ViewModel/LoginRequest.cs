using System.ComponentModel.DataAnnotations;

namespace EXE_BE.Controllers.ViewModel
{
    public class LoginRequest
    {
        [Required]
        [EmailAddress]
        public string Username { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;
    }
    public class GoogleRequest
    {
        [Required]
        public string IdToken { get; set; }
    }
}
