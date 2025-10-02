using EXE_BE.Models;
using EXE_BE.Models.ItemList;

namespace EXE_BE.Controllers.ViewModel
{
  
        public class UserActivitiesDto
        {
            public int Id { get; set; }
            public int UserId { get; set; }
            public DateTime Date { get; set; }
            public float TotalCO2Emission { get; set; }

            public PlasticUsageDto? PlasticUsage { get; set; }
            public TrafficUsageDto? TrafficUsage { get; set; }
            public FoodUsageDto? FoodUsage { get; set; }
            public EnergyUsageDto? EnergyUsage { get; set; }
        }

        public class PlasticUsageDto
        {
            public int Id { get; set; }
            public DateTime Date { get; set; }
            public float CO2Emission { get; set; }
            public List<PlasticItemDto>? PlasticItems { get; set; }
        }

        public class PlasticItemDto
        {
            public int Id { get; set; }
            public string PlasticCategory { get; set; } // Convert enum to string for readability
            public float Weight { get; set; }
        }

        public class FoodUsageDto
        {
            public int Id { get; set; }
            public DateTime Date { get; set; }
            public float CO2Emission { get; set; }
            public int Score { get; set; }
            public List<FoodItemDto>? FoodItems { get; set; }
        }

        public class FoodItemDto
        {
            public int Id { get; set; }
            public string FoodCategory { get; set; } // enum converted to string
            public float Weight { get; set; }
        }

        public class EnergyUsageDto
        {
            public int Id { get; set; }
            public DateTime Date { get; set; }
            public float ElectricityConsumption { get; set; }
            public float CO2Emission { get; set; }
        }

        // Assuming you have a TrafficUsage entity
        public class TrafficUsageDto
        {
            public int Id { get; set; }
            public DateTime Date { get; set; }
            public float CO2Emission { get; set; }
            
        }

    public static class UserActivitiesMapper
    {
        public static UserActivitiesDto ToDto(this UserActivities entity)
        {
            if (entity == null) return null;

            return new UserActivitiesDto
            {
                Id = entity.Id,
                UserId = entity.UserId,
                Date = entity.Date,
                TotalCO2Emission = entity.TotalCO2Emission,
                PlasticUsage = entity.PlasticUsage?.ToDto(),
                TrafficUsage = entity.TrafficUsage?.ToDto(),
                FoodUsage = entity.FoodUsage?.ToDto(),
                EnergyUsage = entity.EnergyUsage?.ToDto()
            };
        }

        public static PlasticUsageDto ToDto(this PlasticUsage entity)
        {
            if (entity == null) return null;

            return new PlasticUsageDto
            {
                Id = entity.Id,
                Date = entity.date,
                CO2Emission = entity.CO2emission,
                PlasticItems = entity.PlasticItems?
                    .Select(p => p.ToDto())
                    .ToList()
            };
        }

        public static PlasticItemDto ToDto(this PlasticItem entity)
        {
            if (entity == null) return null;

            return new PlasticItemDto
            {
                Id = entity.Id,
                PlasticCategory = entity.PlasticCategory.ToString(),
                Weight = entity.Weight
            };
        }

        public static FoodUsageDto ToDto(this FoodUsage entity)
        {
            if (entity == null) return null;

            return new FoodUsageDto
            {
                Id = entity.Id,
                Date = entity.date,
                CO2Emission = entity.CO2emission,
                Score = entity.score,
                FoodItems = entity.FoodItems?
                    .Select(f => f.ToDto())
                    .ToList()
            };
        }

        public static FoodItemDto ToDto(this FoodItem entity)
        {
            if (entity == null) return null;

            return new FoodItemDto
            {
                Id = entity.Id,
                FoodCategory = entity.FoodCategory.ToString(),
                Weight = entity.Weight
            };
        }

        public static EnergyUsageDto ToDto(this EnergyUsage entity)
        {
            if (entity == null) return null;

            return new EnergyUsageDto
            {
                Id = entity.Id,
                Date = entity.date,
                ElectricityConsumption = entity.electricityconsumption,
                CO2Emission = entity.CO2emission
            };
        }

        public static TrafficUsageDto ToDto(this TrafficUsage entity)
        {
            if (entity == null) return null;

            return new TrafficUsageDto
            {
                Id = entity.Id,
                Date = entity.date,
                CO2Emission = entity.CO2emission,
            };
        }
    }

}
