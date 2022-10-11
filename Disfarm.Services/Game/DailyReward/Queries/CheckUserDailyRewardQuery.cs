using System;
using System.Threading;
using System.Threading.Tasks;
using Disfarm.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Disfarm.Services.Game.DailyReward.Queries
{
    public record CheckUserDailyRewardQuery(long UserId, DayOfWeek DayOfWeek) : IRequest<bool>;

    public class CheckUserDailyRewardHandler : IRequestHandler<CheckUserDailyRewardQuery, bool>
    {
        private readonly AppDbContext _db;

        public CheckUserDailyRewardHandler(DbContextOptions options)
        {
            _db = new AppDbContext(options);
        }

        public async Task<bool> Handle(CheckUserDailyRewardQuery request, CancellationToken ct)
        {
            return await _db.UserDailyRewards
                .AnyAsync(x =>
                    x.UserId == request.UserId &&
                    x.DayOfWeek == request.DayOfWeek);
        }
    }
}