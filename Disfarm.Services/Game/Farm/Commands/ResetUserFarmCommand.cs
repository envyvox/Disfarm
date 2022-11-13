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
    public record ResetUserFarmCommand(long UserId, uint Number) : IRequest;

    public class ResetUserFarmHandler : IRequestHandler<ResetUserFarmCommand>
    {
        private readonly ILogger<ResetUserFarmHandler> _logger;
        private readonly IServiceScopeFactory _scopeFactory;

        public ResetUserFarmHandler(
            IServiceScopeFactory scopeFactory,
            ILogger<ResetUserFarmHandler> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        public async Task<Unit> Handle(ResetUserFarmCommand request, CancellationToken ct)
        {
            using var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var entity = await db.UserFarms
                .SingleOrDefaultAsync(x =>
                    x.UserId == request.UserId &&
                    x.Number == request.Number);

            if (entity is null)
            {
                throw new Exception(
                    $"user {request.UserId} doesnt have farm {request.Number}");
            }

            entity.State = FieldState.Empty;
            entity.SeedId = null;
            entity.InReGrowth = false;
            entity.CompleteAt = null;
            entity.UpdatedAt = DateTimeOffset.UtcNow;

            await db.UpdateEntity(entity);

            _logger.LogInformation(
                "Reseted user {UserId} farm {Number} to default values",
                request.UserId, request.Number);

            return Unit.Value;
        }
    }
}