using EXE_BE.Controllers.ViewModel;
using EXE_BE.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace EXE_BE.Data.Repository
{
    public class LeaderboardRepository 
    {
        private readonly AppDbContext _context;

        public LeaderboardRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Leaderboard>> GetAllLeaderboardsAsync()
        {
            return await _context.Leaderboards
                .OrderByDescending(l => l.CreatedAt)
                .ToListAsync();
        }

        public async Task<Leaderboard?> GetLatestLeaderboardAsync(string type)
        {
            return await _context.Leaderboards
                .Where(l => l.Type.ToLower() == type.ToLower() && l.IsActive)
                .OrderByDescending(l => l.CreatedAt)
                .FirstOrDefaultAsync();
        }

        public async Task<Leaderboard?> GetLeaderboardByDateAsync(DateTime date, string type)
        {
            return await _context.Leaderboards
                .Where(l => l.Type.ToLower() == type.ToLower()
                    && l.StartDate.Date <= date.Date
                    && l.EndDate.Date >= date.Date)
                .OrderByDescending(l => l.CreatedAt)
                .FirstOrDefaultAsync();
        }

        public async Task<List<Leaderboard>> GetLeaderboardsByDateRangeAsync(
            DateTime startDate,
            DateTime endDate,
            string type)
        {
            return await _context.Leaderboards
                .Where(l => l.Type.ToLower() == type.ToLower()
                    && l.StartDate >= startDate
                    && l.EndDate <= endDate)
                .OrderByDescending(l => l.CreatedAt)
                .ToListAsync();
        }

        public async Task<List<UserLeaderboardDto>> CalculateLeaderboardAsync(
            string type,
            DateTime? customStartDate = null)
        {
            var (startDate, endDate) = GetDateRange(type, customStartDate);

            var users = await _context.Users
                .Include(u => u.UserActivities.Where(a => a.Date >= startDate && a.Date <= endDate))
                .Where(u => u.UserActivities.Any(a => a.Date >= startDate && a.Date <= endDate))
                .ToListAsync();

            var leaderboard = users
                .Select(u => new UserLeaderboardDto
                {
                    UserName = u.UserName,
                    TotalCO2Emission = u.UserActivities
                        .Where(a => a.Date >= startDate && a.Date <= endDate)
                        .Sum(a => a.TotalCO2Emission)
                })
                .OrderBy(x => x.TotalCO2Emission)
                .Take(LeaderboardConstants.TOP_USERS_COUNT)
                .ToList();

            return leaderboard;
        }

        public async Task SaveLeaderboardSnapshotAsync(string type)
        {
            var leaderboardData = await CalculateLeaderboardAsync(type);

            if (leaderboardData == null || !leaderboardData.Any())
            {
                throw new InvalidOperationException($"No data available for {type} leaderboard");
            }

            var leaderboardJson = JsonSerializer.Serialize(leaderboardData);
            var (startDate, endDate) = GetDateRange(type);

            var leaderboard = new Leaderboard
            {
                Type = NormalizeType(type),
                StartDate = startDate,
                EndDate = endDate,
                LeaderboardData = leaderboardJson,
                IsActive = true
            };

            _context.Leaderboards.Add(leaderboard);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteOldLeaderboardsAsync(int daysToKeep = 90)
        {
            var cutoffDate = DateTime.UtcNow.AddDays(-daysToKeep);

            var oldLeaderboards = await _context.Leaderboards
                .Where(l => l.CreatedAt < cutoffDate)
                .ToListAsync();

            if (oldLeaderboards.Any())
            {
                _context.Leaderboards.RemoveRange(oldLeaderboards);
                await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }

        private (DateTime startDate, DateTime endDate) GetDateRange(string type, DateTime? customStartDate = null)
        {
            DateTime endDate = DateTime.UtcNow;
            DateTime startDate;

            if (customStartDate.HasValue)
            {
                startDate = customStartDate.Value;
            }
            else
            {
                startDate = type.ToLower() switch
                {
                    "daily" => DateTime.UtcNow.Date,
                    "weekly" => DateTime.UtcNow.AddDays(-LeaderboardConstants.WEEKLY_DAYS),
                    "monthly" => DateTime.UtcNow.AddDays(-LeaderboardConstants.MONTHLY_DAYS),
                    _ => throw new ArgumentException($"Invalid leaderboard type: {type}")
                };
            }

            return (startDate, endDate);
        }

        private string NormalizeType(string type)
        {
            return type.ToLower() switch
            {
                "daily" => "Daily",
                "weekly" => "Weekly",
                "monthly" => "Monthly",
                _ => throw new ArgumentException($"Invalid leaderboard type: {type}")
            };
        }
    }

}
