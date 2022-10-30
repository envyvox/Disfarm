using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Disfarm.Data;
using Disfarm.Data.Enums;
using Disfarm.Services.Game.Localization.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using CacheExtensions = Disfarm.Services.Extensions.CacheExtensions;

namespace Disfarm.Services.Game.Localization.Queries
{
	public record GetLocalizationsByCategoryQuery(
			LocalizationCategory Category,
			Language Language)
		: IRequest<List<LocalizationDto>>;

	public class GetLocalizationsByCategoryHandler
		: IRequestHandler<GetLocalizationsByCategoryQuery, List<LocalizationDto>>
	{
		private readonly IMapper _mapper;
		private readonly IMemoryCache _cache;
		private readonly AppDbContext _db;

		public GetLocalizationsByCategoryHandler(
			DbContextOptions options,
			IMapper mapper,
			IMemoryCache cache)
		{
			_db = new AppDbContext(options);
			_mapper = mapper;
			_cache = cache;
		}

		public async Task<List<LocalizationDto>> Handle(GetLocalizationsByCategoryQuery request, CancellationToken ct)
		{
			if (_cache.TryGetValue(
					CacheExtensions.GetLocalizationsInCategoryKey(request.Category, request.Language),
					out List<LocalizationDto> localizations)) return localizations;

			var entities = await _db.Localizations
				.AsQueryable()
				.Where(x =>
					x.Category == request.Category &&
					x.Language == request.Language)
				.ToListAsync();

			localizations = _mapper.Map<List<LocalizationDto>>(entities);

			_cache.Set(CacheExtensions.GetLocalizationsInCategoryKey(request.Category, request.Language),
				localizations, CacheExtensions.DefaultCacheOptions);

			return localizations;
		}
	}
}