using EXE_BE.Services;

namespace EXE_BE.BackgroundServices
{
    public class LeaderboardBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<LeaderboardBackgroundService> _logger;
        private readonly TimeSpan _dailyInterval = TimeSpan.FromHours(24);
        private readonly TimeSpan _weeklyInterval = TimeSpan.FromDays(7);

        public LeaderboardBackgroundService(
            IServiceProvider serviceProvider,
            ILogger<LeaderboardBackgroundService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Leaderboard Background Service started");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await SaveDailyLeaderboardIfNeeded(stoppingToken);
                    await SaveWeeklyLeaderboardIfNeeded(stoppingToken);
                    await CleanupOldLeaderboards(stoppingToken);

                    // Check every minute (thay vì 1 giờ) để đảm bảo 
                    // không bỏ lỡ cửa sổ 10 phút và tránh lãng phí CPU
                    await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
                }
                catch (OperationCanceledException)
                {
                    _logger.LogInformation("Leaderboard Background Service is stopping.");
                    break; // Thoát vòng lặp khi nhận tín hiệu dừng
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error in Leaderboard Background Service");
                    // Đợi 5 phút nếu có lỗi trước khi thử lại
                    await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
                }
            }
        }

        private async Task SaveDailyLeaderboardIfNeeded(CancellationToken cancellationToken)
        {
            var now = DateTime.UtcNow;

            // Save daily leaderboard at midnight (trong 10 phút đầu)
            if (now.Hour == 0 && now.Minute < 10)
            {
                using var scope = _serviceProvider.CreateScope();
                var service = scope.ServiceProvider.GetRequiredService<LeaderboardService>();

                await service.SaveLeaderboardSnapshotAsync("daily");
                _logger.LogInformation("Daily leaderboard snapshot saved");
            }
        }

        private async Task SaveWeeklyLeaderboardIfNeeded(CancellationToken cancellationToken)
        {
            var now = DateTime.UtcNow;

            // Save weekly leaderboard on Sunday at midnight (trong 10 phút đầu)
            if (now.DayOfWeek == DayOfWeek.Sunday && now.Hour == 0 && now.Minute < 10)
            {
                using var scope = _serviceProvider.CreateScope();
                var service = scope.ServiceProvider.GetRequiredService<LeaderboardService>();

                await service.SaveLeaderboardSnapshotAsync("weekly");
                _logger.LogInformation("Weekly leaderboard snapshot saved");
            }
        }

        private async Task CleanupOldLeaderboards(CancellationToken cancellationToken)
        {
            var now = DateTime.UtcNow;

            // Cleanup on first day of month at 1 AM (trong 10 phút đầu)
            if (now.Day == 1 && now.Hour == 1 && now.Minute < 10)
            {
                using var scope = _serviceProvider.CreateScope();
                var service = scope.ServiceProvider.GetRequiredService<LeaderboardService>();

                await service.CleanupOldLeaderboardsAsync(90);
                _logger.LogInformation("Old leaderboards cleaned up");
            }
        }
    }
}