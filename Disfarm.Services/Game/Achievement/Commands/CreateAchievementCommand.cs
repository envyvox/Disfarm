using System;
using System.Threading;
using System.Threading.Tasks;
using Disfarm.Data;
using Disfarm.Data.Enums;
using Disfarm.Data.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Disfarm.Services.Game.Achievement.Commands
{
	public record CreateAchievementCommand(
			Data.Enums.Achievement Type,
			AchievementRewardType RewardType,
			uint RewardNumber,
			uint Points)
		: IRequest;

	public class CreateAchievementHandler : IRequestHandler<CreateAchievementCommand>
	{
		private readonly ILogger<CreateAchievementHandler> _logger;
		private readonly AppDbContext _db;

		public CreateAchievementHandler(
			DbContextOptions options,
			ILogger<CreateAchievementHandler> logger)
		{
			_db = new AppDbContext(options);
			_logger = logger;
		}

		public async Task<Unit> Handle(CreateAchievementCommand request, CancellationToken ct)
		{
			var exist = await _db.Achievements.AnyAsync(x => x.Type == request.Type);

			if (exist)
			{
				throw new Exception(
					$"Achievement {request.Type.ToString()} already exist");
			}

			var created = await _db.CreateEntity(new Data.Entities.Achievement
			{
				Type = request.Type,
				RewardType = request.RewardType,
				RewardNumber = request.RewardNumber,
				Points = request.Points,
				CreatedAt = DateTimeOffset.UtcNow
			});

			_logger.LogInformation(
				"Created achievement entity {@Entity}",
				created);

			return Unit.Value;
		}
	}
}