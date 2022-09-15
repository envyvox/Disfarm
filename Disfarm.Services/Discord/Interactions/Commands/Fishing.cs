using System.Threading.Tasks;
using Discord;
using Discord.Interactions;
using Disfarm.Data.Enums;
using Disfarm.Services.Discord.Embed;
using Disfarm.Services.Discord.Emote.Extensions;
using Disfarm.Services.Discord.Image.Queries;
using Disfarm.Services.Discord.Interactions.Attributes;
using Disfarm.Services.Extensions;
using Disfarm.Services.Game.User.Queries;
using MediatR;

namespace Disfarm.Services.Discord.Interactions.Commands
{
    [RequireLocation(Location.Neutral)]
    public class Fishing : InteractionModuleBase<SocketInteractionContext>
    {
        private readonly IMediator _mediator;

        public Fishing(IMediator mediator)
        {
            _mediator = mediator;
        }

        [SlashCommand("fishing", "Go fishing in the neutral zone")]
        public async Task Execute()
        {
            await DeferAsync();

            var emotes = DiscordRepository.Emotes;
            var user = await _mediator.Send(new GetUserQuery((long) Context.User.Id));

            var embed = new EmbedBuilder()
                .WithUserColor(user.CommandColor)
                .WithAuthor(Location.Fishing.Localize(user.Language), Context.User.GetAvatarUrl())
                .WithDescription(
                    Response.FishingDesc.Parse(user.Language,
                        Context.User.Mention.AsGameMention(user.Title, user.Language),
                        Location.Fishing.Localize(user.Language)) +
                    Response.CubeDropPressButton.Parse(user.Language) +
                    $"\n{StringExtensions.EmptyChar}")
                .AddField(Response.FishingExpectedRewardTitle.Parse(user.Language),
                    Response.FishingExpectedRewardDesc.Parse(user.Language,
                        emotes.GetEmote("Xp"), emotes.GetEmote("OctopusBW")))
                .AddField(Response.FishingWillEndTitle.Parse(user.Language),
                    Response.CubeDropWaiting.Parse(user.Language))
                .WithImageUrl(await _mediator.Send(new GetImageUrlQuery(Data.Enums.Image.Fishing, user.Language)));

            var components = new ComponentBuilder()
                .WithButton(Response.ComponentCubeDrop.Parse(user.Language), $"cube-drop-fishing:{user.Id}")
                .Build();

            await _mediator.Send(new FollowUpEmbedCommand(Context.Interaction, embed, components));
        }
    }
}