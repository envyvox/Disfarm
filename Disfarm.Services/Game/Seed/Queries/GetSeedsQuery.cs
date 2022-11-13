using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Disfarm.Data;
using Disfarm.Services.Game.Seed.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using CacheExtensions = Disfarm.Services.Extensions.CacheExtensions;

namespace Disfarm.Services.Game.Seed.Queries
{
    public record GetSeedsQuery : IRequest<List<SeedDto>>;

    public class GetSeedsHandler : IRequestHandler<GetSeedsQuery, List<SeedDto>>
    {
        private readonly IMapper _mapper;
        private readonly IMemoryCache _cache;
        private readonly IServiceScopeFactory _scopeFactory;

        public GetSeedsHandler(
            IServiceScopeFactory scopeFactory,
            IMapper mapper,
            IMemoryCache cache)
        {
            _scopeFactory = scopeFactory;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task<List<SeedDto>> Handle(GetSeedsQuery request, CancellationToken ct)
        {
            if (_cache.TryGetValue(CacheExtensions.GetSeedsKey(), out List<SeedDto> seeds)) return seeds;

            using var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var entities = await db.Seeds
                .Include(x => x.Crop)
                .OrderBy(x => x.Name)
                .ToListAsync();

            seeds = _mapper.Map<List<SeedDto>>(entities);

            _cache.Set(CacheExtensions.GetSeedsKey(), seeds, CacheExtensions.DefaultCacheOptions);

            return seeds;
        }
    }
}