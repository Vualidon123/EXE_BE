namespace EXE_BE.Models
{
    public class UserActivities
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime Date { get; set; }
        public int Score { get; set; }
        public float TotalCO2Emission { get; set; }
        public int PlasticUsageId { get; set; }
        public int TrafficUsageId { get; set; }
        public int FoodUsageId { get; set; }
        public int EnergyUsageId { get; set; } 
        public virtual PlasticUsage PlasticUsage { get; set; }
        public virtual TrafficUsage TrafficUsage { get; set; }
        public virtual FoodUsage FoodUsage { get; set; }
        public virtual EnergyUsage EnergyUsage { get; set; }

        public virtual User User { get; set; }
    }
}
