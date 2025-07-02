namespace EXE_BE.Models
{
    public class UserActivities
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime Date { get; set; }
        public int Score { get; set; }

        public virtual PlasticUsage PlasticUsage { get; set; }
        public virtual TrafficUsage TrafficUsage { get; set; }
        public virtual FoodUsage FoodUsage { get; set; }
    }
}
