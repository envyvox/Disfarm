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
using Disfarm.Services.Game.Banner.Commands;
using Disfarm.Services.Game.Banner.Queries;
using Disfarm.Services.Game.Currency.Commands;
using Disfarm.Services.Game.Currency.Queries;
using Disfarm.Services.Game.Localization;
using Disfarm.Services.Game.User.Queries;
using MediatR;
using static Disfarm.Services.Extensions.ExceptionExtensions;

namespace Disfarm.Services.Discord.Interactions.Components.Shop
{
	[RequireLocation(Location.Neutral)]
	public class ShopBannerBuy : InteractionModuleBase<SocketInteractionContext>
	{
		private readonly IMediator _mediator;
		private readonly ILocalizationService _local;

		public ShopBannerBuy(
			IMediator mediator,
			ILocalizationService local)
		{
			_mediator = mediator;
			_local = local;
		}

		[ComponentInteraction("shop-banner-buy-currency:*")]
		public async Task ShopBannerBuyTask(string currencyHashcode, string[] selectedValues)
		{
			await DeferAsync();

			var currency = (Currency)int.Parse(currencyHashcode);
			var bannerId = Guid.Parse(selectedValues.First());

			var emotes = DiscordRepository.Emotes;
			var user = await _mediator.Send(new GetUserQuery((long)Context.User.Id));
			var banner = await _mediator.Send(new GetBannerQuery(bannerId));
			var userCurrency = await _mediator.Send(new GetUserCurrencyQuery(user.Id, currency));
			var bannerPrice = currency is Currency.Token ? banner.Price : banner.Price.ConvertTokensToChips();

			if (userCurrency.Amount < bannerPrice)
			{
				throw new GameUserExpectedException(Response.ShopBannerBuyNoCurrency.Parse(user.Language,
					emotes.GetEmote(currency.ToString()),
					_local.Localize(LocalizationCategory.Currency, currency.ToString(), user.Language, 5),
					emotes.GetEmote(banner.Rarity.EmoteName()), banner.Rarity.Localize(user.Language, true),
					_local.Localize(LocalizationCategory.Banner, banner.Name, user.Language)));
			}

			await _mediator.Send(new RemoveCurrencyFromUserCommand(user.Id, currency, bannerPrice));
			await _mediator.Send(new AddBannerToUserCommand(user.Id, banner.Id, TimeSpan.FromDays(30)));

			var embed = new EmbedBuilder()
				.WithUserColor(user.CommandColor)
				.WithAuthor(Response.ShopBannerBuyAuthor.Parse(user.Language), Context.User.GetAvatarUrl())
				.WithDescription(Response.ShopBannerBuyDesc.Parse(user.Language,
					Context.User.Mention.AsGameMention(user.Title, user.Language),
					emotes.GetEmote(banner.Rarity.EmoteName()), banner.Rarity.Localize(user.Language).ToLower(),
					_local.Localize(LocalizationCategory.Banner, banner.Name, user.Language),
					emotes.GetEmote(currency.ToString()), bannerPrice,
					_local.Localize(LocalizationCategory.Currency, currency.ToString(), user.Language, bannerPrice),
					emotes.GetEmote("Arrow")))
				.WithImageUrl(await _mediator.Send(new GetImageUrlQuery(Data.Enums.Image.ShopBanner, user.Language)));

			await Context.Interaction.FollowUpResponse(embed);
			await Context.Interaction.ClearOriginalResponse(user.Language);
		}
	}
}