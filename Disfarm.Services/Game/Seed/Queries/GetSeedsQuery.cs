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
using CacheExtensions = Disfarm.Services.Extensions.CacheExtensions;

namespace Disfarm.Services.Game.Seed.Queries
{
	public record GetSeedsQuery : IRequest<List<SeedDto>>;

	public class GetSeedsHandler : IRequestHandler<GetSeedsQuery, List<SeedDto>>
	{
		private readonly IMapper _mapper;
		private readonly IMemoryCache _cache;
		private readonly AppDbContext _db;

		public GetSeedsHandler(
			DbContextOptions options,
			IMapper mapper,
			IMemoryCache cache)
		{
			_db = new AppDbContext(options);
			_mapper = mapper;
			_cache = cache;
		}

		public async Task<List<SeedDto>> Handle(GetSeedsQuery request, CancellationToken ct)
		{
			if (_cache.TryGetValue(CacheExtensions.GetSeedsKey(), out List<SeedDto> seeds)) return seeds;

			var entities = await _db.Seeds
				.Include(x => x.Crop)
				.OrderBy(x => x.Name)
				.ToListAsync();

			seeds = _mapper.Map<List<SeedDto>>(entities);

			_cache.Set(CacheExtensions.GetSeedsKey(), seeds, CacheExtensions.DefaultCacheOptions);

			return seeds;
		}
	}
}