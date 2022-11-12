using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Disfarm.Data;
using Disfarm.Data.Enums;
using Disfarm.Data.Extensions;
using Disfarm.Services.Game.Farm.Helpers;
using Disfarm.Services.Hangfire.BackgroundJobs.CheckSeedWatered;
using Disfarm.Services.Hangfire.BackgroundJobs.CompleteSeedGrowth;
using Hangfire;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Disfarm.Services.Game.Farm.Commands
{
    public record UpdateUserFarmsStateCommand(long UserId, FieldState State) : IRequest;

    public class UpdateUserFarmsStateHandler : IRequestHandler<UpdateUserFarmsStateCommand>
    {
        private readonly ILogger<UpdateUserFarmsStateHandler> _logger;
        private readonly AppDbContext _db;

        public UpdateUserFarmsStateHandler(
            DbContextOptions options,
            ILogger<UpdateUserFarmsStateHandler> logger)
        {
            _db = new AppDbContext(options);
            _logger = logger;
        }

        public async Task<Unit> Handle(UpdateUserFarmsStateCommand request, CancellationToken ct)
        {
            var entities = await _db.UserFarms
                .Include(x => x.Seed)
                .Where(x =>
                    x.UserId == request.UserId &&
                    x.State == FieldState.Planted)
                .ToListAsync();

            foreach (var entity in entities)
            {
                if (entity.State is FieldState.Planted && request.State is FieldState.Watered)
                {
                    var completeTimeSpan = entity.BeenGrowingFor is null
                        ? entity.InReGrowth
                            ? entity.Seed.ReGrowth ?? throw new Exception("Field in regrowth but seed regrowth is null")
                            : entity.Seed.Growth
                        : entity.InReGrowth
                            ? entity.Seed.ReGrowth?.Subtract(entity.BeenGrowingFor.Value) ??
                              throw new Exception("Field in regrowth but seed regrowth is null")
                            : entity.Seed.Growth.Subtract(entity.BeenGrowingFor.Value);

                    entity.CompleteAt = DateTimeOffset.UtcNow.Add(completeTimeSpan);

                    FarmHelper.ScheduleBackgroundJobs(request.UserId, entity.Id, completeTimeSpan);
                }

                entity.State = request.State;
                entity.UpdatedAt = DateTimeOffset.UtcNow;

                await _db.UpdateEntity(entity);

                _logger.LogInformation(
                    "Updated user {UserId} farm {Number} to state {State}",
                    request.UserId, entity.Number, request.State);
            }

            return Unit.Value;
        }
    }
}