using System.Threading.Tasks;
using Disfarm.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Disfarm.Services.Hangfire.BackgroundJobs.ResetDailyRewards
{
    public class ResetDailyRewardJob : IResetDailyRewardJob
    {
        private readonly ILogger<ResetDailyRewardJob> _logger;

        private readonly IServiceScopeFactory _scopeFactory;

        public ResetDailyRewardJob(
            IServiceScopeFactory scopeFactory,
            ILogger<ResetDailyRewardJob> logger)
        {
            _logger = logger;
            _scopeFactory = scopeFactory;
        }

        public async Task Execute()
        {
            _logger.LogInformation("Reset daily reward job executed");

            using var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            await db.Database.ExecuteSqlRawAsync("delete from user_daily_rewards;");
        }
    }
}