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

namespace Disfarm.Services.Game.DailyReward.Commands
{
    public record CreateUserDailyRewardCommand(long UserId, DayOfWeek DayOfWeek) : IRequest;

    public class CreateUserDailyRewardHandler : IRequestHandler<CreateUserDailyRewardCommand>
    {
        private readonly ILogger<CreateUserDailyRewardHandler> _logger;
        private readonly IServiceScopeFactory _scopeFactory;

        public CreateUserDailyRewardHandler(
            IServiceScopeFactory scopeFactory,
            ILogger<CreateUserDailyRewardHandler> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        public async Task<Unit> Handle(CreateUserDailyRewardCommand request, CancellationToken ct)
        {
            using var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var exist = await db.UserDailyRewards
                .AnyAsync(x =>
                    x.UserId == request.UserId &&
                    x.DayOfWeek == request.DayOfWeek);

            if (exist)
            {
                throw new Exception(
                    $"user {request.UserId} daily reward for day {request.DayOfWeek.ToString()} already exist");
            }

            var created = await db.CreateEntity(new UserDailyReward
            {
                Id = Guid.NewGuid(),
                UserId = request.UserId,
                DayOfWeek = request.DayOfWeek
            });

            _logger.LogInformation(
                "Created user daily reward entity {@Entity}",
                created);

            return Unit.Value;
        }
    }
}