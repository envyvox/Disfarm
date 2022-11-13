using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Disfarm.Data;
using Disfarm.Data.Enums;
using Disfarm.Services.Game.Fish.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using CacheExtensions = Disfarm.Services.Extensions.CacheExtensions;

namespace Disfarm.Services.Game.Fish.Queries
{
    public record GetFishesWithSeasonQuery(Season Season) : IRequest<List<FishDto>>;

    public class GetFishesWithSeasonHandler : IRequestHandler<GetFishesWithSeasonQuery, List<FishDto>>
    {
        private readonly IMapper _mapper;
        private readonly IMemoryCache _cache;
        private readonly IServiceScopeFactory _scopeFactory;

        public GetFishesWithSeasonHandler(
            IServiceScopeFactory scopeFactory,
            IMapper mapper,
            IMemoryCache cache)
        {
            _scopeFactory = scopeFactory;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task<List<FishDto>> Handle(GetFishesWithSeasonQuery request, CancellationToken ct)
        {
            if (_cache.TryGetValue(CacheExtensions.GetFishesWithSeasonKey(request.Season),
                    out List<FishDto> fishes)) return fishes;

            using var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var entities = await db.Fishes
                .AsQueryable()
                .ToListAsync();

            var filteredEntities = entities
                .Where(x =>
                    x.CatchSeasons.Contains(Season.Any) ||
                    x.CatchSeasons.Contains(request.Season))
                .ToList();

            fishes = _mapper.Map<List<FishDto>>(filteredEntities);

            _cache.Set(CacheExtensions.GetFishesWithSeasonKey(request.Season), fishes,
                CacheExtensions.DefaultCacheOptions);

            return fishes;
        }
    }
}