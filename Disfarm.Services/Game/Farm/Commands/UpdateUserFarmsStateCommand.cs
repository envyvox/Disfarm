using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Disfarm.Data;
using Disfarm.Data.Enums;
using Disfarm.Data.Extensions;
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

            foreach (var field in entities)
            {
                if (field.State is FieldState.Planted && request.State is FieldState.Watered)
                {
                    var completeTimeSpan = field.BeenGrowingFor is null
                        ? field.InReGrowth
                            ? field.Seed.ReGrowth ?? throw new Exception("Field in regrowth but seed regrowth is null")
                            : field.Seed.Growth
                        : field.InReGrowth
                            ? field.Seed.ReGrowth?.Subtract(field.BeenGrowingFor.Value) ??
                              throw new Exception("Field in regrowth but seed regrowth is null")
                            : field.Seed.Growth.Subtract(field.BeenGrowingFor.Value);

                    field.CompleteAt = DateTimeOffset.UtcNow.Add(completeTimeSpan);

                    var completeSeedGrowthJobId = BackgroundJob.Schedule<ICompleteSeedGrowthJob>(x =>
                            x.Execute(request.UserId, field.Id),
                        completeTimeSpan);

                    if (completeTimeSpan > TimeSpan.FromHours(24))
                    {
                        BackgroundJob.Schedule<ICheckSeedWateredJob>(x =>
                                x.Execute(request.UserId, field.Id, completeSeedGrowthJobId),
                            TimeSpan.FromHours(24));
                    }
                }

                field.State = request.State;
                field.UpdatedAt = DateTimeOffset.UtcNow;

                await _db.UpdateEntity(field);

                _logger.LogInformation(
                    "Updated user {UserId} farm {Number} to state {State}",
                    request.UserId, field.Number, request.State);
            }

            return Unit.Value;
        }
    }
}