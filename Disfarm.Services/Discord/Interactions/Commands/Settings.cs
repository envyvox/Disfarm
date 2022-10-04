using System.Threading.Tasks;
using Discord;
using Discord.Interactions;
using Disfarm.Data.Enums;
using Disfarm.Services.Discord.Embed;
using Disfarm.Services.Discord.Extensions;
using Disfarm.Services.Extensions;
using Disfarm.Services.Game.User.Commands;
using Disfarm.Services.Game.User.Queries;
using MediatR;
using static Disfarm.Services.Extensions.ExceptionExtensions;

namespace Disfarm.Services.Discord.Interactions.Commands
{
    [RequireContext(ContextType.Guild)]
    [Group("settings", "Settings")]
    public class Settings : InteractionModuleBase<SocketInteractionContext>
    {
        private readonly IMediator _mediator;

        public Settings(IMediator mediator)
        {
            _mediator = mediator;
        }

        [SlashCommand("language", "Updates the bot's response language")]
        public async Task Execute(Language language)
        {
            await DeferAsync(true);

            var user = await _mediator.Send(new GetUserQuery((long) Context.User.Id));

            if (user.Language == language)
            {
                throw new GameUserExpectedException(
                    Response.SettingsLanguageAlready.Parse(user.Language));
            }

            await _mediator.Send(new UpdateUserCommand(user with {Language = language}));

            var embed = new EmbedBuilder()
                .WithUserColor(user.CommandColor)
                .WithAuthor(Response.SettingsLanguageTitle.Parse(language), Context.User.GetAvatarUrl())
                .WithDescription(Response.SettingsLanguageDesc.Parse(language,
                    Context.User.Mention.AsGameMention(user.Title, language)));

            await Context.Interaction.FollowUpResponse(embed);
        }
    }
}