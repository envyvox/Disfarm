using System.Threading.Tasks;
using Discord;
using Discord.Interactions;
using Disfarm.Data.Enums;
using Disfarm.Services.Discord.Emote.Extensions;
using Disfarm.Services.Discord.Extensions;
using Disfarm.Services.Discord.Interactions.Attributes;
using Disfarm.Services.Extensions;
using Disfarm.Services.Game.Banner.Queries;
using Disfarm.Services.Game.Calculation;
using Disfarm.Services.Game.Transit.Queries;
using Disfarm.Services.Game.User.Queries;
using MediatR;
using static Discord.Emote;

namespace Disfarm.Services.Discord.Interactions.Commands.UserInfo
{
	[RequireGuildContext]
	public class UserProfile : InteractionModuleBase<SocketInteractionContext>
	{
		private readonly IMediator _mediator;

		public UserProfile(IMediator mediator)
		{
			_mediator = mediator;
		}

		[SlashCommand("profile", "View your profile.")]
		public async Task Execute()
		{
			await DeferAsync(true);

			var emotes = DiscordRepository.Emotes;
			var user = await _mediator.Send(new GetUserQuery((long)Context.User.Id));
			var banner = await _mediator.Send(new GetUserActiveBannerQuery(user.Id));
			var requiredXp = await _mediator.Send(new GetRequiredXpQuery(user.Level + 1));

			string locationString;
			switch (user.Location)
			{
				case Location.InTransit:
					{
						var userMovement = await _mediator.Send(new GetUserMovementQuery(user.Id));

						locationString = Response.UserProfileLocationTransit.Parse(user.Language,
							emotes.GetEmote(userMovement.Departure.EmoteName()),
							userMovement.Departure.Localize(user.Language),
							emotes.GetEmote(userMovement.Destination.EmoteName()),
							userMovement.Destination.Localize(user.Language),
							userMovement.Arrival.ToDiscordTimestamp(TimestampFormat.RelativeTime));

						break;
					}

				case Location.WorkOnContract:
					{
						locationString = "";
						break;
					}

				case Location.Fishing:
				case Location.FarmWatering:
					{
						var userMovement = await _mediator.Send(new GetUserMovementQuery(user.Id));

						locationString = Response.UserProfileLocationFishingAndFarmWatering.Parse(user.Language,
							emotes.GetEmote(user.Location.EmoteName()), user.Location.Localize(user.Language),
							userMovement.Arrival.ToDiscordTimestamp(TimestampFormat.RelativeTime));

						break;
					}

				default:
					{
						locationString =
							$"{emotes.GetEmote(user.Location.EmoteName())} **{user.Location.Localize(user.Language, true)}**";
						break;
					}
			}

			var embed = new EmbedBuilder()
				.WithUserColor(user.CommandColor)
				.WithAuthor(Response.UserProfileAuthor.Parse(user.Language), Context.User.GetAvatarUrl())
				.WithThumbnailUrl(Context.User.GetAvatarUrl())
				.WithDescription(
					$"{Context.User.Mention.AsGameMention(user.Title, user.Language)}")
				.AddField(Response.UserProfileFractionTitle.Parse(user.Language),
					$"{emotes.GetEmote(user.Fraction.EmoteName())} {user.Fraction.Localize(user.Language)}", true)
				.AddField(Response.UserProfileLocationTitle.Parse(user.Language),
					locationString +
					$"\n{StringExtensions.EmptyChar}")
				.AddField(Response.UserProfileLevelTitle.Parse(user.Language),
					Response.UserProfileLevelDescLevel.Parse(user.Language,
						user.Level.AsLevelEmote(), user.Level, emotes.GetEmote("Xp"), user.Xp) +
					(user.Level < 100
						? Response.UserProfileLevelDescNextLevel.Parse(user.Language,
							emotes.GetEmote("Arrow"), emotes.GetEmote("Xp"), requiredXp - user.Xp)
						: "") +
					$"\n{StringExtensions.EmptyChar}")
				.AddField(Response.UserProfileCreatedAtTitle.Parse(user.Language),
					user.CreatedAt.ToDiscordTimestamp(TimestampFormat.LongDate))
				.AddField(Response.UserProfileAboutTitle.Parse(user.Language),
					user.About ?? Response.UserProfileAboutDesc.Parse(user.Language))
				.WithImageUrl(banner.Url);

			var components = new ComponentBuilder()
				.WithButton(
					Response.ComponentUserProfileUpdateAboutLabel.Parse(user.Language),
					"user-profile-update-about")
				.WithButton(
					Response.ComponentUserProfileUpdateCommandColor.Parse(user.Language),
					"user-profile-update-commandcolor",
					emote: Parse(emotes.GetEmote("Premium")),
					disabled: user.IsPremium is false);

			await Context.Interaction.FollowUpResponse(embed, components.Build());
		}
	}
}