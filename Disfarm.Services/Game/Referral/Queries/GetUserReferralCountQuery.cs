using System.Threading;
using System.Threading.Tasks;
using Disfarm.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Disfarm.Services.Game.Referral.Queries
{
    public record GetUserReferralCountQuery(long UserId) : IRequest<uint>;

    public class GetUserReferralCountHandler : IRequestHandler<GetUserReferralCountQuery, uint>
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public GetUserReferralCountHandler(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        public async Task<uint> Handle(GetUserReferralCountQuery request, CancellationToken ct)
        {
            using var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            return (uint)await db.UserReferrers
                .CountAsync(x => x.ReferrerId == request.UserId);
        }
    }
}