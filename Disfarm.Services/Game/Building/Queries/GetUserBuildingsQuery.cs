using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Disfarm.Data;
using Disfarm.Data.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Disfarm.Services.Game.Building.Queries
{
    public record GetUserBuildingsQuery(
            long UserId,
            BuildingCategory Category = BuildingCategory.Undefined)
        : IRequest<IList<Data.Enums.Building>>;

    public class GetUserBuildingsHandler : IRequestHandler<GetUserBuildingsQuery, IList<Data.Enums.Building>>
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public GetUserBuildingsHandler(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        public async Task<IList<Data.Enums.Building>> Handle(GetUserBuildingsQuery request, CancellationToken ct)
        {
            using var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var buildings = await db.UserBuildings
                .AsQueryable()
                .Where(x => x.UserId == request.UserId)
                .Select(x => x.Building)
                .ToListAsync();

            if (request.Category is not BuildingCategory.Undefined)
            {
                buildings = buildings
                    .Where(x => x.Category() == BuildingCategory.Farm)
                    .ToList();
            }

            return buildings;
        }
    }
}