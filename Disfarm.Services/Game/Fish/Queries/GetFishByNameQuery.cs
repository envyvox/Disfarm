using System;
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
    public record GetFishByNameQuery(string Name) : IRequest<FishDto>;

    public class GetFishByNameHandler : IRequestHandler<GetFishByNameQuery, FishDto>
    {
        private readonly IMapper _mapper;
        private readonly IMemoryCache _cache;
        private readonly IServiceScopeFactory _scopeFactory;

        public GetFishByNameHandler(
            IServiceScopeFactory scopeFactory,
            IMapper mapper,
            IMemoryCache cache)
        {
            _scopeFactory = scopeFactory;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task<FishDto> Handle(GetFishByNameQuery request, CancellationToken ct)
        {
            if (_cache.TryGetValue(CacheExtensions.GetFishByNameKey(request.Name), out FishDto fish))
                return fish;

            using var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var entity = await db.Fishes
                .SingleOrDefaultAsync(x => x.Name == request.Name);

            if (entity is null)
            {
                throw new Exception(
                    $"fish with name {request.Name} not found");
            }

            fish = _mapper.Map<FishDto>(entity);

            _cache.Set(CacheExtensions.GetFishByNameKey(request.Name), fish,
                CacheExtensions.DefaultCacheOptions);

            return fish;
        }
    }
}