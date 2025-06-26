namespace EXE_BE.Models
{
    public class User
    {
        
        public int Id { get; set; }                // Primary key
        public string UserName { get; set; } = ""; // Username
        public string Email { get; set; } = "";    // Email address
        public string PasswordHash { get; set; } = ""; // Hashed password
    }
}
