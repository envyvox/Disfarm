using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Disfarm.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Disfarm.Services.Game.DailyReward.Queries
{
    public record GetUserDailyRewardsQuery(long UserId) : IRequest<List<DayOfWeek>>;

    public class GetUserDailyRewardsHandler : IRequestHandler<GetUserDailyRewardsQuery, List<DayOfWeek>>
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public GetUserDailyRewardsHandler(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        public async Task<List<DayOfWeek>> Handle(GetUserDailyRewardsQuery request, CancellationToken ct)
        {
            using var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var entities = await db.UserDailyRewards
                .AsQueryable()
                .Where(x => x.UserId == request.UserId)
                .Select(x => x.DayOfWeek)
                .ToListAsync();

            return entities;
        }
    }
}