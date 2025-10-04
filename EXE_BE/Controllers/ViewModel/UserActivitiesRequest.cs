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
    public static class UserActivitiesInputMapper
    {
        public static PlasticUsage ToEntity(this PlasticUsageInputModel input) =>
            input == null ? null : new PlasticUsage
            {
                PlasticItems = input.PlasticItems?
                    .Select(p => new PlasticItem
                    {
                        PlasticCategory = p.PlasticCategory,
                        Weight = p.Weight
                    }).ToList()
            };

        public static FoodUsage ToEntity(this FoodUsageInputModel input) =>
            input == null ? null : new FoodUsage
            {
                FoodItems = input.FoodItems?
                    .Select(f => new FoodItem
                    {
                        FoodCategory = f.FoodCategory,
                        Weight = f.Weight
                    }).ToList()
            };

        public static TrafficUsage ToEntity(this TrafficUsageInputModel input) =>
            input == null ? null : new TrafficUsage
            {
                trafficCategory = input.TrafficCategory,
                distance = input.Distance
            };

        public static EnergyUsage ToEntity(this EnergyUsageInputModel input) =>
            input == null ? null : new EnergyUsage
            {
                electricityconsumption = input.ElectricityConsumption
            };
    }
}
