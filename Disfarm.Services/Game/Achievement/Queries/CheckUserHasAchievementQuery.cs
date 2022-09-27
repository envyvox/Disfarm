using System.Threading;
using System.Threading.Tasks;
using Disfarm.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using CacheExtensions = Disfarm.Services.Extensions.CacheExtensions;

namespace Disfarm.Services.Game.Achievement.Queries
{
    public record CheckUserHasAchievementQuery(long UserId, Data.Enums.Achievement Type) : IRequest<bool>;

    public class CheckUserHasAchievementHandler : IRequestHandler<CheckUserHasAchievementQuery, bool>
    {
        private readonly IMemoryCache _cache;
        private readonly AppDbContext _db;

        public CheckUserHasAchievementHandler(
            DbContextOptions options,
            IMemoryCache cache)
        {
            _cache = cache;
            _db = new AppDbContext(options);
        }

        public async Task<bool> Handle(CheckUserHasAchievementQuery request, CancellationToken ct)
        {
            if (_cache.TryGetValue(string.Format(CacheExtensions.UserHasAchievementKey, request.UserId, request.Type),
                    out bool exist)) return exist;

            exist = await _db.UserAchievements
                .AnyAsync(x =>
                    x.UserId == request.UserId &&
                    x.Type == request.Type);

            _cache.Set(string.Format(CacheExtensions.UserHasAchievementKey, request.UserId, request.Type), exist,
                CacheExtensions.DefaultCacheOptions);

            return exist;
        }
    }
}