using EXE_BE.Models;
using EXE_BE.Models.ItemList;

namespace EXE_BE.Controllers.ViewModel
{
    public class UserActivitiesInputModel
    {
       
       
        
        public PlasticUsageInputModel PlasticUsage { get; set; }
        public TrafficUsageInputModel TrafficUsage { get; set; }
        public FoodUsageInputModel FoodUsage { get; set; }
        public EnergyUsageInputModel EnergyUsage { get; set; }
    }
    public class PlasticUsageInputModel
    {
        
        
        public List<PlasticItemInputModel> PlasticItems { get; set; }
    }

    public class PlasticItemInputModel
    {
        public plastic_category PlasticCategory { get; set; }
        public float Weight { get; set; }
    }
    public class TrafficUsageInputModel
    {
        
        public float Distance { get; set; }
        public Traffic_category TrafficCategory { get; set; }
        
    }
    public class FoodUsageInputModel
    {
        
        public List<FoodItemInputModel> FoodItems { get; set; }
    }

    public class FoodItemInputModel
    {
        public food_category FoodCategory { get; set; }
        public float Weight { get; set; }
    }
    public class EnergyUsageInputModel
    {
        
        public float ElectricityConsumption { get; set; }
        
    }
}
