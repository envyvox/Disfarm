using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Disfarm.Data.Enums;
using Disfarm.Data.Enums.Achievement;
using Disfarm.Data.Util;
using Disfarm.Services.Game.Achievement.Commands;
using MediatR;

namespace Disfarm.Services.Seeder
{
	public record SeedAchievementsCommand : IRequest<TotalAndAffectedCountDto>;

	public class SeedAchievementsHandler : IRequestHandler<SeedAchievementsCommand, TotalAndAffectedCountDto>
	{
		private readonly IMediator _mediator;

		public SeedAchievementsHandler(IMediator mediator)
		{
			_mediator = mediator;
		}

		public async Task<TotalAndAffectedCountDto> Handle(SeedAchievementsCommand request, CancellationToken ct)
		{
			var result = new TotalAndAffectedCountDto();
			var achievements = Enum
				.GetValues(typeof(Achievement))
				.Cast<Achievement>();

			var commands = achievements.Select(achievement =>
				new CreateAchievementCommand(achievement, AchievementRewardType.Chip, 1, 1)).ToList();

			foreach (var createAchievementCommand in commands)
			{
				result.Total++;

				try
				{
					await _mediator.Send(createAchievementCommand);

					result.Affected++;
				}
				catch
				{
					// ignored
				}
			}

			return result;
		}
	}
}