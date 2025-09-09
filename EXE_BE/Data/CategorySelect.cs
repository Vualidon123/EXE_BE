using EXE_BE.Models;
using EXE_BE.Models.ItemList;

namespace EXE_BE.Data
{
    public class CategorySelect
    {
       /* public float CalculateTotalCO2Emission(UserActivities activities)
        {
            float totalCO2 = 0;
            if (activities.PlasticUsage != null)
            {
                totalCO2 += activities.PlasticUsage.CO2emission;
            }
            if (activities.TrafficUsage != null)
            {
                totalCO2 += activities.TrafficUsage.CO2emission;
            }
            if (activities.FoodUsage != null)
            {
                totalCO2 += activities.FoodUsage.CO2emission;
            }
            if (activities.EnergyUsage != null)
            {
                totalCO2 += activities.EnergyUsage.CO2emission;
            }
            return totalCO2;
        }*/
        public float FoodCO2Emission(FoodItem fooditem)
        {
            float co2PerKg;
            switch (fooditem.FoodCategory)
            {
                case food_category.Beef:
                    co2PerKg = 27.0f;
                    break;
                case food_category.Lamb:
                    co2PerKg = 39.2f;
                    break;
                case food_category.Pork:
                    co2PerKg = 12.1f;
                    break;
                case food_category.Chicken:
                    co2PerKg = 6.9f;
                    break;
                case food_category.Fish:
                    co2PerKg = 6.1f;
                    break;
                case food_category.Eggs:
                    co2PerKg = 4.8f;
                    break;
                case food_category.Rice:
                    co2PerKg = 4.0f;
                    break;
                case food_category.Vegetables:
                    co2PerKg = 2.0f;
                    break;
                case food_category.Others:
                    co2PerKg = 5.0f; // Average value for other food items
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(fooditem), "Unknown food category");
            }
            return co2PerKg;
        }
      /*  public float EnergyCO2Emission(UserActivities activities)
        {
            float totalCO2 = 0;
            if (activities.PlasticUsage != null)
            {
                totalCO2 += activities.PlasticUsage.CO2emission;
            }
            if (activities.TrafficUsage != null)
            {
                totalCO2 += activities.TrafficUsage.CO2emission;
            }
            if (activities.FoodUsage != null)
            {
                totalCO2 += activities.FoodUsage.CO2emission;
            }
            if (activities.EnergyUsage != null)
            {
                totalCO2 += activities.EnergyUsage.CO2emission;
            }
            return totalCO2;
        }*/
        public float TrafficCO2Emission(TrafficUsage trafficUsage)
        {
            float co2PerKm;
            switch (trafficUsage.trafficCategory)
            {
                case Traffic_category.GasolineCar:
                    co2PerKm = 0.045f;
                    break;
                case Traffic_category.Bus:
                    co2PerKm = 0.102f;
                    break;
                case Traffic_category.Train:
                    co2PerKm = 0.041f;
                    break;
                case Traffic_category.Bicycle:
                case Traffic_category.Walking:
                    co2PerKm = 0.0f;
                    break;
                case Traffic_category.Plane:
                    co2PerKm = 0.225f;
                    break;
                case Traffic_category.DieselCar:
                    co2PerKm = 0.23f;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(trafficUsage.trafficCategory), "Unknown traffic category");
            }
            return co2PerKm;
        }
        public float PlasticCO2Emission(PlasticItem item)
        {
            float co2PerKg;
            switch (item.PlasticCategory)
            {
                case plastic_category.PlasticBottle:
                    co2PerKg = 1.5f;
                    break;
                case plastic_category.PlasticBag:
                    co2PerKg = 3.0f;
                    break;
                case plastic_category.PlasticCup:
                    co2PerKg = 2.5f;
                    break;
                case plastic_category.PlasticStraw:
                    co2PerKg = 4.0f;
                    break;
                case plastic_category.PlasticContainer:
                    co2PerKg = 2.0f;
                    break;
                case plastic_category.Other:
                    co2PerKg = 3.0f; // Average value for other plastic items
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(item), "Unknown plastic category");
            }
            return co2PerKg;
        }
    }
}
