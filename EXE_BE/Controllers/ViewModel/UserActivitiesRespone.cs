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
        public string PlasticCategory { get; set; } // enum as string
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
        public string FoodCategory { get; set; } // enum as string
        public float Weight { get; set; }
    }

    public class EnergyUsageDto
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public float ElectricityConsumption { get; set; }
        public float CO2Emission { get; set; }
    }

    public class TrafficUsageDto
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public float CO2Emission { get; set; }
    }

    // ✅ Unified Mapper (Entity ↔ DTO)
    public static class UserActivitiesMapper
    {
        // ---------------------- ToDto ----------------------
        public static UserActivitiesDto ToDto(this UserActivities entity) =>
            entity == null ? null : new UserActivitiesDto
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

        public static PlasticUsageDto ToDto(this PlasticUsage entity) =>
            entity == null ? null : new PlasticUsageDto
            {
                Id = entity.Id,
                Date = entity.date,
                CO2Emission = entity.CO2emission,
                PlasticItems = entity.PlasticItems?.Select(p => p.ToDto()).ToList()
            };

        public static PlasticItemDto ToDto(this PlasticItem entity) =>
            entity == null ? null : new PlasticItemDto
            {
                Id = entity.Id,
                PlasticCategory = entity.PlasticCategory.ToString(),
                Weight = entity.Weight
            };

        public static FoodUsageDto ToDto(this FoodUsage entity) =>
            entity == null ? null : new FoodUsageDto
            {
                Id = entity.Id,
                Date = entity.date,
                CO2Emission = entity.CO2emission,
                Score = entity.score,
                FoodItems = entity.FoodItems?.Select(f => f.ToDto()).ToList()
            };

        public static FoodItemDto ToDto(this FoodItem entity) =>
            entity == null ? null : new FoodItemDto
            {
                Id = entity.Id,
                FoodCategory = entity.FoodCategory.ToString(),
                Weight = entity.Weight
            };

        public static EnergyUsageDto ToDto(this EnergyUsage entity) =>
            entity == null ? null : new EnergyUsageDto
            {
                Id = entity.Id,
                Date = entity.date,
                ElectricityConsumption = entity.electricityconsumption,
                CO2Emission = entity.CO2emission
            };

        public static TrafficUsageDto ToDto(this TrafficUsage entity) =>
            entity == null ? null : new TrafficUsageDto
            {
                Id = entity.Id,
                Date = entity.date,
                CO2Emission = entity.CO2emission
            };

        // ---------------------- ToEntity ----------------------
        public static UserActivities ToEntity(this UserActivitiesDto dto) =>
            dto == null ? null : new UserActivities
            {
                Id = dto.Id,
                UserId = dto.UserId,
                Date = dto.Date,
                TotalCO2Emission = dto.TotalCO2Emission,
                PlasticUsage = dto.PlasticUsage?.ToEntity(),
                TrafficUsage = dto.TrafficUsage?.ToEntity(),
                FoodUsage = dto.FoodUsage?.ToEntity(),
                EnergyUsage = dto.EnergyUsage?.ToEntity()
            };

        public static PlasticUsage ToEntity(this PlasticUsageDto dto) =>
            dto == null ? null : new PlasticUsage
            {
                Id = dto.Id,
                date = dto.Date,
                CO2emission = dto.CO2Emission,
                PlasticItems = dto.PlasticItems?.Select(p => p.ToEntity()).ToList()
            };

        public static PlasticItem ToEntity(this PlasticItemDto dto) =>
            dto == null ? null : new PlasticItem
            {
                Id = dto.Id,
                PlasticCategory = Enum.Parse<plastic_category>(dto.PlasticCategory),
                Weight = dto.Weight
            };

        public static FoodUsage ToEntity(this FoodUsageDto dto) =>
            dto == null ? null : new FoodUsage
            {
                Id = dto.Id,
                date = dto.Date,
                CO2emission = dto.CO2Emission,
                score = dto.Score,
                FoodItems = dto.FoodItems?.Select(f => f.ToEntity()).ToList()
            };

        public static FoodItem ToEntity(this FoodItemDto dto) =>
            dto == null ? null : new FoodItem
            {
                Id = dto.Id,
                FoodCategory = Enum.Parse<food_category>(dto.FoodCategory),
                Weight = dto.Weight
            };

        public static EnergyUsage ToEntity(this EnergyUsageDto dto) =>
            dto == null ? null : new EnergyUsage
            {
                Id = dto.Id,
                date = dto.Date,
                electricityconsumption = dto.ElectricityConsumption,
                CO2emission = dto.CO2Emission
            };

        public static TrafficUsage ToEntity(this TrafficUsageDto dto) =>
            dto == null ? null : new TrafficUsage
            {
                Id = dto.Id,
                date = dto.Date,
                CO2emission = dto.CO2Emission
            };
    }
}
