using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Disfarm.Data;
using Disfarm.Services.Game.Fish.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using CacheExtensions = Disfarm.Services.Extensions.CacheExtensions;

namespace Disfarm.Services.Game.Fish.Queries
{
    public record GetFishesQuery : IRequest<List<FishDto>>;

    public class GetFishesHandler : IRequestHandler<GetFishesQuery, List<FishDto>>
    {
        private readonly IMapper _mapper;
        private readonly IMemoryCache _cache;
        private readonly IServiceScopeFactory _scopeFactory;

        public GetFishesHandler(
            IServiceScopeFactory scopeFactory,
            IMapper mapper,
            IMemoryCache cache)
        {
            _scopeFactory = scopeFactory;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task<List<FishDto>> Handle(GetFishesQuery request, CancellationToken ct)
        {
            if (_cache.TryGetValue(CacheExtensions.GetFishesKey(), out List<FishDto> fishes)) return fishes;

            using var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var entities = await db.Fishes
                .AsQueryable()
                .OrderBy(x => x.Name)
                .ToListAsync();

            fishes = _mapper.Map<List<FishDto>>(entities);

            _cache.Set(CacheExtensions.GetFishesKey(), fishes, CacheExtensions.DefaultCacheOptions);

            return fishes;
        }
    }
}