using System;
using System.Threading;
using System.Threading.Tasks;
using Disfarm.Data;
using Disfarm.Data.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Disfarm.Services.Game.Fish.Commands
{
    public record RemoveFishFromUserCommand(long UserId, Guid FishId, uint Amount) : IRequest;

    public class RemoveFishFromUserHandler : IRequestHandler<RemoveFishFromUserCommand>
    {
        private readonly ILogger<RemoveFishFromUserHandler> _logger;
        private readonly IServiceScopeFactory _scopeFactory;

        public RemoveFishFromUserHandler(
            IServiceScopeFactory scopeFactory,
            ILogger<RemoveFishFromUserHandler> logger)
        {
            _logger = logger;
            _scopeFactory = scopeFactory;
        }

        public async Task<Unit> Handle(RemoveFishFromUserCommand request, CancellationToken ct)
        {
            using var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var entity = await db.UserFishes
                .SingleOrDefaultAsync(x =>
                    x.UserId == request.UserId &&
                    x.FishId == request.FishId);

            if (entity is null)
            {
                throw new Exception(
                    $"user {request.UserId} doesnt have fish {request.FishId}");
            }

            entity.Amount -= request.Amount;
            entity.UpdatedAt = DateTimeOffset.UtcNow;

            await db.UpdateEntity(entity);

            _logger.LogInformation(
                "Removed from user {UserId} fish {FishId} amount {Amount}",
                request.UserId, request.FishId, request.Amount);

            return Unit.Value;
        }
    }
}