using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Disfarm.Data;
using Disfarm.Services.Game.World.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using CacheExtensions = Disfarm.Services.Extensions.CacheExtensions;

namespace Disfarm.Services.Game.World.Queries
{
    public record GetWorldStateQuery : IRequest<WorldStateDto>;

    public class GetWorldStateHandler : IRequestHandler<GetWorldStateQuery, WorldStateDto>
    {
        private readonly IMapper _mapper;
        private readonly IMemoryCache _cache;
        private readonly IServiceScopeFactory _scopeFactory;

        public GetWorldStateHandler(
            IServiceScopeFactory scopeFactory,
            IMapper mapper,
            IMemoryCache cache)
        {
            _scopeFactory = scopeFactory;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task<WorldStateDto> Handle(GetWorldStateQuery request, CancellationToken ct)
        {
            if (_cache.TryGetValue(CacheExtensions.GetWorldStateKey(), out WorldStateDto state))
                return state;

            using var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var entity = await db.WorldStates.FirstAsync();

            state = _mapper.Map<WorldStateDto>(entity);

            _cache.Set(CacheExtensions.GetWorldStateKey(), state, CacheExtensions.DefaultCacheOptions);

            return state;
        }
    }
}