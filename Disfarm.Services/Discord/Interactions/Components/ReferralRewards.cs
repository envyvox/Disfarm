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

namespace Disfarm.Services.Discord.Interactions.Components
{
    public class ReferralRewards : InteractionModuleBase<SocketInteractionContext>
    {
        private readonly IMediator _mediator;
        private readonly ILocalizationService _local;

        public ReferralRewards(
            IMediator mediator,
            ILocalizationService local)
        {
            _mediator = mediator;
            _local = local;
        }

        [ComponentInteraction("referral-rewards")]
        public async Task Execute()
        {
            await DeferAsync();

            var emotes = DiscordRepository.Emotes;
            var user = await _mediator.Send(new GetUserQuery((long) Context.User.Id));
            var banners = await _mediator.Send(new GetBannersQuery());
            var banner = banners.Single(x => x.Name == "BibaAndBoba");

            var embed = new EmbedBuilder()
                .WithUserColor(user.CommandColor)
                .WithAuthor(Response.ReferralRewardsAuthor.Parse(user.Language), Context.User.GetAvatarUrl())
                .WithDescription(Response.ReferralRewardsDesc.Parse(user.Language,
                    emotes.GetEmote(banner.Rarity.EmoteName()), banner.Rarity.Localize(user.Language).ToLower(),
                    _local.Localize(LocalizationCategory.Banner, banner.Name, user.Language), banner.Url))
                .WithImageUrl(
                    await _mediator.Send(new GetImageUrlQuery(Data.Enums.Image.ReferralRewards, user.Language)));

            await Context.Interaction.FollowUpResponse(embed, ephemeral: true);
        }
    }
}