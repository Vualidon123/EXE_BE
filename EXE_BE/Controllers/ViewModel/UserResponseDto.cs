using EXE_BE.Models;

namespace EXE_BE.Controllers.ViewModel
{
    public class UserResponseDto
    {
        public int Id { get; set; }
        public string UserName { get; set; } = "";
        public string Email { get; set; } = "";
        public string? PhoneNumber { get; set; }
        public user_role Role { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public subscription_type SubscriptionType { get; set; }

        public static UserResponseDto FromUser(User user)
        {
            return new UserResponseDto
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Role = user.Role,
                DateOfBirth = user.DateOfBirth,
                SubscriptionType = user.SubscriptionType
            };
        }
    }
}