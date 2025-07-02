namespace EXE_BE.Models
{
    public class TrafficUsage
    {

        
            public int Id { get; set; }
            public int userId { get; set; }
            public DateTime date { get; set; }
            public float distance { get; set; }
            public Traffic_category trafficCategory { get; set; }
            public int score { get; set; }
            public int duration { get; set; } // Duration in minutes

    }
    public enum Traffic_category
    {
        Car = 1,
        Bus = 2,
        Train = 3,
        Bicycle = 4,
        Walking = 5,
        Other = 6
    }

}
