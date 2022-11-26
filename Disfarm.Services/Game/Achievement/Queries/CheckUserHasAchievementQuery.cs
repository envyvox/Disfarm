using System.Threading;
using System.Threading.Tasks;
using Disfarm.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using CacheExtensions = Disfarm.Services.Extensions.CacheExtensions;

namespace Disfarm.Services.Game.Achievement.Queries
{
    public record CheckUserHasAchievementQuery(long UserId, Data.Enums.Achievement.Achievement Type) : IRequest<bool>;

    public class CheckUserHasAchievementHandler : IRequestHandler<CheckUserHasAchievementQuery, bool>
    {
        private readonly IMemoryCache _cache;
        private readonly IServiceScopeFactory _scopeFactory;

        public CheckUserHasAchievementHandler(
            IServiceScopeFactory scopeFactory,
            IMemoryCache cache)
        {
            _cache = cache;
            _scopeFactory = scopeFactory;
        }

        public async Task<bool> Handle(CheckUserHasAchievementQuery request, CancellationToken ct)
        {
            if (_cache.TryGetValue(CacheExtensions.GetUserHasAchievementKey(request.UserId, request.Type),
                    out bool exist)) return exist;

            using var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            exist = await db.UserAchievements
                .AnyAsync(x =>
                    x.UserId == request.UserId &&
                    x.Type == request.Type);

            _cache.Set(CacheExtensions.GetUserHasAchievementKey(request.UserId, request.Type), exist,
                CacheExtensions.DefaultCacheOptions);

            return exist;
        }
    }
}