using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Interactions;
using Disfarm.Data.Enums;
using Disfarm.Services.Discord.Emote.Extensions;
using Disfarm.Services.Discord.Extensions;
using Disfarm.Services.Discord.Image.Queries;
using Disfarm.Services.Extensions;
using Disfarm.Services.Game.Banner.Queries;
using Disfarm.Services.Game.Localization;
using Disfarm.Services.Game.User.Queries;
using MediatR;
using static Discord.Emote;

namespace Disfarm.Services.Discord.Interactions.Components.UserBanners
{
    public class UserBannersPaginator : InteractionModuleBase<SocketInteractionContext>
    {
        private readonly IMediator _mediator;
        private readonly ILocalizationService _local;

        public UserBannersPaginator(
            IMediator mediator,
            ILocalizationService local)
        {
            _mediator = mediator;
            _local = local;
        }

        [ComponentInteraction("user-banners-paginator:*")]
        public async Task USerBannersPaginatorTask(string pageString)
        {
            await DeferAsync(true);

            var page = int.Parse(pageString);

            var emotes = DiscordRepository.Emotes;
            var user = await _mediator.Send(new GetUserQuery((long) Context.User.Id));
            var activeBanner = await _mediator.Send(new GetUserActiveBannerQuery(user.Id));
            var userBanners = await _mediator.Send(new GetUserBannersQuery(user.Id));

            userBanners = userBanners
                .Where(x => x.IsActive is false)
                .OrderBy(x => x.Expiration)
                .ToList();

            var maxPage = (int) Math.Ceiling(userBanners.Count / 5.0);
            maxPage = maxPage > 0 ? maxPage : 1; // just for better display

            userBanners = userBanners
                .Skip(page > 1 ? (page - 1) * 5 : 0)
                .Take(5)
                .ToList();

            var components = new ComponentBuilder()
                .WithButton(
                    Response.ComponentPaginatorBack.Parse(user.Language),
                    $"user-banners-paginator:{page - 1}",
                    disabled: page <= 1)
                .WithButton(
                    Response.ComponentPaginatorForward.Parse(user.Language),
                    $"user-banners-paginator:{page + 1}",
                    disabled: page >= maxPage);

            var embed = new EmbedBuilder()
                .WithUserColor(user.CommandColor)
                .WithAuthor(Response.UserBannersAuthor.Parse(user.Language), Context.User.GetAvatarUrl())
                .WithDescription(
                    Response.UserBannersDesc.Parse(user.Language,
                        Context.User.Mention.AsGameMention(user.Title, user.Language),
                        emotes.GetEmote("Arrow")) +
                    $"\n{StringExtensions.EmptyChar}")
                .AddField(Response.UserBannersCurrentBanner.Parse(user.Language,
                        emotes.GetEmote("Arrow"), emotes.GetEmote(activeBanner.Rarity.EmoteName()),
                        activeBanner.Rarity.Localize(user.Language), activeBanner.Name),
                    StringExtensions.EmptyChar)
                .WithImageUrl(await _mediator.Send(new GetImageUrlQuery(Data.Enums.Image.UserBanners, user.Language)))
                .WithFooter(Response.PaginatorFooter.Parse(user.Language, page, maxPage));

            var selectMenu = new SelectMenuBuilder()
                .WithPlaceholder(Response.ComponentUserBannerUpdate.Parse(user.Language))
                .WithCustomId("user-banner-update");

            foreach (var userBanner in userBanners)
            {
                embed.AddField(
                    $"{emotes.GetEmote(userBanner.Banner.Rarity.EmoteName())} {userBanner.Banner.Rarity.Localize(user.Language)} " +
                    $"«{_local.Localize(LocalizationCategory.Banner, userBanner.Banner.Name, user.Language)}»",
                    Response.UserBannersBannerDesc.Parse(user.Language, userBanner.Banner.Url,
                        userBanner.Expiration.ConvertToDiscordTimestamp(TimestampFormat.RelativeTime)));

                selectMenu.AddOption(
                    userBanner.Banner.Name,
                    $"{userBanner.Banner.Id}",
                    emote: Parse(emotes.GetEmote(userBanner.Banner.Rarity.EmoteName())));
            }

            if (selectMenu.Options.Any())
            {
                components.WithSelectMenu(selectMenu);
            }

            await ModifyOriginalResponseAsync(x =>
            {
                x.Embed = embed.Build();
                x.Components = components.Build();
            });
        }
    }
}