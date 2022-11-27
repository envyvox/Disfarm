using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Interactions;
using Disfarm.Data.Enums;
using Disfarm.Services.Discord.Emote.Extensions;
using Disfarm.Services.Discord.Extensions;
using Disfarm.Services.Extensions;
using Disfarm.Services.Game.Currency.Commands;
using Disfarm.Services.Game.Currency.Queries;
using Disfarm.Services.Game.Localization;
using Disfarm.Services.Game.Product.Commands;
using Disfarm.Services.Game.Product.Queries;
using Disfarm.Services.Game.User.Queries;
using MediatR;
using static Discord.Emote;

namespace Disfarm.Services.Discord.Interactions.Commands.Shop
{
	public class ShopProduct : InteractionModuleBase<SocketInteractionContext>
	{
		private readonly IMediator _mediator;
		private readonly ILocalizationService _local;
		private const uint BuyAmount = 5;

		public ShopProduct(IMediator mediator, ILocalizationService local)
		{
			_mediator = mediator;
			_local = local;
		}

		[SlashCommand("shop-products", "Purchase various cooking products")]
		public async Task Execute()
		{
			await DeferAsync(true);

			var emotes = DiscordRepository.Emotes;
			var user = await _mediator.Send(new GetUserQuery((long) Context.User.Id));
			var products = await _mediator.Send(new GetProductsQuery());

			var embed = new EmbedBuilder()
				.WithUserColor(user.CommandColor)
				.WithAuthor(Response.ShopProductAuthor.Parse(user.Language), Context.User.GetAvatarUrl())
				.WithDescription(
					Response.ShopProductDesc.Parse(user.Language,
						Context.User.Mention.AsGameMention(user.Title, user.Language), emotes.GetEmote("Arrow")) +
					$"\n{StringExtensions.EmptyChar}");

			var selectMenu = new SelectMenuBuilder()
				.WithPlaceholder(Response.ComponentShopProductBuy.Parse(user.Language))
				.WithCustomId("shop-products-buy");

			foreach (var product in products)
			{
				embed.AddField(Response.ShopProductFieldDesc.Parse(user.Language,
						emotes.GetEmote(product.Name), BuyAmount,
						_local.Localize(LocalizationCategory.Product, product.Name, user.Language, BuyAmount),
						emotes.GetEmote(Currency.Token.ToString()), product.Price * BuyAmount,
						_local.Localize(LocalizationCategory.Currency, Currency.Token.ToString(), user.Language,
							product.Price * BuyAmount)),
					StringExtensions.EmptyChar);

				selectMenu.AddOption(
					_local.Localize(LocalizationCategory.Product, product.Name, user.Language, BuyAmount),
					product.Id.ToString(),
					emote: Parse(emotes.GetEmote(product.Name)));
			}

			var components = new ComponentBuilder()
				.WithSelectMenu(selectMenu)
				.Build();

			await Context.Interaction.FollowUpResponse(embed, components);
		}

		[ComponentInteraction("shop-products-buy")]
		public async Task Execute(string[] selectedValues)
		{
			await DeferAsync();

			var productId = Guid.Parse(selectedValues.First());

			var emotes = DiscordRepository.Emotes;
			var user = await _mediator.Send(new GetUserQuery((long) Context.User.Id));
			var product = await _mediator.Send(new GetProductQuery(productId));
			var userCurrency = await _mediator.Send(new GetUserCurrencyQuery(user.Id, Currency.Token));

			if (product.Price * BuyAmount > userCurrency.Amount)
			{
				throw new Exception(Response.ShopProductNoCurrency.Parse(user.Language,
					emotes.GetEmote(Currency.Token.ToString()),
					_local.Localize(LocalizationCategory.Currency, Currency.Token.ToString(), user.Language, 5),
					emotes.GetEmote(product.Name),
					_local.Localize(LocalizationCategory.Product, product.Name, user.Language, 5)));
			}

			await _mediator.Send(new RemoveCurrencyFromUserCommand(user.Id, Currency.Token, product.Price * BuyAmount));
			await _mediator.Send(new AddProductToUserCommand(user.Id, product.Id, BuyAmount));

			var embed = new EmbedBuilder()
				.WithUserColor(user.CommandColor)
				.WithAuthor(Response.ShopProductBuyAuthor.Parse(user.Language))
				.WithDescription(Response.ShopProductBuySuccess.Parse(user.Language,
					Context.User.Mention.AsGameMention(user.Title, user.Language),
					emotes.GetEmote(product.Name), BuyAmount,
					_local.Localize(LocalizationCategory.Product, product.Name, user.Language, BuyAmount),
					emotes.GetEmote(Currency.Token.ToString()), product.Price * BuyAmount,
					_local.Localize(LocalizationCategory.Currency, Currency.Token.ToString(), user.Language,
						product.Price * BuyAmount)));

			await Context.Interaction.ClearOriginalResponse(user.Language);
			await Context.Interaction.FollowUpResponse(embed);
		}
	}
}