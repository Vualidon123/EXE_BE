using EXE_BE.Models;

namespace EXE_BE.Controllers.ViewModel
{
    public class ChalengeRequest
    {
        public string Name { get; set; } = ""; // Name of the challenge
        public string Description { get; set; } = ""; // Description of the challenge
        public DateTime StartDate { get; set; } // Start date of the challenge
    }
}
