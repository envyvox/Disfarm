using System.Threading;
using System.Threading.Tasks;
using Disfarm.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Disfarm.Services.Game.Building.Queries
{
    public record CheckUserHasBuildingQuery(long UserId, Data.Enums.Building Building) : IRequest<bool>;

    public class CheckUserHasBuildingHandler : IRequestHandler<CheckUserHasBuildingQuery, bool>
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public CheckUserHasBuildingHandler(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        public async Task<bool> Handle(CheckUserHasBuildingQuery request, CancellationToken ct)
        {
            using var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            return await db.UserBuildings
                .AnyAsync(x =>
                    x.UserId == request.UserId &&
                    x.Building == request.Building);
        }
    }
}