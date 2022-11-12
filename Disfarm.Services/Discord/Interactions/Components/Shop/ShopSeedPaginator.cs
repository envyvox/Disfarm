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
using Disfarm.Services.Game.Localization;
using Disfarm.Services.Game.Seed.Queries;
using Disfarm.Services.Game.User.Queries;
using Disfarm.Services.Game.World.Queries;
using MediatR;
using static Discord.Emote;

namespace Disfarm.Services.Discord.Interactions.Components.Shop
{
    [RequireLocation(Location.Neutral)]
    public class ShopSeedPaginator : InteractionModuleBase<SocketInteractionContext>
    {
        private readonly IMediator _mediator;
        private readonly ILocalizationService _local;
        private readonly TimeZoneInfo _timeZoneInfo;

        public ShopSeedPaginator(
            IMediator mediator,
            ILocalizationService local,
            TimeZoneInfo timeZoneInfo)
        {
            _mediator = mediator;
            _local = local;
            _timeZoneInfo = timeZoneInfo;
        }

        [ComponentInteraction("shop-seed-paginator:*")]
        public async Task Execute(string pageString)
        {
            await DeferAsync(true);

            var timeNow = TimeZoneInfo.ConvertTime(DateTimeOffset.UtcNow, _timeZoneInfo);
            var page = int.Parse(pageString);
            var emotes = DiscordRepository.Emotes;
            var user = await _mediator.Send(new GetUserQuery((long)Context.User.Id));
            var state = await _mediator.Send(new GetWorldStateQuery());
            var seeds = await _mediator.Send(new GetSeedsBySeasonQuery(state.CurrentSeason));

            var maxPage = (int)Math.Ceiling(seeds.Count / 5.0);
            maxPage = maxPage > 0 ? maxPage : 1; // just for better display

            var components = new ComponentBuilder()
                .WithButton(
                    Response.ComponentPaginatorBack.Parse(user.Language),
                    $"shop-seed-paginator:{page - 1}",
                    disabled: page <= 1)
                .WithButton(
                    Response.ComponentPaginatorForward.Parse(user.Language),
                    $"shop-seed-paginator:{page + 1}",
                    disabled: page >= maxPage);

            seeds = seeds
                .Skip(page > 1 ? (page - 1) * 5 : 0)
                .Take(5)
                .ToList();

            var embed = new EmbedBuilder()
                .WithUserColor(user.CommandColor)
                .WithAuthor(Response.ShopSeedAuthor.Parse(user.Language), Context.User.GetAvatarUrl())
                .WithDescription(
                    Response.ShopSeedDesc.Parse(user.Language,
                        Context.User.Mention.AsGameMention(user.Title, user.Language), emotes.GetEmote("Arrow")) +
                    $"\n{StringExtensions.EmptyChar}")
                .WithImageUrl(await _mediator.Send(new GetImageUrlQuery(Data.Enums.Image.ShopSeed, user.Language)))
                .WithFooter(Response.PaginatorFooter.Parse(user.Language, page, maxPage));

            var selectMenu = new SelectMenuBuilder()
                .WithPlaceholder(Response.ComponentShopSeedBuy.Parse(user.Language))
                .WithCustomId("shop-seed-buy");

            foreach (var seed in seeds)
            {
                var seedDesc = Response.ShopSeedSeedDesc.Parse(user.Language,
                    timeNow.Add(seed.Growth).ToDiscordTimestamp(TimestampFormat.RelativeTime),
                    emotes.GetEmote(seed.Crop.Name),
                    _local.Localize(LocalizationCategory.Crop, seed.Crop.Name, user.Language),
                    emotes.GetEmote(Currency.Token.ToString()), seed.Crop.Price,
                    _local.Localize(LocalizationCategory.Currency, Currency.Token.ToString(), user.Language,
                        seed.Crop.Price));

                if (seed.IsMultiply)
                    seedDesc += Response.ShopSeedSeedMultiply.Parse(user.Language,
                        emotes.GetEmote("Arrow"));

                if (seed.ReGrowth is not null)
                    seedDesc += Response.ShopSeedSeedReGrowth.Parse(user.Language,
                        emotes.GetEmote("Arrow"),
                        timeNow.Add(seed.ReGrowth.Value).ToDiscordTimestamp(TimestampFormat.RelativeTime));

                embed.AddField(Response.ShopSeedSeedPrice.Parse(user.Language,
                        emotes.GetEmote(seed.Name),
                        _local.Localize(LocalizationCategory.Seed, seed.Name, user.Language, 5),
                        emotes.GetEmote(Currency.Token.ToString()), seed.Price * 5,
                        _local.Localize(LocalizationCategory.Currency, Currency.Token.ToString(), user.Language,
                            seed.Price * 5)),
                    seedDesc + $"\n{StringExtensions.EmptyChar}");

                selectMenu.AddOption(
                    $"5 {_local.Localize(LocalizationCategory.Seed, seed.Name, user.Language, 5)}",
                    $"{seed.Id}",
                    emote: Parse(emotes.GetEmote(seed.Name)));
            }

            await ModifyOriginalResponseAsync(x =>
            {
                x.Embed = embed.Build();
                x.Components = components.WithSelectMenu(selectMenu).Build();
            });
        }
    }
}