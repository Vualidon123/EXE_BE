namespace EXE_BE.Models
{
    public class User
    {

        public int Id { get; set; }                // Primary key
        public string UserName { get; set; } = ""; // Username
        public string Email { get; set; } = "";    // Email address
        public string PasswordHash { get; set; } = ""; // Hashed password
        public string? PhoneNumber { get; set; } // Phone number
        public user_role Role { get; set; }  // User role (Admin, User, Staff)

        public subscription_type SubscriptionType { get; set; } // Subscription type (Free, Vip)
        public virtual List<UserActivities>? UserActivities { get; set; }
    }
    public enum user_role
    {
        Admin=1,
        User=2,
        Staff=3
    }
    public enum subscription_type
    {
        Free=1,
        Vip=2
        
    }
}
