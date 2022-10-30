using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Disfarm.Data;
using Disfarm.Data.Enums;
using Disfarm.Services.Game.Seed.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using CacheExtensions = Disfarm.Services.Extensions.CacheExtensions;

namespace Disfarm.Services.Game.Seed.Queries
{
	public record GetSeedsBySeasonQuery(Season Season) : IRequest<List<SeedDto>>;

	public class GetSeedsBySeasonHandler : IRequestHandler<GetSeedsBySeasonQuery, List<SeedDto>>
	{
		private readonly IMapper _mapper;
		private readonly IMemoryCache _cache;
		private readonly AppDbContext _db;

		public GetSeedsBySeasonHandler(
			DbContextOptions options,
			IMapper mapper,
			IMemoryCache cache)
		{
			_db = new AppDbContext(options);
			_mapper = mapper;
			_cache = cache;
		}

		public async Task<List<SeedDto>> Handle(GetSeedsBySeasonQuery request, CancellationToken cancellationToken)
		{
			if (_cache.TryGetValue(CacheExtensions.GetSeedsWithSeasonKey(request.Season),
					out List<SeedDto> seeds)) return seeds;

			var entities = await _db.Seeds
				.Include(x => x.Crop)
				.OrderBy(x => x.Name)
				.Where(x => x.Season == request.Season)
				.ToListAsync();

			seeds = _mapper.Map<List<SeedDto>>(entities);

			_cache.Set(CacheExtensions.GetSeedsWithSeasonKey(request.Season), seeds,
				CacheExtensions.DefaultCacheOptions);

			return seeds;
		}
	}
}