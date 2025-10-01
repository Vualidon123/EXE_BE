using System.ComponentModel.DataAnnotations;

namespace EXE_BE.Controllers.ViewModel
{
    public class UpdateUserRequest
    {
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Username must be between 3 and 50 characters")]
        public string? UserName { get; set; }

        [Phone(ErrorMessage = "Invalid phone number format")]
        public string? PhoneNumber { get; set; }

        public DateOnly? DateOfBirth { get; set; }

        // Email update might be sensitive, so we could add it separately
        // [EmailAddress(ErrorMessage = "Invalid email format")]
        // public string? Email { get; set; }
    }
}