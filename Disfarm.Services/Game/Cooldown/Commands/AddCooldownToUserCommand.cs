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

namespace Disfarm.Services.Game.Cooldown.Commands
{
    public record AddCooldownToUserCommand(long UserId, Data.Enums.Cooldown Type, TimeSpan Duration) : IRequest;

    public class AddCooldownToUserHandler : IRequestHandler<AddCooldownToUserCommand>
    {
        private readonly ILogger<AddCooldownToUserHandler> _logger;
        private readonly IServiceScopeFactory _scopeFactory;

        public AddCooldownToUserHandler(
            IServiceScopeFactory scopeFactory,
            ILogger<AddCooldownToUserHandler> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        public async Task<Unit> Handle(AddCooldownToUserCommand request, CancellationToken ct)
        {
            using var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var entity = await db.UserCooldowns
                .SingleOrDefaultAsync(x =>
                    x.UserId == request.UserId &&
                    x.Type == request.Type);

            if (entity is null)
            {
                var created = await db.CreateEntity(new UserCooldown
                {
                    Id = Guid.NewGuid(),
                    UserId = request.UserId,
                    Type = request.Type,
                    Expiration = DateTimeOffset.UtcNow.Add(request.Duration),
                    CreatedAt = DateTimeOffset.UtcNow,
                    UpdatedAt = DateTimeOffset.UtcNow
                });

                _logger.LogInformation(
                    "Created user cooldown entity {@Entity}",
                    created);
            }
            else
            {
                entity.Expiration = DateTimeOffset.UtcNow.Add(request.Duration);
                entity.UpdatedAt = DateTimeOffset.UtcNow;

                await db.UpdateEntity(entity);

                _logger.LogInformation(
                    "Added user {UserId} cooldown {Type} duration {Duration}",
                    request.UserId, request.Type.ToString(), request.Duration);
            }

            return Unit.Value;
        }
    }
}