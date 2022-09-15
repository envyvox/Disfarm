using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Disfarm.Data;
using Disfarm.Services.Game.Seed.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using CacheExtensions = Disfarm.Services.Extensions.CacheExtensions;

namespace Disfarm.Services.Game.Seed.Queries
{
    public record GetSeedQuery(Guid Id) : IRequest<SeedDto>;

    public class GetSeedHandler : IRequestHandler<GetSeedQuery, SeedDto>
    {
        private readonly IMapper _mapper;
        private readonly IMemoryCache _cache;
        private readonly AppDbContext _db;

        public GetSeedHandler(
            DbContextOptions options,
            IMapper mapper,
            IMemoryCache cache)
        {
            _db = new AppDbContext(options);
            _mapper = mapper;
            _cache = cache;
        }

        public async Task<SeedDto> Handle(GetSeedQuery request, CancellationToken ct)
        {
            if (_cache.TryGetValue(string.Format(CacheExtensions.SeedKey, request.Id), out SeedDto seed)) return seed;

            var entity = await _db.Seeds
                .Include(x => x.Crop)
                .SingleOrDefaultAsync(x => x.Id == request.Id);

            if (entity is null)
            {
                throw new Exception(
                    $"seed {request.Id} not found");
            }

            seed = _mapper.Map<SeedDto>(entity);

            _cache.Set(string.Format(CacheExtensions.SeedKey, request.Id), seed, CacheExtensions.DefaultCacheOptions);

            return seed;
        }
    }
}