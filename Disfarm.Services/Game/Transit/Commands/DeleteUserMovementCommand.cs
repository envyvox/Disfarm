using System;
using System.Threading;
using System.Threading.Tasks;
using Disfarm.Data;
using Disfarm.Data.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Disfarm.Services.Game.Transit.Commands
{
    public record DeleteUserMovementCommand(long UserId) : IRequest;

    public class DeleteUserMovementHandler : IRequestHandler<DeleteUserMovementCommand>
    {
        private readonly ILogger<DeleteUserMovementHandler> _logger;
        private readonly IServiceScopeFactory _scopeFactory;

        public DeleteUserMovementHandler(
            IServiceScopeFactory scopeFactory,
            ILogger<DeleteUserMovementHandler> logger)
        {
            _logger = logger;
            _scopeFactory = scopeFactory;
        }

        public async Task<Unit> Handle(DeleteUserMovementCommand request, CancellationToken ct)
        {
            using var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var entity = await db.UserMovements
                .SingleOrDefaultAsync(x => x.UserId == request.UserId);

            if (entity is null)
            {
                throw new Exception(
                    $"user {request.UserId} doesnt have movement entity");
            }

            await db.DeleteEntity(entity);

            _logger.LogInformation(
                "Deleted user movement entity for user {UserId}",
                request.UserId);

            return Unit.Value;
        }
    }
}