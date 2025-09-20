namespace EXE_BE.Models
{
    public class ChallengeProgress
    {
        public int Id {get;set;}
        public int UserId { get; set; } // Foreign key to User
        public int ChallengeId { get; set; } // Foreign key to Challenge
        public float Progress { get; set; } // Name of the challenge
        public string Description { get; set; } = ""; // Description of the challenge
        public DateTime FinishDate { get; set; } // Start date of the challenge
        public bool isComplete { get; set; } // Indicates if the challenge is complete
        public int Score { get; set; }
        public virtual User? User { get; set; } // Navigation property to User
        public virtual Challenge? Challenge { get; set; } // Navigation property to Challenge
    }
}
