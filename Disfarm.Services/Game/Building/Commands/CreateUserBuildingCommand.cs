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

namespace Disfarm.Services.Game.Building.Commands
{
    public record CreateUserBuildingCommand(long UserId, Data.Enums.Building Building) : IRequest;

    public class CreateUserBuildingHandler : IRequestHandler<CreateUserBuildingCommand>
    {
        private readonly ILogger<CreateUserBuildingHandler> _logger;
        private readonly IServiceScopeFactory _scopeFactory;

        public CreateUserBuildingHandler(
            IServiceScopeFactory scopeFactory,
            ILogger<CreateUserBuildingHandler> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        public async Task<Unit> Handle(CreateUserBuildingCommand request, CancellationToken ct)
        {
            using var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var exist = await db.UserBuildings
                .AnyAsync(x =>
                    x.UserId == request.UserId &&
                    x.Building == request.Building);

            if (exist)
            {
                throw new Exception(
                    $"user {request.UserId} already have building {request.Building.ToString()}");
            }

            var created = await db.CreateEntity(new UserBuilding
            {
                Id = Guid.NewGuid(),
                UserId = request.UserId,
                Building = request.Building,
                CreatedAt = DateTimeOffset.UtcNow
            });

            _logger.LogInformation(
                "Created user building entity {@Entity}",
                created);

            return Unit.Value;
        }
    }
}