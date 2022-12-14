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

namespace Disfarm.Services.Game.Container.Commands
{
    public record AddContainerToUserCommand(long UserId, Data.Enums.Container Type, uint Amount) : IRequest;

    public class AddContainerToUserHandler : IRequestHandler<AddContainerToUserCommand>
    {
        private readonly ILogger<AddContainerToUserHandler> _logger;
        private readonly IServiceScopeFactory _scopeFactory;

        public AddContainerToUserHandler(
            IServiceScopeFactory scopeFactory,
            ILogger<AddContainerToUserHandler> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        public async Task<Unit> Handle(AddContainerToUserCommand request, CancellationToken ct)
        {
            using var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var entity = await db.UserContainers
                .SingleOrDefaultAsync(x =>
                    x.UserId == request.UserId &&
                    x.Type == request.Type);

            if (entity is null)
            {
                var created = await db.CreateEntity(new UserContainer
                {
                    Id = Guid.NewGuid(),
                    UserId = request.UserId,
                    Type = request.Type,
                    Amount = request.Amount,
                    CreatedAt = DateTimeOffset.UtcNow,
                    UpdatedAt = DateTimeOffset.UtcNow
                });

                _logger.LogInformation(
                    "Created user container entity {@Entity}",
                    created);
            }
            else
            {
                entity.Amount += request.Amount;
                entity.UpdatedAt = DateTimeOffset.UtcNow;

                await db.UpdateEntity(entity);

                _logger.LogInformation(
                    "Added user {UserId} container {Type} amount {Amount}",
                    request.UserId, request.Type.ToString(), request.Amount);
            }

            return Unit.Value;
        }
    }
}