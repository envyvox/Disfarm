using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Disfarm.Data;
using Disfarm.Data.Enums;
using Disfarm.Data.Extensions;
using Disfarm.Services.Game.Building.Queries;
using Disfarm.Services.Game.Farm.Helpers;
using Disfarm.Services.Game.Seed.Models;
using Disfarm.Services.Game.World.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
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
        private readonly IServiceScopeFactory _scopeFactory;

        public PlantUserFarmsHandler(
            IServiceScopeFactory scopeFactory,
            ILogger<PlantUserFarmsHandler> logger,
            IMediator mediator)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
            _mediator = mediator;
        }

        public async Task<Unit> Handle(PlantUserFarmsCommand request, CancellationToken ct)
        {
            using var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var entities = await db.UserFarms
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

            var worldState = await _mediator.Send(new GetWorldStateQuery());
            var farmState = worldState.WeatherToday is Weather.Rain ? FieldState.Watered : FieldState.Planted;

            foreach (var entity in entities)
            {
                entity.SeedId = request.Seed.Id;
                entity.State = farmState;
                entity.UpdatedAt = DateTimeOffset.UtcNow;

                if (entity.State is FieldState.Watered)
                {
                    var userBuildings = await _mediator.Send(new GetUserBuildingsQuery(request.UserId));
                    var completionTime = FarmHelper.CompletionTimeAfterBuildingsSpeedBonus(
                        request.Seed.Growth, userBuildings);

                    entity.CompleteAt = DateTimeOffset.UtcNow.Add(completionTime);

                    FarmHelper.ScheduleBackgroundJobs(request.UserId, entity.Id, completionTime);
                }

                await db.UpdateEntity(entity);

                _logger.LogInformation(
                    "Planted user {UserId} farm {Number} with seed {SeedId} and state {State}",
                    request.UserId, entity.Number, request.Seed.Id, FieldState.Planted.ToString());
            }

            return Unit.Value;
        }
    }
}