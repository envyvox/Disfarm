using System;
using System.Threading;
using System.Threading.Tasks;
using Disfarm.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Disfarm.Services.Game.DailyReward.Queries
{
    public record CheckUserDailyRewardQuery(long UserId, DayOfWeek DayOfWeek) : IRequest<bool>;

    public class CheckUserDailyRewardHandler : IRequestHandler<CheckUserDailyRewardQuery, bool>
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public CheckUserDailyRewardHandler(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        public async Task<bool> Handle(CheckUserDailyRewardQuery request, CancellationToken ct)
        {
            using var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            return await db.UserDailyRewards
                .AnyAsync(x =>
                    x.UserId == request.UserId &&
                    x.DayOfWeek == request.DayOfWeek);
        }
    }
}