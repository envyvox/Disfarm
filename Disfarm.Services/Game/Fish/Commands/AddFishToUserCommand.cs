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

namespace Disfarm.Services.Game.Fish.Commands
{
    public record AddFishToUserCommand(long UserId, Guid FishId, uint Amount) : IRequest;

    public class AddFishToUserHandler : IRequestHandler<AddFishToUserCommand>
    {
        private readonly ILogger<AddFishToUserHandler> _logger;
        private readonly IServiceScopeFactory _scopeFactory;

        public AddFishToUserHandler(
            IServiceScopeFactory scopeFactory,
            ILogger<AddFishToUserHandler> logger)
        {
            _logger = logger;
            _scopeFactory = scopeFactory;
        }

        public async Task<Unit> Handle(AddFishToUserCommand request, CancellationToken ct)
        {
            using var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var entity = await db.UserFishes
                .SingleOrDefaultAsync(x =>
                    x.UserId == request.UserId &&
                    x.FishId == request.FishId);

            if (entity is null)
            {
                var created = await db.CreateEntity(new UserFish
                {
                    Id = Guid.NewGuid(),
                    UserId = request.UserId,
                    FishId = request.FishId,
                    Amount = request.Amount,
                    CreatedAt = DateTimeOffset.UtcNow,
                    UpdatedAt = DateTimeOffset.UtcNow
                });

                _logger.LogInformation(
                    "Created user fish entity {@Entity}",
                    created);
            }
            else
            {
                entity.Amount += request.Amount;
                entity.UpdatedAt = DateTimeOffset.UtcNow;

                await db.UpdateEntity(entity);

                _logger.LogInformation(
                    "Added user {UserId} fish {FishId} amount {Amount}",
                    request.UserId, request.FishId, request.Amount);
            }

            return Unit.Value;
        }
    }
}