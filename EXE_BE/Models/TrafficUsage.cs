using System.Text.Json.Serialization;

namespace EXE_BE.Models
{
    public class TrafficUsage
    {
            public int Id { get; set; }
            public int ActivityId { get; set; }
            public DateTime date { get; set; }
            public float distance { get; set; }
            public Traffic_category trafficCategory { get; set; }
            [JsonIgnore]
            public virtual UserActivities? UserActivities { get; set; }
            public float CO2emission { get; set; }
        

    }
    public enum Traffic_category
    {
        GasolineCar = 1,//0.045 kg CO2 per km
        Bus = 2,//0.102 kg CO2 per km
        Train = 3,//0.041 kg CO2 per km
        Bicycle = 4,//0 kg CO2 per km
        Walking = 5,//0 kg CO2 per km
        Plane = 6,//0.225 kg CO2 per km
        DieselCar = 7,//0.23 kg CO2 per km
    }

}
