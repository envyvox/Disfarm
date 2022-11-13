using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Disfarm.Data;
using Disfarm.Data.Enums;
using Disfarm.Data.Extensions;
using Disfarm.Services.Game.Building.Queries;
using Disfarm.Services.Game.Farm.Helpers;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Disfarm.Services.Game.Farm.Commands
{
    public record UpdateUserFarmsStateCommand(long UserId, FieldState State) : IRequest;

    public class UpdateUserFarmsStateHandler : IRequestHandler<UpdateUserFarmsStateCommand>
    {
        private readonly ILogger<UpdateUserFarmsStateHandler> _logger;
        private readonly IMediator _mediator;
        private readonly IServiceScopeFactory _scopeFactory;

        public UpdateUserFarmsStateHandler(
            IServiceScopeFactory scopeFactory,
            ILogger<UpdateUserFarmsStateHandler> logger,
            IMediator mediator)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
            _mediator = mediator;
        }

        public async Task<Unit> Handle(UpdateUserFarmsStateCommand request, CancellationToken ct)
        {
            using var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var entities = await db.UserFarms
                .Include(x => x.Seed)
                .Where(x =>
                    x.UserId == request.UserId &&
                    x.State == FieldState.Planted)
                .ToListAsync();

            foreach (var entity in entities)
            {
                if (entity.State is FieldState.Planted && request.State is FieldState.Watered)
                {
                    var userBuildings = await _mediator.Send(new GetUserBuildingsQuery(request.UserId));
                    var defaultTime = entity.BeenGrowingFor is null
                        ? entity.InReGrowth
                            ? entity.Seed.ReGrowth ?? throw new Exception("Field in regrowth but seed regrowth is null")
                            : entity.Seed.Growth
                        : entity.InReGrowth
                            ? entity.Seed.ReGrowth?.Subtract(entity.BeenGrowingFor.Value) ??
                              throw new Exception("Field in regrowth but seed regrowth is null")
                            : entity.Seed.Growth.Subtract(entity.BeenGrowingFor.Value);
                    var completionTime = FarmHelper.CompletionTimeAfterBuildingsSpeedBonus(defaultTime, userBuildings);

                    entity.CompleteAt = DateTimeOffset.UtcNow.Add(completionTime);

                    FarmHelper.ScheduleBackgroundJobs(request.UserId, entity.Id, completionTime);
                }

                entity.State = request.State;
                entity.UpdatedAt = DateTimeOffset.UtcNow;

                await db.UpdateEntity(entity);

                _logger.LogInformation(
                    "Updated user {UserId} farm {Number} to state {State}",
                    request.UserId, entity.Number, request.State);
            }

            return Unit.Value;
        }
    }
}