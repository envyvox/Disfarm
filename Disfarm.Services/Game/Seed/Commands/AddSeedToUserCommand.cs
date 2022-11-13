using System;
using System.Threading;
using System.Threading.Tasks;
using Disfarm.Data;
using Disfarm.Data.Entities.User;
using Disfarm.Data.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Disfarm.Services.Game.Seed.Commands
{
    public record AddSeedToUserCommand(long UserId, Guid SeedId, uint Amount) : IRequest;

    public class AddSeedToUserHandler : IRequestHandler<AddSeedToUserCommand>
    {
        private readonly ILogger<AddSeedToUserHandler> _logger;
        private readonly IServiceScopeFactory _scopeFactory;

        public AddSeedToUserHandler(
            IServiceScopeFactory scopeFactory,
            ILogger<AddSeedToUserHandler> logger)
        {
            _logger = logger;
            _scopeFactory = scopeFactory;
        }

        public async Task<Unit> Handle(AddSeedToUserCommand request, CancellationToken ct)
        {
            using var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var entity = await db.UserSeeds
                .SingleOrDefaultAsync(x =>
                    x.UserId == request.UserId &&
                    x.SeedId == request.SeedId);

            if (entity is null)
            {
                var created = await db.CreateEntity(new UserSeed
                {
                    Id = Guid.NewGuid(),
                    UserId = request.UserId,
                    SeedId = request.SeedId,
                    Amount = request.Amount,
                    CreatedAt = DateTimeOffset.UtcNow,
                    UpdatedAt = DateTimeOffset.UtcNow
                });

                _logger.LogInformation(
                    "Created user seed entity {@Entity}",
                    created);
            }
            else
            {
                entity.Amount += request.Amount;
                entity.UpdatedAt = DateTimeOffset.UtcNow;

                await db.UpdateEntity(entity);

                _logger.LogInformation(
                    "Added user {UserId} seed {SeedId} amount {Amount}",
                    request.UserId, request.SeedId, request.Amount);
            }

            return Unit.Value;
        }
    }
}