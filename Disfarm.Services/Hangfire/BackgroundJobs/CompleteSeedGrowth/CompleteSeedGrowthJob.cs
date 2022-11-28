using System;
using System.Threading.Tasks;
using Disfarm.Data;
using Disfarm.Data.Enums;
using Disfarm.Data.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Disfarm.Services.Hangfire.BackgroundJobs.CompleteSeedGrowth
{
    public class CompleteSeedGrowthJob : ICompleteSeedGrowthJob
    {
        private readonly ILogger<CompleteSeedGrowthJob> _logger;
        private readonly IServiceScopeFactory _scopeFactory;

        public CompleteSeedGrowthJob(
            IServiceScopeFactory scopeFactory,
            ILogger<CompleteSeedGrowthJob> logger)
        {
            _logger = logger;
            _scopeFactory = scopeFactory;
        }

        public async Task Execute(Guid userFarmId)
        {
            using var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var entity = await db.UserFarms.SingleOrDefaultAsync(x => x.Id == userFarmId);

            if (entity.State is FieldState.Empty)
            {
                throw new Exception(
                    $"Cannot complete seed growth since user {entity.UserId} farm {entity.Number} state is empty");
            }

            entity.State = FieldState.Completed;
            entity.BeenGrowingFor = null;
            entity.CompleteAt = null;
            entity.UpdatedAt = DateTimeOffset.UtcNow;

            await db.UpdateEntity(entity);

            _logger.LogInformation(
                "Completed seed growth for user {UserId} farm {Number}",
                entity.UserId, entity.Number);
        }
    }
}