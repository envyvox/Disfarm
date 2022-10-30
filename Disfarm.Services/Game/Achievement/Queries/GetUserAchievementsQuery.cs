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

namespace Disfarm.Services.Game.Achievement.Queries
{
	public record GetUserAchievementsQuery(
			long UserId,
			AchievementCategory Category)
		: IRequest<List<UserAchievementDto>>;

	public class GetUserAchievementsHandler : IRequestHandler<GetUserAchievementsQuery, List<UserAchievementDto>>
	{
		private readonly IMapper _mapper;
		private readonly AppDbContext _db;

		public GetUserAchievementsHandler(
			DbContextOptions options,
			IMapper mapper)
		{
			_db = new AppDbContext(options);
			_mapper = mapper;
		}

		public async Task<List<UserAchievementDto>> Handle(GetUserAchievementsQuery request, CancellationToken ct)
		{
			var entities = await _db.UserAchievements
				.Include(x => x.Achievement)
				.Where(x => x.UserId == request.UserId)
				.ToListAsync();

			entities = entities.Where(x => x.Achievement.Type.Category() == request.Category).ToList();

			return _mapper.Map<List<UserAchievementDto>>(entities);
		}
	}
}