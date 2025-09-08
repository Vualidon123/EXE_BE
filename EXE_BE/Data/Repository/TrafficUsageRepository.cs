using EXE_BE.Models;

namespace EXE_BE.Data.Repository
{
    public class TrafficUsageRepository
    {
        private readonly AppDbContext _context;
        public TrafficUsageRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<TrafficUsage> AddTrafficUsageAsync(TrafficUsage trafficUsage)
        {
            double co2PerKm;
            switch (trafficUsage.trafficCategory)
            {
                case Traffic_category.GasolineCar:
                    co2PerKm = 0.045;
                    break;
                case Traffic_category.Bus:
                    co2PerKm = 0.102;
                    break;
                case Traffic_category.Train:
                    co2PerKm = 0.041;
                    break;
                case Traffic_category.Bicycle:
                case Traffic_category.Walking:
                    co2PerKm = 0.0;
                    break;
                case Traffic_category.Plane:
                    co2PerKm = 0.225;
                    break;
                case Traffic_category.DieselCar:
                    co2PerKm = 0.23;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(trafficUsage.trafficCategory), "Unknown traffic category");
            }

            // Assuming you have a property like DistanceInKm and Emission in TrafficUsage
            trafficUsage.CO2emission = ((float)co2PerKm * trafficUsage.distance);
            _context.TrafficUsages.Add(trafficUsage);
            await _context.SaveChangesAsync();
            return trafficUsage;
        }
        public async Task<TrafficUsage?> GetTrafficUsageByIdAsync(int id)
        {
            return await _context.TrafficUsages.FindAsync(id);
        }
        public async Task UpdateTrafficUsageAsync(TrafficUsage trafficUsage)
        {
            _context.TrafficUsages.Update(trafficUsage);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteTrafficUsageAsync(int id)
        {
            var trafficUsage = await _context.TrafficUsages.FindAsync(id);
            if (trafficUsage != null)
            {
                _context.TrafficUsages.Remove(trafficUsage);
                await _context.SaveChangesAsync();
            }
        }
    }
}
