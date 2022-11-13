using System;
using System.Threading.Tasks;
using Disfarm.Data;
using Disfarm.Data.Enums;
using Disfarm.Data.Extensions;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Disfarm.Services.Hangfire.BackgroundJobs.CheckSeedWatered
{
    public class CheckSeedWateredJob : ICheckSeedWateredJob
    {
        private readonly ILogger<CheckSeedWateredJob> _logger;
        private readonly IServiceScopeFactory _scopeFactory;

        public CheckSeedWateredJob(
            IServiceScopeFactory scopeFactory,
            ILogger<CheckSeedWateredJob> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        public async Task Execute(long userId, Guid userFarmId, string completeSeedGrowthJobId)
        {
            using var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var entity = await db.UserFarms.SingleOrDefaultAsync(x => x.Id == userFarmId);

            switch (entity.State)
            {
                case FieldState.Empty:
                {
                    throw new Exception(
                        $"Cannot check seed watered since user {userId} farm {entity.Number} state is empty");
                }
                case FieldState.Completed:
                {
                    _logger.LogInformation(
                        "Checked seed watered for user {UserId} farm {Number} but state is already completed",
                        userId, entity.Number);
                    break;
                }
                case FieldState.Planted:
                {
                    BackgroundJob.Delete(completeSeedGrowthJobId);

                    entity.BeenGrowingFor =
                        entity.BeenGrowingFor?.Add(TimeSpan.FromHours(24)) ?? TimeSpan.FromHours(24);

                    await db.UpdateEntity(entity);

                    _logger.LogInformation(
                        "Checked seed watered for user {UserId} farm {Number} and state is planted, " +
                        "deleted complete seed growth job and added 24h growth time to entity",
                        userId, entity.Number);
                    break;
                }
                case FieldState.Watered:
                {
                    _logger.LogInformation(
                        "Check seed watered for user {UserId} farm {Number} and state is watered",
                        userId, entity.Number);

                    BackgroundJob.Schedule<ICheckSeedWateredJob>(x =>
                            x.Execute(userId, userFarmId, completeSeedGrowthJobId),
                        TimeSpan.FromHours(24));
                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}