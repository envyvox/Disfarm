using System;
using System.Collections.Generic;
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

namespace Disfarm.Services.Game.Farm.Commands
{
    public record CreateUserFarmsCommand(long UserId, IEnumerable<uint> Numbers) : IRequest;

    public class CreateUserFarmsHandler : IRequestHandler<CreateUserFarmsCommand>
    {
        private readonly ILogger<CreateUserFarmsHandler> _logger;
        private readonly IServiceScopeFactory _scopeFactory;

        public CreateUserFarmsHandler(
            IServiceScopeFactory scopeFactory,
            ILogger<CreateUserFarmsHandler> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        public async Task<Unit> Handle(CreateUserFarmsCommand request, CancellationToken ct)
        {
            using var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            foreach (var number in request.Numbers)
            {
                var exist = await db.UserFarms
                    .AnyAsync(x =>
                        x.UserId == request.UserId &&
                        x.Number == number);

                if (exist)
                {
                    throw new Exception(
                        $"user {request.UserId} already have farm with number {number}");
                }

                var created = await db.CreateEntity(new UserFarm
                {
                    Id = Guid.NewGuid(),
                    UserId = request.UserId,
                    Number = number,
                    State = FieldState.Empty,
                    SeedId = null,
                    InReGrowth = false,
                    CompleteAt = null,
                    CreatedAt = DateTimeOffset.UtcNow,
                    UpdatedAt = DateTimeOffset.UtcNow
                });

                _logger.LogInformation(
                    "Created user farm entity {@Entity}",
                    created);
            }

            return Unit.Value;
        }
    }
}