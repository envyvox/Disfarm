using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Interactions;
using Disfarm.Data.Enums;
using Disfarm.Services.Discord.Emote.Extensions;
using Disfarm.Services.Discord.Image.Queries;
using Disfarm.Services.Extensions;
using Disfarm.Services.Game.Banner.Commands;
using Disfarm.Services.Game.Banner.Queries;
using Disfarm.Services.Game.User.Queries;
using MediatR;

namespace Disfarm.Services.Discord.Interactions.Components.UserBanners
{
    public class UserBannerUpdate : InteractionModuleBase<SocketInteractionContext>
    {
        private readonly IMediator _mediator;

        public UserBannerUpdate(IMediator mediator)
        {
            _mediator = mediator;
        }

        [ComponentInteraction("user-banner-update")]
        public async Task UserBannerUpdateTask(string[] selectedValues)
        {
            await DeferAsync();

            var bannerId = Guid.Parse(selectedValues.First());

            var emotes = DiscordRepository.Emotes;
            var user = await _mediator.Send(new GetUserQuery((long) Context.User.Id));
            var banner = await _mediator.Send(new GetBannerQuery(bannerId));
            var activeBanner = await _mediator.Send(new GetUserActiveBannerQuery(user.Id));

            await _mediator.Send(new DeactivateUserBannerCommand(user.Id, activeBanner.Id));
            await _mediator.Send(new ActivateUserBannerCommand(user.Id, banner.Id));

            var embed = new EmbedBuilder()
                .WithUserColor(user.CommandColor)
                .WithAuthor(Response.UserBannersUpdateAuthor.Parse(user.Language), Context.User.GetAvatarUrl())
                .WithDescription(
                    Response.UserBannersUpdateDesc.Parse(user.Language,
                        Context.User.Mention.AsGameMention(user.Title, user.Language),
                        emotes.GetEmote(banner.Rarity.EmoteName()),
                        banner.Rarity.Localize(user.Language).ToLower(), banner.Name))
                .WithImageUrl(await _mediator.Send(new GetImageUrlQuery(Data.Enums.Image.UserBanners, user.Language)));

            await ModifyOriginalResponseAsync(x =>
            {
                x.Embed = embed.Build();
                x.Components = new ComponentBuilder().Build();
            });
        }
    }
}