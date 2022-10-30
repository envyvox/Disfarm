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
using CacheExtensions = Disfarm.Services.Extensions.CacheExtensions;

namespace Disfarm.Services.Game.Fish.Queries
{
	public record GetFishesQuery : IRequest<List<FishDto>>;

	public class GetFishesHandler : IRequestHandler<GetFishesQuery, List<FishDto>>
	{
		private readonly IMapper _mapper;
		private readonly IMemoryCache _cache;
		private readonly AppDbContext _db;

		public GetFishesHandler(
			DbContextOptions options,
			IMapper mapper,
			IMemoryCache cache)
		{
			_db = new AppDbContext(options);
			_mapper = mapper;
			_cache = cache;
		}

		public async Task<List<FishDto>> Handle(GetFishesQuery request, CancellationToken ct)
		{
			if (_cache.TryGetValue(CacheExtensions.GetFishesKey(), out List<FishDto> fishes)) return fishes;

			var entities = await _db.Fishes
				.AsQueryable()
				.OrderBy(x => x.Name)
				.ToListAsync();

			fishes = _mapper.Map<List<FishDto>>(entities);

			_cache.Set(CacheExtensions.GetFishesKey(), fishes, CacheExtensions.DefaultCacheOptions);

			return fishes;
		}
	}
}