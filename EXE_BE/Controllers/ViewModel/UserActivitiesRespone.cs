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
        public int PlasticUsageId { get; set; }
        public int TrafficUsageId { get; set; }
        public int FoodUsageId { get; set; }
        public int EnergyUsageId { get; set; }
        public PlasticUsageDto PlasticUsage { get; set; }
        public TrafficUsageDto TrafficUsage { get; set; }
        public FoodUsageDto FoodUsage { get; set; }
        public EnergyUsageDto EnergyUsage { get; set; }
    }

    public class PlasticUsageDto
    {
        public int Id { get; set; }
        public int ActivityId { get; set; }
        public DateTime Date { get; set; }
        public float CO2Emission { get; set; }
        public List<PlasticItemDto> PlasticItems { get; set; }
    }

    public class TrafficUsageDto
    {
        public int Id { get; set; }
        public int ActivityId { get; set; }
        public DateTime Date { get; set; }
        public float Distance { get; set; }
        public Traffic_category TrafficCategory { get; set; }
        public float CO2Emission { get; set; }
    }

    public class FoodUsageDto
    {
        public int Id { get; set; }
        public int ActivityId { get; set; }
        public DateTime Date { get; set; }
        public float CO2Emission { get; set; }
        public int Score { get; set; }
        public List<FoodItemDto> FoodItems { get; set; }
    }

    public class EnergyUsageDto
    {
        public int Id { get; set; }
        public int ActivityId { get; set; }
        public DateTime Date { get; set; }
        public float ElectricityConsumption { get; set; }
        public float CO2Emission { get; set; }
    }

    public class FoodItemDto
    {
        public int Id { get; set; }
        public food_category FoodCategory { get; set; }
        public float Weight { get; set; }
        public int FoodUsageId { get; set; }
    }

    public class PlasticItemDto
    {
        public int Id { get; set; }
        public plastic_category PlasticCategory { get; set; }
        public float Weight { get; set; }
        public int PlasticUsageId { get; set; }
    }

    // ✅ Unified Mapper (Entity ↔ DTO)

    public static class Mapper
    {
        // DTO to Entity mappings
        public static UserActivities ToEntity(this UserActivitiesDto dto)
        {
            if (dto == null) return null;

            return new UserActivities
            {
                Id = dto.Id,
                UserId = dto.UserId,
                Date = dto.Date,
                TotalCO2Emission = dto.TotalCO2Emission,
                PlasticUsageId = dto.PlasticUsageId,
                TrafficUsageId = dto.TrafficUsageId,
                FoodUsageId = dto.FoodUsageId,
                EnergyUsageId = dto.EnergyUsageId,
                PlasticUsage = dto.PlasticUsage?.ToEntity(),
                TrafficUsage = dto.TrafficUsage?.ToEntity(),
                FoodUsage = dto.FoodUsage?.ToEntity(),
                EnergyUsage = dto.EnergyUsage?.ToEntity()
            };
        }

        public static PlasticUsage ToEntity(this PlasticUsageDto dto)
        {
            if (dto == null) return null;

            return new PlasticUsage
            {
                Id = dto.Id,
                ActivityId = dto.ActivityId,
                date = dto.Date,
                CO2emission = dto.CO2Emission,
                PlasticItems = dto.PlasticItems?.Select(pi => pi.ToEntity()).ToList()
            };
        }

        public static TrafficUsage ToEntity(this TrafficUsageDto dto)
        {
            if (dto == null) return null;

            return new TrafficUsage
            {
                Id = dto.Id,
                ActivityId = dto.ActivityId,
                date = dto.Date,
                distance = dto.Distance,
                trafficCategory = dto.TrafficCategory,
                CO2emission = dto.CO2Emission
            };
        }

        public static FoodUsage ToEntity(this FoodUsageDto dto)
        {
            if (dto == null) return null;

            return new FoodUsage
            {
                Id = dto.Id,
                ActivityId = dto.ActivityId,
                date = dto.Date,
                CO2emission = dto.CO2Emission,
                score = dto.Score,
                FoodItems = dto.FoodItems?.Select(fi => fi.ToEntity()).ToList()
            };
        }

        public static EnergyUsage ToEntity(this EnergyUsageDto dto)
        {
            if (dto == null) return null;

            return new EnergyUsage
            {
                Id = dto.Id,
                ActivityId = dto.ActivityId,
                date = dto.Date,
                electricityconsumption = dto.ElectricityConsumption,
                CO2emission = dto.CO2Emission
            };
        }

        public static FoodItem ToEntity(this FoodItemDto dto)
        {
            if (dto == null) return null;

            return new FoodItem
            {
                Id = dto.Id,
                FoodCategory = dto.FoodCategory,
                Weight = dto.Weight,
                FoodUsageId = dto.FoodUsageId
            };
        }

        public static PlasticItem ToEntity(this PlasticItemDto dto)
        {
            if (dto == null) return null;

            return new PlasticItem
            {
                Id = dto.Id,
                PlasticCategory = dto.PlasticCategory,
                Weight = dto.Weight,
                PlasticUsageId = dto.PlasticUsageId
            };
        }

        // Batch conversion methods for DTO to Entity
        public static List<UserActivities> ToEntity(this IEnumerable<UserActivitiesDto> dtos)
        {
            return dtos?.Select(d => d.ToEntity()).ToList() ?? new List<UserActivities>();
        }

        public static List<PlasticUsage> ToEntity(this IEnumerable<PlasticUsageDto> dtos)
        {
            return dtos?.Select(d => d.ToEntity()).ToList() ?? new List<PlasticUsage>();
        }

        public static List<TrafficUsage> ToEntity(this IEnumerable<TrafficUsageDto> dtos)
        {
            return dtos?.Select(d => d.ToEntity()).ToList() ?? new List<TrafficUsage>();
        }

        public static List<FoodUsage> ToEntity(this IEnumerable<FoodUsageDto> dtos)
        {
            return dtos?.Select(d => d.ToEntity()).ToList() ?? new List<FoodUsage>();
        }

        public static List<EnergyUsage> ToEntity(this IEnumerable<EnergyUsageDto> dtos)
        {
            return dtos?.Select(d => d.ToEntity()).ToList() ?? new List<EnergyUsage>();
        }

        public static List<FoodItem> ToEntity(this IEnumerable<FoodItemDto> dtos)
        {
            return dtos?.Select(d => d.ToEntity()).ToList() ?? new List<FoodItem>();
        }

        public static List<PlasticItem> ToEntity(this IEnumerable<PlasticItemDto> dtos)
        {
            return dtos?.Select(d => d.ToEntity()).ToList() ?? new List<PlasticItem>();
        }

        // Existing Entity to DTO mappings (unchanged)
        public static UserActivitiesDto ToDto(this UserActivities entity)
        {
            if (entity == null) return null;

            return new UserActivitiesDto
            {
                Id = entity.Id,
                UserId = entity.UserId,
                Date = entity.Date,
                TotalCO2Emission = entity.TotalCO2Emission,
                PlasticUsageId = entity.PlasticUsageId,
                TrafficUsageId = entity.TrafficUsageId,
                FoodUsageId = entity.FoodUsageId,
                EnergyUsageId = entity.EnergyUsageId,
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
                ActivityId = entity.ActivityId,
                Date = entity.date,
                CO2Emission = entity.CO2emission,
                PlasticItems = entity.PlasticItems?.Select(pi => pi.ToDto()).ToList()
            };
        }

        public static TrafficUsageDto ToDto(this TrafficUsage entity)
        {
            if (entity == null) return null;

            return new TrafficUsageDto
            {
                Id = entity.Id,
                ActivityId = entity.ActivityId,
                Date = entity.date,
                Distance = entity.distance,
                TrafficCategory = entity.trafficCategory,
                CO2Emission = entity.CO2emission
            };
        }

        public static FoodUsageDto ToDto(this FoodUsage entity)
        {
            if (entity == null) return null;

            return new FoodUsageDto
            {
                Id = entity.Id,
                ActivityId = entity.ActivityId,
                Date = entity.date,
                CO2Emission = entity.CO2emission,
                Score = entity.score,
                FoodItems = entity.FoodItems?.Select(fi => fi.ToDto()).ToList()
            };
        }

        public static EnergyUsageDto ToDto(this EnergyUsage entity)
        {
            if (entity == null) return null;

            return new EnergyUsageDto
            {
                Id = entity.Id,
                ActivityId = entity.ActivityId,
                Date = entity.date,
                ElectricityConsumption = entity.electricityconsumption,
                CO2Emission = entity.CO2emission
            };
        }

        public static FoodItemDto ToDto(this FoodItem entity)
        {
            if (entity == null) return null;

            return new FoodItemDto
            {
                Id = entity.Id,
                FoodCategory = entity.FoodCategory,
                Weight = entity.Weight,
                FoodUsageId = entity.FoodUsageId
            };
        }

        public static PlasticItemDto ToDto(this PlasticItem entity)
        {
            if (entity == null) return null;

            return new PlasticItemDto
            {
                Id = entity.Id,
                PlasticCategory = entity.PlasticCategory,
                Weight = entity.Weight,
                PlasticUsageId = entity.PlasticUsageId
            };
        }

        public static List<UserActivitiesDto> ToDto(this IEnumerable<UserActivities> entities)
        {
            return entities?.Select(e => e.ToDto()).ToList() ?? new List<UserActivitiesDto>();
        }

        public static List<PlasticUsageDto> ToDto(this IEnumerable<PlasticUsage> entities)
        {
            return entities?.Select(e => e.ToDto()).ToList() ?? new List<PlasticUsageDto>();
        }

        public static List<TrafficUsageDto> ToDto(this IEnumerable<TrafficUsage> entities)
        {
            return entities?.Select(e => e.ToDto()).ToList() ?? new List<TrafficUsageDto>();
        }

        public static List<FoodUsageDto> ToDto(this IEnumerable<FoodUsage> entities)
        {
            return entities?.Select(e => e.ToDto()).ToList() ?? new List<FoodUsageDto>();
        }

        public static List<EnergyUsageDto> ToDto(this IEnumerable<EnergyUsage> entities)
        {
            return entities?.Select(e => e.ToDto()).ToList() ?? new List<EnergyUsageDto>();
        }

        public static List<FoodItemDto> ToDto(this IEnumerable<FoodItem> entities)
        {
            return entities?.Select(e => e.ToDto()).ToList() ?? new List<FoodItemDto>();
        }

        public static List<PlasticItemDto> ToDto(this IEnumerable<PlasticItem> entities)
        {
            return entities?.Select(e => e.ToDto()).ToList() ?? new List<PlasticItemDto>();
        }
    }
}
