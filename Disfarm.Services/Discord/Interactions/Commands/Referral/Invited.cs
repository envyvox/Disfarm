using System.Threading.Tasks;
using Discord;
using Discord.Interactions;
using Disfarm.Data.Enums;
using Disfarm.Services.Discord.Client.Queries;
using Disfarm.Services.Discord.Emote.Extensions;
using Disfarm.Services.Discord.Extensions;
using Disfarm.Services.Discord.Image.Queries;
using Disfarm.Services.Extensions;
using Disfarm.Services.Game.Container.Commands;
using Disfarm.Services.Game.Localization;
using Disfarm.Services.Game.Referral.Commands;
using Disfarm.Services.Game.Referral.Queries;
using Disfarm.Services.Game.User.Queries;
using MediatR;
using static Disfarm.Services.Extensions.ExceptionExtensions;

namespace Disfarm.Services.Discord.Interactions.Commands.Referral
{
    [RequireContext(ContextType.Guild)]
    public class Invited : InteractionModuleBase<SocketInteractionContext>
    {
        private readonly IMediator _mediator;
        private readonly ILocalizationService _local;

        public Invited(
            IMediator mediator,
            ILocalizationService local)
        {
            _mediator = mediator;
            _local = local;
        }

        [SlashCommand("invited", "Specify the user who invited you and get rewards")]
        public async Task Execute(
            [Summary("user", "User who invited you (@mention or ID)")]
            IUser mentionedUser)
        {
            await DeferAsync();

            var emotes = DiscordRepository.Emotes;
            var user = await _mediator.Send(new GetUserQuery((long) Context.User.Id));
            var tUser = await _mediator.Send(new GetUserQuery((long) mentionedUser.Id));
            var tSocketUser = await _mediator.Send(new GetClientUserQuery(mentionedUser.Id));

            if (user.Id == tUser.Id)
            {
                throw new GameUserExpectedException(Response.InvitedYourself.Parse(user.Language));
            }

            if (tSocketUser.IsBot)
            {
                throw new GameUserExpectedException(Response.InvitedIsBot.Parse(user.Language));
            }

            var hasReferrer = await _mediator.Send(new CheckUserHasReferrerQuery(user.Id));

            if (hasReferrer)
            {
                var rUser = await _mediator.Send(new GetUserReferrerQuery(user.Id));
                var rSocketUser = await _mediator.Send(new GetClientUserQuery((ulong) rUser.Id));

                throw new GameUserExpectedException(Response.InvitedHasReferrer.Parse(user.Language,
                    rSocketUser.Mention.AsGameMention(rUser.Title, user.Language)));
            }

            await _mediator.Send(new AddContainerToUserCommand(user.Id, Container.Token, 1));
            await _mediator.Send(new CreateUserReferrerCommand(
                Context.Guild.Id, Context.Channel.Id, user.Id, tUser.Id));

            var embed = new EmbedBuilder()
                .WithUserColor(user.CommandColor)
                .WithAuthor(Response.ReferralSystemAuthor.Parse(user.Language), Context.User.GetAvatarUrl())
                .WithDescription(Response.InvitedDesc.Parse(user.Language,
                    Context.User.Mention.AsGameMention(user.Title, user.Language),
                    tSocketUser.Mention.AsGameMention(tUser.Title, user.Language),
                    emotes.GetEmote(Container.Token.EmoteName()),
                    _local.Localize(LocalizationCategory.Container, Container.Token.ToString(), user.Language)))
                .WithImageUrl(await _mediator.Send(new GetImageUrlQuery(Data.Enums.Image.Referral, user.Language)));

            await Context.Interaction.FollowUpResponse(embed);
        }
    }
}