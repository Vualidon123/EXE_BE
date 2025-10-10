using System.Text.Json.Serialization;

namespace EXE_BE.Models
{
    public class EnergyUsage
    {

        public int Id { get; set; }
        public int ActivityId { get; set; }
        public DateTime date { get; set; }
        public float electricityconsumption { get; set; }
        public float CO2emission { get; set; }
       
        public virtual UserActivities? UserActivities { get; set; }
    }
}
