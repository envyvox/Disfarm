using System;
using System.Threading.Tasks;
using Disfarm.Data;
using Disfarm.Data.Enums;
using Disfarm.Data.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Disfarm.Services.Hangfire.BackgroundJobs.CompleteSeedGrowth
{
    public class CompleteSeedGrowthJob : ICompleteSeedGrowthJob
    {
        private readonly ILogger<CompleteSeedGrowthJob> _logger;
        private readonly AppDbContext _db;

        public CompleteSeedGrowthJob(
            DbContextOptions options,
            ILogger<CompleteSeedGrowthJob> logger)
        {
            _logger = logger;
            _db = new AppDbContext(options);
        }

        public async Task Execute(long userId, Guid userFarmId)
        {
            var entity = await _db.UserFarms.SingleOrDefaultAsync(x => x.Id == userFarmId);

            if (entity.State is FieldState.Empty)
            {
                throw new Exception(
                    $"Cannot complete seed growth since user {userId} farm {entity.Number} state is empty");
            }

            entity.State = FieldState.Completed;
            entity.BeenGrowingFor = null;
            entity.CompleteAt = null;
            entity.UpdatedAt = DateTimeOffset.UtcNow;

            await _db.UpdateEntity(entity);
            
            _logger.LogInformation(
                "Completed seed growth for user {UserId} farm {Number}",
                userId, entity.Number);
        }
    }
}