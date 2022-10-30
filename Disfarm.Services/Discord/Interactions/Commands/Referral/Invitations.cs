using System.Threading.Tasks;
using Discord;
using Discord.Interactions;
using Disfarm.Data.Enums;
using Disfarm.Services.Discord.Client.Queries;
using Disfarm.Services.Discord.Emote.Extensions;
using Disfarm.Services.Discord.Extensions;
using Disfarm.Services.Discord.Image.Queries;
using Disfarm.Services.Discord.Interactions.Attributes;
using Disfarm.Services.Extensions;
using Disfarm.Services.Game.Localization;
using Disfarm.Services.Game.Referral.Queries;
using Disfarm.Services.Game.User.Queries;
using MediatR;
using static Discord.Emote;

namespace Disfarm.Services.Discord.Interactions.Commands.Referral
{
	[RequireGuildContext]
	public class Invitations : InteractionModuleBase<SocketInteractionContext>
	{
		private readonly IMediator _mediator;
		private readonly ILocalizationService _local;

		public Invitations(
			IMediator mediator,
			ILocalizationService local)
		{
			_mediator = mediator;
			_local = local;
		}

		[SlashCommand("invitations", "View information about your participation in the referral system")]
		public async Task Execute()
		{
			await DeferAsync(true);

			var emotes = DiscordRepository.Emotes;
			var user = await _mediator.Send(new GetUserQuery((long)Context.User.Id));
			var userReferrals = await _mediator.Send(new GetUserReferralsQuery(user.Id));
			var hasReferrer = await _mediator.Send(new CheckUserHasReferrerQuery(user.Id));

			string referrerString;
			if (hasReferrer)
			{
				var referrer = await _mediator.Send(new GetUserReferrerQuery(user.Id));
				var socketReferrer = await _mediator.Send(new GetClientUserQuery((ulong)referrer.Id));

				referrerString = Response.InvitationsReferrerDesc.Parse(user.Language,
					socketReferrer.Mention.AsGameMention(referrer.Title, user.Language),
					emotes.GetEmote(Container.Token.EmoteName()),
					_local.Localize(LocalizationCategory.Container, Container.Token.ToString(), user.Language));
			}
			else
			{
				referrerString = Response.InvitationsReferrerEmpty.Parse(user.Language,
					emotes.GetEmote("Arrow"), emotes.GetEmote(Container.Token.EmoteName()),
					_local.Localize(LocalizationCategory.Container, Container.Token.ToString(), user.Language));
			}

			var referralString = string.Empty;

			foreach (var userReferral in userReferrals)
			{
				var socketUserReferral = await _mediator.Send(new GetClientUserQuery((ulong)userReferral.Id));

				referralString +=
					$"{emotes.GetEmote("List")} {socketUserReferral.Mention.AsGameMention(userReferral.Title, user.Language)}\n";
			}

			var embed = new EmbedBuilder()
				.WithUserColor(user.CommandColor)
				.WithAuthor(Response.ReferralSystemAuthor.Parse(user.Language), Context.User.GetAvatarUrl())
				.WithDescription(
					Response.InvitationsDesc.Parse(user.Language,
						Context.User.Mention.AsGameMention(user.Title, user.Language)) +
					$"\n{StringExtensions.EmptyChar}")
				.AddField(Response.InvitationsReferrerTitle.Parse(user.Language),
					referrerString +
					$"\n{StringExtensions.EmptyChar}")
				.AddField(Response.InvitationsReferralsTitle.Parse(user.Language),
					referralString.Length > 0
						? referralString.Length > 1024
							? Response.InvitationsReferralsOutOfLimit.Parse(user.Language, userReferrals.Count)
							: referralString
						: Response.InvitationsReferralsEmpty.Parse(user.Language,
							emotes.GetEmote(Container.Token.EmoteName()), emotes.GetEmote(Currency.Chip.ToString())))
				.WithImageUrl(await _mediator.Send(new GetImageUrlQuery(Data.Enums.Image.Referral, user.Language)));

			var components = new ComponentBuilder()
				.WithButton(
					Response.ComponentReferralRewards.Parse(user.Language),
					"referral-rewards",
					ButtonStyle.Secondary,
					Parse(emotes.GetEmote("DiscordHelp")))
				.Build();

			await Context.Interaction.FollowUpResponse(embed, components);
		}
	}
}