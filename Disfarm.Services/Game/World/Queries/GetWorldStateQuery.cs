using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Disfarm.Data;
using Disfarm.Services.Game.World.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using CacheExtensions = Disfarm.Services.Extensions.CacheExtensions;

namespace Disfarm.Services.Game.World.Queries
{
	public record GetWorldStateQuery : IRequest<WorldStateDto>;

	public class GetWorldStateHandler : IRequestHandler<GetWorldStateQuery, WorldStateDto>
	{
		private readonly IMapper _mapper;
		private readonly IMemoryCache _cache;
		private readonly AppDbContext _db;

		public GetWorldStateHandler(
			DbContextOptions options,
			IMapper mapper,
			IMemoryCache cache)
		{
			_db = new AppDbContext(options);
			_mapper = mapper;
			_cache = cache;
		}

		public async Task<WorldStateDto> Handle(GetWorldStateQuery request, CancellationToken ct)
		{
			if (_cache.TryGetValue(CacheExtensions.GetWorldStateKey(), out WorldStateDto state))
				return state;

			var entity = await _db.WorldStates.FirstAsync();

			state = _mapper.Map<WorldStateDto>(entity);

			_cache.Set(CacheExtensions.GetWorldStateKey(), state, CacheExtensions.DefaultCacheOptions);

			return state;
		}
	}
}