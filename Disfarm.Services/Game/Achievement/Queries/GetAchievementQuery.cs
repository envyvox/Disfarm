using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Disfarm.Data;
using Disfarm.Services.Game.Achievement.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using CacheExtensions = Disfarm.Services.Extensions.CacheExtensions;

namespace Disfarm.Services.Game.Achievement.Queries
{
	public record GetAchievementQuery(Data.Enums.Achievement Type) : IRequest<AchievementDto>;

	public class GetAchievementHandler : IRequestHandler<GetAchievementQuery, AchievementDto>
	{
		private readonly IMapper _mapper;
		private readonly IMemoryCache _cache;
		private readonly AppDbContext _db;

		public GetAchievementHandler(
			DbContextOptions options,
			IMapper mapper,
			IMemoryCache cache)
		{
			_mapper = mapper;
			_cache = cache;
			_db = new AppDbContext(options);
		}

		public async Task<AchievementDto> Handle(GetAchievementQuery request, CancellationToken ct)
		{
			if (_cache.TryGetValue(CacheExtensions.GetAchievementKey(request.Type),
					out AchievementDto achievement)) return achievement;

			var entity = await _db.Achievements
				.SingleOrDefaultAsync(x => x.Type == request.Type);

			if (entity is null)
			{
				throw new Exception(
					$"achievement {request.Type.ToString()} not found in database");
			}

			achievement = _mapper.Map<AchievementDto>(entity);

			_cache.Set(CacheExtensions.GetAchievementKey(request.Type), achievement,
				CacheExtensions.DefaultCacheOptions);

			return achievement;
		}
	}
}