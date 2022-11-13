using System.Threading;
using System.Threading.Tasks;
using Disfarm.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Disfarm.Services.Game.Referral.Queries
{
    public record CheckUserHasReferrerQuery(long UserId) : IRequest<bool>;

    public class CheckUserHasReferrerHandler : IRequestHandler<CheckUserHasReferrerQuery, bool>
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public CheckUserHasReferrerHandler(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        public async Task<bool> Handle(CheckUserHasReferrerQuery request, CancellationToken ct)
        {
            using var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            return await db.UserReferrers
                .AnyAsync(x => x.UserId == request.UserId);
        }
    }
}