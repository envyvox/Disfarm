using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Interactions;
using Disfarm.Data.Enums;
using Disfarm.Services.Discord.Emote.Extensions;
using Disfarm.Services.Discord.Extensions;
using Disfarm.Services.Discord.Image.Queries;
using Disfarm.Services.Discord.Interactions.Attributes;
using Disfarm.Services.Extensions;
using Disfarm.Services.Game.Achievement.Queries;
using Disfarm.Services.Game.Localization;
using Disfarm.Services.Game.User.Queries;
using MediatR;

namespace Disfarm.Services.Discord.Interactions.Commands.UserInfo
{
	[RequireGuildContext]
	public class UserAchievements : InteractionModuleBase<SocketInteractionContext>
	{
		private readonly IMediator _mediator;
		private readonly ILocalizationService _local;

		public UserAchievements(
			IMediator mediator,
			ILocalizationService local)
		{
			_mediator = mediator;
			_local = local;
		}

		[SlashCommand("achievements", "View various achievements and rewards for completing them")]
		public async Task Execute(
			[Summary("category", "Achievement category you want to view")]
			AchievementCategory category)
		{
			await DeferAsync(true);

			var emotes = DiscordRepository.Emotes;
			var user = await _mediator.Send(new GetUserQuery((long)Context.User.Id));
			var achievements = await _mediator.Send(new GetAchievementsQuery(category));
			var userAchievements = await _mediator.Send(new GetUserAchievementsQuery(user.Id, category));

			var embed = new EmbedBuilder()
				.WithUserColor(user.CommandColor)
				.WithAuthor(Response.UserAchievementsAuthor.Parse(user.Language), Context.User.GetAvatarUrl())
				.WithDescription(
					Response.UserAchievementsDesc.Parse(user.Language,
						Context.User.Mention.AsGameMention(user.Title, user.Language),
						category.Localize(user.Language)) +
					$"\n{StringExtensions.EmptyChar}")
				.WithImageUrl(await _mediator.Send(new GetImageUrlQuery(
					Data.Enums.Image.UserAchievements, user.Language)));

			foreach (var achievement in achievements)
			{
				var userAchievement = userAchievements.SingleOrDefault(x => x.Achievement.Type == achievement.Type);
				var exist = userAchievement is not null;

				embed.AddField(
					$"{emotes.GetEmote("Achievement" + (exist ? "" : "BW"))} {achievement.Type.Localize(user.Language)}",
					achievement.RewardType switch
					{
						AchievementRewardType.Chip =>
							Response.UserAchievementsAchievementDescChip.Parse(user.Language,
								emotes.GetEmote(Currency.Chip.ToString()), achievement.RewardNumber,
								_local.Localize(LocalizationCategory.Currency, Currency.Chip.ToString(), user.Language,
									achievement.RewardNumber), achievement.Points),
						AchievementRewardType.Title =>
							Response.UserAchievementsAchievementDescTitle.Parse(user.Language,
								emotes.GetEmote(((Title)achievement.RewardNumber).EmoteName()),
								((Title)achievement.RewardNumber).Localize(user.Language), achievement.Points),
						_ => throw new ArgumentOutOfRangeException()
					} + (exist
						? Response.UserAchievementsAchievementCompleted.Parse(user.Language,
							emotes.GetEmote("Checkmark"),
							userAchievement.CreatedAt.ToDiscordTimestamp(TimestampFormat.ShortDateTime))
						: ""));
			}

			await Context.Interaction.FollowUpResponse(embed);
		}
	}
}