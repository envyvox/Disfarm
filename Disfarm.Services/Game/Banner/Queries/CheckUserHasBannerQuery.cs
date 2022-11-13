using System;
using System.Threading;
using System.Threading.Tasks;
using Disfarm.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Disfarm.Services.Game.Banner.Queries
{
    public record CheckUserHasBannerQuery(long UserId, Guid BannerId) : IRequest<bool>;

    public class CheckUserHasBannerHandler : IRequestHandler<CheckUserHasBannerQuery, bool>
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public CheckUserHasBannerHandler(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        public async Task<bool> Handle(CheckUserHasBannerQuery request, CancellationToken ct)
        {
            using var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            return await db.UserBanners
                .AnyAsync(x =>
                    x.UserId == request.UserId &&
                    x.BannerId == request.BannerId);
        }
    }
}