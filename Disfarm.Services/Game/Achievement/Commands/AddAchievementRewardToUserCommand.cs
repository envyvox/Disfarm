using System;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Disfarm.Data.Enums;
using Disfarm.Data.Enums.Achievement;
using Disfarm.Services.Discord.Client.Queries;
using Disfarm.Services.Discord.Embed;
using Disfarm.Services.Discord.Emote.Extensions;
using Disfarm.Services.Extensions;
using Disfarm.Services.Game.Achievement.Queries;
using Disfarm.Services.Game.Currency.Commands;
using Disfarm.Services.Game.Localization;
using Disfarm.Services.Game.Title.Commands;
using Disfarm.Services.Game.User.Queries;
using MediatR;

namespace Disfarm.Services.Game.Achievement.Commands
{
	public record AddAchievementRewardToUserCommand(
			long UserId,
			Data.Enums.Achievement.Achievement Type)
		: IRequest;

	public class AddAchievementRewardToUserHandler : IRequestHandler<AddAchievementRewardToUserCommand>
	{
		private readonly IMediator _mediator;
		private readonly ILocalizationService _local;

		public AddAchievementRewardToUserHandler(
			IMediator mediator,
			ILocalizationService local)
		{
			_mediator = mediator;
			_local = local;
		}

		public async Task<Unit> Handle(AddAchievementRewardToUserCommand request, CancellationToken ct)
		{
			var emotes = DiscordRepository.Emotes;
			var user = await _mediator.Send(new GetUserQuery(request.UserId));
			var socketUser = await _mediator.Send(new GetClientUserQuery((ulong)user.Id));
			var achievement = await _mediator.Send(new GetAchievementQuery(request.Type));

			string rewardString;
			switch (achievement.RewardType)
			{
				case AchievementRewardType.Chip:
					{
						await _mediator.Send(new AddCurrencyToUserCommand(
							user.Id, Data.Enums.Currency.Chip, achievement.RewardNumber));

						rewardString = Response.AchievementRewardChip.Parse(user.Language,
							emotes.GetEmote(Data.Enums.Currency.Chip.ToString()), achievement.RewardNumber,
							_local.Localize(LocalizationCategory.Currency, Data.Enums.Currency.Chip.ToString(),
								user.Language, achievement.RewardNumber), emotes.GetEmote("Arrow"));

						break;
					}
				case AchievementRewardType.Title:
					{
						var title = (Data.Enums.Title)achievement.RewardNumber;

						await _mediator.Send(new AddTitleToUserCommand(user.Id, title));

						rewardString = Response.AchievementRewardTitle.Parse(user.Language,
							emotes.GetEmote(title.EmoteName()), title.Localize(user.Language), emotes.GetEmote("Arrow"));

						break;
					}
				default:
					throw new ArgumentOutOfRangeException();
			}

			var embed = new EmbedBuilder()
				.WithUserColor(user.CommandColor)
				.WithAuthor(Response.AchievementAuthor.Parse(user.Language), socketUser.GetAvatarUrl())
				.WithDescription(Response.AchievementDesc.Parse(user.Language,
					socketUser.Mention.AsGameMention(user.Title, user.Language), emotes.GetEmote("Achievement"),
					achievement.Type.Localize(user.Language), achievement.Type.Category().Localize(user.Language),
					rewardString, emotes.GetEmote("Arrow")));

			return await _mediator.Send(new SendEmbedToUserCommand(socketUser.Id, embed));
		}
	}
}