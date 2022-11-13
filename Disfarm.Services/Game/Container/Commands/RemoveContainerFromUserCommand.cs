using System;
using System.Threading;
using System.Threading.Tasks;
using Disfarm.Data;
using Disfarm.Data.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Disfarm.Services.Game.Container.Commands
{
    public record RemoveContainerFromUserCommand(long UserId, Data.Enums.Container Type, uint Amount) : IRequest;

    public class RemoveContainerFromUserHandler : IRequestHandler<RemoveContainerFromUserCommand>
    {
        private readonly ILogger<RemoveContainerFromUserHandler> _logger;
        private readonly IServiceScopeFactory _scopeFactory;

        public RemoveContainerFromUserHandler(
            IServiceScopeFactory scopeFactory,
            ILogger<RemoveContainerFromUserHandler> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        public async Task<Unit> Handle(RemoveContainerFromUserCommand request, CancellationToken ct)
        {
            using var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var entity = await db.UserContainers
                .SingleOrDefaultAsync(x =>
                    x.UserId == request.UserId &&
                    x.Type == request.Type);

            if (entity is null)
            {
                throw new Exception(
                    $"user {request.UserId} doesnt have container {request.Type.ToString()} entity");
            }

            entity.Amount -= request.Amount;
            entity.UpdatedAt = DateTimeOffset.UtcNow;

            await db.UpdateEntity(entity);

            _logger.LogInformation(
                "Removed from user {UserId} container {Type} amount {Amount}",
                request.UserId, request.Type.ToString(), request.Amount);

            return Unit.Value;
        }
    }
}