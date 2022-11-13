using System;
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
    public record GetSeedQuery(Guid Id) : IRequest<SeedDto>;

    public class GetSeedHandler : IRequestHandler<GetSeedQuery, SeedDto>
    {
        private readonly IMapper _mapper;
        private readonly IMemoryCache _cache;
        private readonly IServiceScopeFactory _scopeFactory;

        public GetSeedHandler(
            IServiceScopeFactory scopeFactory,
            IMapper mapper,
            IMemoryCache cache)
        {
            _scopeFactory = scopeFactory;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task<SeedDto> Handle(GetSeedQuery request, CancellationToken ct)
        {
            if (_cache.TryGetValue(CacheExtensions.GetSeedByIdKey(request.Id), out SeedDto seed)) return seed;

            using var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var entity = await db.Seeds
                .Include(x => x.Crop)
                .SingleOrDefaultAsync(x => x.Id == request.Id);

            if (entity is null)
            {
                throw new Exception(
                    $"seed {request.Id} not found");
            }

            seed = _mapper.Map<SeedDto>(entity);

            _cache.Set(CacheExtensions.GetSeedByIdKey(request.Id), seed, CacheExtensions.DefaultCacheOptions);

            return seed;
        }
    }
}