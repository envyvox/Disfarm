using System;
using System.Threading;
using System.Threading.Tasks;
using Disfarm.Data;
using Disfarm.Data.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Disfarm.Services.Game.Seed.Commands
{
    public record RemoveSeedFromUserCommand(long UserId, Guid SeedId, uint Amount) : IRequest;

    public class RemoveSeedFromUserHandler : IRequestHandler<RemoveSeedFromUserCommand>
    {
        private readonly ILogger<RemoveSeedFromUserHandler> _logger;
        private readonly IServiceScopeFactory _scopeFactory;

        public RemoveSeedFromUserHandler(
            IServiceScopeFactory scopeFactory,
            ILogger<RemoveSeedFromUserHandler> logger)
        {
            _logger = logger;
            _scopeFactory = scopeFactory;
        }

        public async Task<Unit> Handle(RemoveSeedFromUserCommand request, CancellationToken ct)
        {
            using var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var entity = await db.UserSeeds
                .SingleOrDefaultAsync(x =>
                    x.UserId == request.UserId &&
                    x.SeedId == request.SeedId);

            if (entity is null)
            {
                throw new Exception(
                    $"user {request.UserId} doesnt have seed {request.SeedId}");
            }

            entity.Amount -= request.Amount;
            entity.UpdatedAt = DateTimeOffset.UtcNow;

            await db.UpdateEntity(entity);

            _logger.LogInformation(
                "Removed from user {UserId} seed {SeedId} amount {Amount}",
                request.UserId, request.SeedId, request.Amount);

            return Unit.Value;
        }
    }
}