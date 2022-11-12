using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Disfarm.Data;
using Disfarm.Data.Enums;
using Disfarm.Data.Extensions;
using Disfarm.Services.Game.Farm.Helpers;
using Disfarm.Services.Game.Seed.Models;
using Disfarm.Services.Game.World.Queries;
using Disfarm.Services.Hangfire.BackgroundJobs.CheckSeedWatered;
using Disfarm.Services.Hangfire.BackgroundJobs.CompleteSeedGrowth;
using Hangfire;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Disfarm.Services.Game.Farm.Commands
{
    public record PlantUserFarmsCommand(
            long UserId,
            uint[] Numbers,
            SeedDto Seed)
        : IRequest;

    public class PlantUserFarmsHandler : IRequestHandler<PlantUserFarmsCommand>
    {
        private readonly ILogger<PlantUserFarmsHandler> _logger;
        private readonly IMediator _mediator;
        private readonly AppDbContext _db;

        public PlantUserFarmsHandler(
            DbContextOptions options,
            ILogger<PlantUserFarmsHandler> logger,
            IMediator mediator)
        {
            _db = new AppDbContext(options);
            _logger = logger;
            _mediator = mediator;
        }

        public async Task<Unit> Handle(PlantUserFarmsCommand request, CancellationToken ct)
        {
            var entities = await _db.UserFarms
                .AsQueryable()
                .Where(x =>
                    x.UserId == request.UserId &&
                    request.Numbers.Contains(x.Number))
                .ToListAsync();

            if (entities.Any() is false)
            {
                throw new Exception(
                    $"user {request.UserId} doesnt have farms with numbers {request.Numbers}");
            }

            var state = await _mediator.Send(new GetWorldStateQuery());

            foreach (var entity in entities)
            {
                entity.SeedId = request.Seed.Id;
                entity.State = state.WeatherToday is Weather.Rain ? FieldState.Watered : FieldState.Planted;
                entity.UpdatedAt = DateTimeOffset.UtcNow;

                if (entity.State is FieldState.Watered)
                {
                    entity.CompleteAt = DateTimeOffset.UtcNow.Add(request.Seed.Growth);
                    
                    FarmHelper.ScheduleBackgroundJobs(request.UserId, entity.Id, request.Seed.Growth);
                }

                await _db.UpdateEntity(entity);

                _logger.LogInformation(
                    "Planted user {UserId} farm {Number} with seed {SeedId} and state {State}",
                    request.UserId, entity.Number, request.Seed.Id, FieldState.Planted.ToString());
            }

            return Unit.Value;
        }
    }
}