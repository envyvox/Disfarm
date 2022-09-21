using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Interactions;
using Disfarm.Data.Enums;
using Disfarm.Services.Discord.Emote.Extensions;
using Disfarm.Services.Discord.Image.Queries;
using Disfarm.Services.Discord.Interactions.Attributes;
using Disfarm.Services.Extensions;
using Disfarm.Services.Game.Banner.Queries;
using Disfarm.Services.Game.Localization;
using Disfarm.Services.Game.User.Queries;
using MediatR;
using static Discord.Emote;

namespace Disfarm.Services.Discord.Interactions.Components.Shop
{
    [RequireLocation(Location.Neutral)]
    public class ShopBannerSwitchCurrency : InteractionModuleBase<SocketInteractionContext>
    {
        private readonly IMediator _mediator;
        private readonly ILocalizationService _local;

        public ShopBannerSwitchCurrency(
            IMediator mediator,
            ILocalizationService local)
        {
            _mediator = mediator;
            _local = local;
        }

        [ComponentInteraction("shop-banner-select-currency:*,*")]
        public async Task Execute(string currencyHashcode, string pageString)
        {
            await DeferAsync(true);

            var currency = (Currency) int.Parse(currencyHashcode);
            var page = int.Parse(pageString);

            var emotes = DiscordRepository.Emotes;
            var user = await _mediator.Send(new GetUserQuery((long) Context.User.Id));
            var banners = await _mediator.Send(new GetBannersQuery());

            banners = banners
                .Where(x =>
                    x.Rarity is BannerRarity.Common
                        or BannerRarity.Rare
                        or BannerRarity.Animated)
                .ToList();

            var maxPage = (int) Math.Ceiling(banners.Count / 5.0);
            maxPage = maxPage > 0 ? maxPage : 1; // just for better display

            banners = banners
                .Skip(page > 1 ? (page - 1) * 5 : 0)
                .Take(5)
                .ToList();

            var embed = new EmbedBuilder()
                .WithUserColor(user.CommandColor)
                .WithAuthor(Response.ShopBannerAuthor.Parse(user.Language), Context.User.GetAvatarUrl())
                .WithDescription(
                    Response.ShopBannerDesc.Parse(user.Language,
                        Context.User.Mention.AsGameMention(user.Title, user.Language), emotes.GetEmote("Arrow")) +
                    $"\n{StringExtensions.EmptyChar}")
                .AddField(Response.ShopBannerCurrentCurrencyTitle.Parse(user.Language,
                        emotes.GetEmote("Arrow"), emotes.GetEmote(currency.ToString()),
                        _local.Localize(LocalizationCategory.Currency, currency.ToString(), user.Language)),
                    StringExtensions.EmptyChar)
                .WithImageUrl(await _mediator.Send(new GetImageUrlQuery(Data.Enums.Image.ShopBanner, user.Language)))
                .WithFooter(Response.PaginatorFooter.Parse(user.Language, page, maxPage));

            var components = new ComponentBuilder
            {
                ActionRows = new List<ActionRowBuilder>
                {
                    new ActionRowBuilder()
                        .AddComponent(new ButtonBuilder(
                                Response.ComponentPaginatorBack.Parse(user.Language),
                                $"shop-banner-paginator:{currencyHashcode},{page - 1}",
                                isDisabled: page <= 1)
                            .Build())
                        .AddComponent(new ButtonBuilder(
                                Response.ComponentPaginatorForward.Parse(user.Language),
                                $"shop-banner-paginator:{currencyHashcode},{page + 1}",
                                isDisabled: page >= maxPage)
                            .Build()),

                    new ActionRowBuilder()
                        .AddComponent(new ButtonBuilder(
                                Response.ComponentShopBannerSelectCurrencyToken.Parse(user.Language),
                                $"shop-banner-select-currency:{Currency.Token.GetHashCode()},{pageString}",
                                ButtonStyle.Primary,
                                emote: Parse(emotes.GetEmote(Currency.Token.ToString())),
                                isDisabled: currency is Currency.Token)
                            .Build())
                        .AddComponent(new ButtonBuilder(
                                Response.ComponentShopBannerSelectCurrencyChip.Parse(user.Language),
                                $"shop-banner-select-currency:{Currency.Chip.GetHashCode()},{pageString}",
                                ButtonStyle.Primary,
                                emote: Parse(emotes.GetEmote(Currency.Chip.ToString())),
                                isDisabled: currency is Currency.Chip)
                            .Build())
                }
            };

            var selectMenu = new SelectMenuBuilder()
                .WithCustomId($"shop-banner-buy-currency:{currencyHashcode}")
                .WithPlaceholder(Response.ComponentShopBannerSelectBanner.Parse(user.Language));

            foreach (var banner in banners)
            {
                embed.AddField(
                    $"{emotes.GetEmote(banner.Rarity.EmoteName())} {banner.Rarity.Localize(user.Language)} " +
                    $"«{_local.Localize(LocalizationCategory.Banner, banner.Name, user.Language)}»",
                    Response.ShopBannerBannerDesc.Parse(user.Language, banner.Url,
                        emotes.GetEmote(Currency.Token.ToString()), banner.Price,
                        _local.Localize(LocalizationCategory.Currency, Currency.Token.ToString(), user.Language,
                            banner.Price),
                        emotes.GetEmote(Currency.Chip.ToString()), banner.Price.ConvertTokensToChips(),
                        _local.Localize(LocalizationCategory.Currency, Currency.Chip.ToString(), user.Language,
                            banner.Price.ConvertTokensToChips())));

                selectMenu.AddOption(banner.Name.ToLower(), $"{banner.Id}",
                    emote: Parse(emotes.GetEmote(banner.Rarity.EmoteName())));
            }

            await ModifyOriginalResponseAsync(x =>
            {
                x.Embed = embed.Build();
                x.Components = components.WithSelectMenu(selectMenu).Build();
            });
        }
    }
}