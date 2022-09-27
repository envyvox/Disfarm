using System.Threading.Tasks;
using Discord;
using Discord.Interactions;
using Disfarm.Data.Enums;
using Disfarm.Services.Discord.Embed;
using Disfarm.Services.Discord.Emote.Extensions;
using Disfarm.Services.Extensions;
using Disfarm.Services.Game.User.Queries;
using MediatR;

namespace Disfarm.Services.Discord.Interactions.Components
{
    public class CubeDropHowWorks : InteractionModuleBase<SocketInteractionContext>
    {
        private readonly IMediator _mediator;

        public CubeDropHowWorks(IMediator mediator)
        {
            _mediator = mediator;
        }

        [ComponentInteraction("how-cube-drop-works")]
        public async Task Execute()
        {
            await DeferAsync(true);

            var emotes = DiscordRepository.Emotes;
            var user = await _mediator.Send(new GetUserQuery((long) Context.User.Id));

            var embed = new EmbedBuilder()
                .WithUserColor(user.CommandColor)
                .WithAuthor(Response.HowCubeDropWorksAuthor.Parse(user.Language), Context.User.GetAvatarUrl())
                .WithDescription(Response.HowCubeDropWorksDesc.Parse(user.Language,
                    Context.User.Mention.AsGameMention(user.Title, user.Language), emotes.GetEmote("Arrow"),
                    emotes.GetEmote("CubeD61"), emotes.GetEmote("CubeD66")));

            await _mediator.Send(new FollowUpEmbedCommand(Context.Interaction, embed, Ephemeral: true));
        }
    }
}