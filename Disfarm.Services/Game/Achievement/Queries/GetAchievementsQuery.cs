using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Disfarm.Data;
using Disfarm.Data.Enums;
using Disfarm.Services.Game.Achievement.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using CacheExtensions = Disfarm.Services.Extensions.CacheExtensions;

namespace Disfarm.Services.Game.Achievement.Queries
{
	public record GetAchievementsQuery(AchievementCategory Category) : IRequest<List<AchievementDto>>;

	public class GetAchievementsHandler : IRequestHandler<GetAchievementsQuery, List<AchievementDto>>
	{
		private readonly IMapper _mapper;
		private readonly IMemoryCache _cache;
		private readonly AppDbContext _db;

		public GetAchievementsHandler(
			DbContextOptions options,
			IMapper mapper,
			IMemoryCache cache)
		{
			_db = new AppDbContext(options);
			_mapper = mapper;
			_cache = cache;
		}

		public async Task<List<AchievementDto>> Handle(GetAchievementsQuery request, CancellationToken ct)
		{
			if (_cache.TryGetValue(CacheExtensions.GetAchievementsInCategoryKey(request.Category),
					out List<AchievementDto> achievements)) return achievements;

			var entities = await _db.Achievements
				.AsQueryable()
				.OrderByDescending(x => x.CreatedAt)
				.ToListAsync();

			entities = entities.Where(x => x.Type.Category() == request.Category).ToList();

			achievements = _mapper.Map<List<AchievementDto>>(entities);

			_cache.Set(CacheExtensions.GetAchievementsInCategoryKey(request.Category), achievements,
				CacheExtensions.DefaultCacheOptions);

			return achievements;
		}
	}
}