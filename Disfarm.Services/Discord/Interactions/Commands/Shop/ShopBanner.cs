using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Interactions;
using Disfarm.Data.Enums;
using Disfarm.Services.Discord.Embed;
using Disfarm.Services.Discord.Emote.Extensions;
using Disfarm.Services.Discord.Image.Queries;
using Disfarm.Services.Discord.Interactions.Attributes;
using Disfarm.Services.Extensions;
using Disfarm.Services.Game.Banner.Queries;
using Disfarm.Services.Game.Localization;
using Disfarm.Services.Game.User.Queries;
using MediatR;
using static Discord.Emote;

namespace Disfarm.Services.Discord.Interactions.Commands.Shop
{
    [RequireContext(ContextType.Guild)]
    [RequireLocation(Location.Neutral)]
    public class ShopBanner : InteractionModuleBase<SocketInteractionContext>
    {
        private readonly IMediator _mediator;
        private readonly ILocalizationService _local;

        public ShopBanner(
            IMediator mediator,
            ILocalizationService local)
        {
            _mediator = mediator;
            _local = local;
        }

        [SlashCommand("shop-banners", "Purchase different banners for your profile")]
        public async Task Execute()
        {
            await DeferAsync(true);

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
                        emotes.GetEmote("Arrow"), emotes.GetEmote(Currency.Token.ToString()),
                        _local.Localize(LocalizationCategory.Currency, Currency.Token.ToString(), user.Language)),
                    StringExtensions.EmptyChar)
                .WithImageUrl(await _mediator.Send(new GetImageUrlQuery(Data.Enums.Image.ShopBanner, user.Language)))
                .WithFooter(Response.PaginatorFooter.Parse(user.Language, 1, maxPage));

            var components = new ComponentBuilder
            {
                ActionRows = new List<ActionRowBuilder>
                {
                    new ActionRowBuilder()
                        .AddComponent(new ButtonBuilder(
                                Response.ComponentPaginatorBack.Parse(user.Language),
                                $"shop-banner-paginator:{Currency.Token.GetHashCode()},1",
                                isDisabled: true)
                            .Build())
                        .AddComponent(new ButtonBuilder(
                                Response.ComponentPaginatorForward.Parse(user.Language),
                                $"shop-banner-paginator:{Currency.Token.GetHashCode()},2")
                            .Build()),

                    new ActionRowBuilder()
                        .AddComponent(new ButtonBuilder(
                                Response.ComponentShopBannerSelectCurrencyToken.Parse(user.Language),
                                $"shop-banner-select-currency:{Currency.Token.GetHashCode()},1",
                                ButtonStyle.Primary,
                                emote: Parse(emotes.GetEmote(Currency.Token.ToString())),
                                isDisabled: true)
                            .Build())
                        .AddComponent(new ButtonBuilder(
                                Response.ComponentShopBannerSelectCurrencyChip.Parse(user.Language),
                                $"shop-banner-select-currency:{Currency.Chip.GetHashCode()},1",
                                ButtonStyle.Primary,
                                emote: Parse(emotes.GetEmote(Currency.Chip.ToString())))
                            .Build())
                }
            };

            var selectMenu = new SelectMenuBuilder()
                .WithCustomId($"shop-banner-buy-currency:{Currency.Token.GetHashCode()}")
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

            await _mediator.Send(new FollowUpEmbedCommand(Context.Interaction, embed,
                components
                    .WithSelectMenu(selectMenu)
                    .Build()));
        }
    }
}