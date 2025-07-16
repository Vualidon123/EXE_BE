namespace EXE_BE.Models
{
    public class Challenge
    {
        public int Id {get;set;}
        public string Name { get; set; } = ""; // Name of the challenge
        public string Description { get; set; } = ""; // Description of the challenge
        public DateTime StartDate { get; set; } // Start date of the challenge
        public DateTime EndDate { get; set; } // End date of the challenge
        public bool isComplete { get; set; } // Indicates if the challenge is complete
    }
}
