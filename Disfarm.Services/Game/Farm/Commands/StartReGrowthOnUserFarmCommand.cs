using System;
using System.Threading;
using System.Threading.Tasks;
using Disfarm.Data;
using Disfarm.Data.Enums;
using Disfarm.Data.Extensions;
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
        private readonly IServiceScopeFactory _scopeFactory;

        public StartReGrowthOnUserFarmHandler(
            IServiceScopeFactory scopeFactory,
            ILogger<StartReGrowthOnUserFarmHandler> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
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

            entity.State = FieldState.Planted;
            entity.InReGrowth = true;
            entity.UpdatedAt = DateTimeOffset.UtcNow;

            await db.UpdateEntity(entity);

            _logger.LogInformation(
                "Started re growth on user {UserId} farm {Number}",
                request.UserId, request.Number);

            return Unit.Value;
        }
    }
}