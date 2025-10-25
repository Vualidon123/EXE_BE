using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace EXE_BE.Models
{

    public class Leaderboard
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(20)]
        public string Type { get; set; } // "Daily", "Weekly", "Monthly"

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Required]
        public string LeaderboardData { get; set; } // JSON string

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Index for faster queries
        public bool IsActive { get; set; } = true;
    }
    public enum LeaderboardType
    {
        Daily,
        Weekly,
        Monthly
    }

    public static class LeaderboardConstants
    {
        public const int TOP_USERS_COUNT = 10;
        public const int WEEKLY_DAYS = 7;
        public const int MONTHLY_DAYS = 30;
    }
}
