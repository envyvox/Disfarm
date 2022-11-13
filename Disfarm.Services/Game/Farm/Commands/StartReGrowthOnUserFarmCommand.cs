using System;
using System.Threading;
using System.Threading.Tasks;
using Disfarm.Data;
using Disfarm.Data.Enums;
using Disfarm.Data.Extensions;
using Disfarm.Services.Game.Building.Queries;
using Disfarm.Services.Game.Farm.Helpers;
using Disfarm.Services.Game.World.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Disfarm.Services.Game.Farm.Commands
{
    public record StartReGrowthOnUserFarmCommand(long UserId, uint Number) : IRequest;

    public class StartReGrowthOnUserFarmHandler : IRequestHandler<StartReGrowthOnUserFarmCommand>
    {
        private readonly ILogger<StartReGrowthOnUserFarmHandler> _logger;
        private readonly IMediator _mediator;
        private readonly IServiceScopeFactory _scopeFactory;

        public StartReGrowthOnUserFarmHandler(
            IServiceScopeFactory scopeFactory,
            ILogger<StartReGrowthOnUserFarmHandler> logger,
            IMediator mediator)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
            _mediator = mediator;
        }

        public async Task<Unit> Handle(StartReGrowthOnUserFarmCommand request, CancellationToken ct)
        {
            using var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var entity = await db.UserFarms
                .Include(x => x.Seed)
                .SingleOrDefaultAsync(x =>
                    x.UserId == request.UserId &&
                    x.Number == request.Number);

            if (entity is null)
            {
                throw new Exception(
                    $"user {request.UserId} doesnt have farm with number {request.Number}");
            }

            if (entity.Seed.ReGrowth is null)
            {
                throw new Exception(
                    $"seed {entity.Seed.Id} doesnt regrowth");
            }

            var state = await _mediator.Send(new GetWorldStateQuery());

            entity.State = state.WeatherToday == Weather.Clear
                ? FieldState.Planted
                : FieldState.Watered;
            entity.InReGrowth = true;
            entity.UpdatedAt = DateTimeOffset.UtcNow;

            if (entity.State is FieldState.Watered)
            {
                var userBuildings = await _mediator.Send(new GetUserBuildingsQuery(request.UserId));
                var completionTime = FarmHelper.CompletionTimeAfterBuildingsSpeedBonus(
                    entity.Seed.ReGrowth.Value, userBuildings);

                entity.CompleteAt = DateTimeOffset.UtcNow.Add(completionTime);

                FarmHelper.ScheduleBackgroundJobs(request.UserId, entity.Id, completionTime);
            }

            await db.UpdateEntity(entity);

            _logger.LogInformation(
                "Started re growth on user {UserId} farm {Number}",
                request.UserId, request.Number);

            return Unit.Value;
        }
    }
}