using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Disfarm.Data;
using Disfarm.Services.Game.Seed.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Disfarm.Services.Game.Seed.Queries
{
    public record GetUserSeedsQuery(long UserId) : IRequest<List<UserSeedDto>>;

    public class GetUserSeedsHandler : IRequestHandler<GetUserSeedsQuery, List<UserSeedDto>>
    {
        private readonly IMapper _mapper;
        private readonly IServiceScopeFactory _scopeFactory;

        public GetUserSeedsHandler(
            IServiceScopeFactory scopeFactory,
            IMapper mapper)
        {
            _scopeFactory = scopeFactory;
            _mapper = mapper;
        }

        public async Task<List<UserSeedDto>> Handle(GetUserSeedsQuery request, CancellationToken ct)
        {
            using var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var entities = await db.UserSeeds
                .Include(x => x.Seed)
                .ThenInclude(x => x.Crop)
                .Where(x =>
                    x.UserId == request.UserId &&
                    x.Amount > 0)
                .ToListAsync();

            return _mapper.Map<List<UserSeedDto>>(entities);
        }
    }
}