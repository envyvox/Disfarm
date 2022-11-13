using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Disfarm.Data;
using Disfarm.Services.Game.Crop.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using CacheExtensions = Disfarm.Services.Extensions.CacheExtensions;

namespace Disfarm.Services.Game.Crop.Queries
{
    public record GetCropsQuery : IRequest<List<CropDto>>;

    public class GetCropsHandler : IRequestHandler<GetCropsQuery, List<CropDto>>
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _cache;

        public GetCropsHandler(
            IServiceScopeFactory scopeFactory,
            IMapper mapper,
            IMemoryCache cache)
        {
            _scopeFactory = scopeFactory;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task<List<CropDto>> Handle(GetCropsQuery request, CancellationToken ct)
        {
            if (_cache.TryGetValue(CacheExtensions.GetCropsKey(), out List<CropDto> crops)) return crops;

            using var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var entities = await db.Crops
                .Include(x => x.Seed)
                .OrderBy(x => x.Name)
                .ToListAsync();

            crops = _mapper.Map<List<CropDto>>(entities);

            _cache.Set(CacheExtensions.GetCropsKey(), crops, CacheExtensions.DefaultCacheOptions);

            return crops;
        }
    }
}