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

namespace Disfarm.Services.Game.Statistic.Commands
{
    public record AddStatisticToUserCommand(long UserId, Data.Enums.Statistic Type, uint Amount = 1) : IRequest;

    public class AddStatisticToUserHandler : IRequestHandler<AddStatisticToUserCommand>
    {
        private readonly ILogger<AddStatisticToUserHandler> _logger;
        private readonly IServiceScopeFactory _scopeFactory;

        public AddStatisticToUserHandler(
            IServiceScopeFactory scopeFactory,
            ILogger<AddStatisticToUserHandler> logger)
        {
            _logger = logger;
            _scopeFactory = scopeFactory;
        }

        public async Task<Unit> Handle(AddStatisticToUserCommand request, CancellationToken ct)
        {
            using var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var entity = await db.UserStatistics
                .SingleOrDefaultAsync(x =>
                    x.UserId == request.UserId &&
                    x.Period == StatisticPeriod.Weekly &&
                    x.Type == request.Type);

            if (entity is null)
            {
                var created = await db.CreateEntity(new UserStatistic
                {
                    Id = Guid.NewGuid(),
                    UserId = request.UserId,
                    Period = StatisticPeriod.Weekly,
                    Type = request.Type,
                    Amount = request.Amount,
                    CreatedAt = DateTimeOffset.UtcNow,
                    UpdatedAt = DateTimeOffset.UtcNow
                });

                _logger.LogInformation(
                    "Created user statistic entity {@Entity}",
                    created);
            }
            else
            {
                entity.Amount += request.Amount;
                entity.UpdatedAt = DateTimeOffset.UtcNow;

                await db.UpdateEntity(entity);

                _logger.LogInformation(
                    "Added user {UserId} statistic {Type} amount {Amount}",
                    request.UserId, request.Type.ToString(), request.Amount);
            }

            return await AddGeneralStatisticToUser(db, request.UserId, request.Type, request.Amount);
        }

        private async Task<Unit> AddGeneralStatisticToUser(AppDbContext db, long userId, Data.Enums.Statistic type,
            uint amount = 1)
        {
            var entity = await db.UserStatistics
                .SingleOrDefaultAsync(x =>
                    x.UserId == userId &&
                    x.Period == StatisticPeriod.General &&
                    x.Type == type);

            if (entity is null)
            {
                await db.CreateEntity(new UserStatistic
                {
                    Id = Guid.NewGuid(),
                    UserId = userId,
                    Period = StatisticPeriod.General,
                    Type = type,
                    Amount = amount,
                    CreatedAt = DateTimeOffset.UtcNow,
                    UpdatedAt = DateTimeOffset.UtcNow
                });
            }
            else
            {
                entity.Amount += amount;
                entity.UpdatedAt = DateTimeOffset.UtcNow;

                await db.UpdateEntity(entity);
            }

            return Unit.Value;
        }
    }
}