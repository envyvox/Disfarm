using System;
using System.Threading;
using System.Threading.Tasks;
using Disfarm.Data;
using Disfarm.Data.Entities.User;
using Disfarm.Data.Enums;
using Disfarm.Data.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Disfarm.Services.Game.Transit.Commands
{
    public record CreateUserMovementCommand(
            long UserId,
            Location Departure,
            Location Destination,
            TimeSpan Duration)
        : IRequest;

    public class CreateUserMovementHandler : IRequestHandler<CreateUserMovementCommand>
    {
        private readonly ILogger<CreateUserMovementHandler> _logger;
        private readonly IServiceScopeFactory _scopeFactory;

        public CreateUserMovementHandler(
            IServiceScopeFactory scopeFactory,
            ILogger<CreateUserMovementHandler> logger)
        {
            _logger = logger;
            _scopeFactory = scopeFactory;
        }

        public async Task<Unit> Handle(CreateUserMovementCommand request, CancellationToken ct)
        {
            using var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var exist = await db.UserMovements
                .AnyAsync(x => x.UserId == request.UserId);

            if (exist)
            {
                throw new Exception(
                    $"user {request.UserId} already have movement entity");
            }

            var created = await db.CreateEntity(new UserMovement
            {
                Id = Guid.NewGuid(),
                UserId = request.UserId,
                Departure = request.Departure,
                Destination = request.Destination,
                Arrival = DateTimeOffset.UtcNow.Add(request.Duration),
                CreatedAt = DateTimeOffset.UtcNow
            });

            _logger.LogInformation(
                "Created user movement entity {@Entity}",
                created);

            return Unit.Value;
        }
    }
}