using EXE_BE.Controllers.ViewModel;
using EXE_BE.Data;
using EXE_BE.Data.Repository;
using EXE_BE.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace EXE_BE.Services
{
    

    public class LeaderboardService 
    {
        private readonly LeaderboardRepository _repository;
        private readonly ILogger<LeaderboardService> _logger;

        public LeaderboardService(
            LeaderboardRepository repository,
            ILogger<LeaderboardService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<List<UserLeaderboardDto>> GetCurrentLeaderboardAsync(string type)
        {
            try
            {
                return await _repository.CalculateLeaderboardAsync(type);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting current {type} leaderboard");
                throw;
            }
        }

        public async Task<LeaderboardSnapshotDto?> GetLatestSnapshotAsync(string type)
        {
            try
            {
                var snapshot = await _repository.GetLatestLeaderboardAsync(type);

                if (snapshot == null)
                {
                    return null;
                }

                var leaderboardData = JsonSerializer.Deserialize<List<UserLeaderboardDto>>(
                    snapshot.LeaderboardData);

                return new LeaderboardSnapshotDto
                {
                    Type = snapshot.Type,
                    StartDate = snapshot.StartDate,
                    EndDate = snapshot.EndDate,
                    CreatedAt = snapshot.CreatedAt,
                    Data = leaderboardData ?? new List<UserLeaderboardDto>()
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting latest {type} snapshot");
                throw;
            }
        }

        public async Task SaveLeaderboardSnapshotAsync(string type)
        {
            try
            {
                await _repository.SaveLeaderboardSnapshotAsync(type);
                _logger.LogInformation($"Successfully saved {type} leaderboard snapshot");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error saving {type} leaderboard snapshot");
                throw;
            }
        }

        public async Task<bool> CleanupOldLeaderboardsAsync(int daysToKeep = 90)
        {
            try
            {
                var result = await _repository.DeleteOldLeaderboardsAsync(daysToKeep);

                if (result)
                {
                    _logger.LogInformation($"Successfully cleaned up leaderboards older than {daysToKeep} days");
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error cleaning up old leaderboards");
                throw;
            }
        }
    }

    // DTOs
    public class LeaderboardSnapshotDto
    {
        public string Type { get; set; } = "";
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<UserLeaderboardDto> Data { get; set; } = new();
    }
}
