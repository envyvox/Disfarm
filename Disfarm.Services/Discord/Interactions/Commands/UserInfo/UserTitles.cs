using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Interactions;
using Disfarm.Data.Enums;
using Disfarm.Services.Discord.Embed;
using Disfarm.Services.Discord.Emote.Extensions;
using Disfarm.Services.Discord.Extensions;
using Disfarm.Services.Discord.Image.Queries;
using Disfarm.Services.Extensions;
using Disfarm.Services.Game.Title.Queries;
using Disfarm.Services.Game.User.Queries;
using MediatR;
using static Discord.Emote;

namespace Disfarm.Services.Discord.Interactions.Commands.UserInfo
{
    [RequireContext(ContextType.Guild)]
    public class UserTitles : InteractionModuleBase<SocketInteractionContext>
    {
        private readonly IMediator _mediator;

        public UserTitles(IMediator mediator)
        {
            _mediator = mediator;
        }

        [SlashCommand("titles", "View and manage your titles")]
        public async Task Execute()
        {
            await DeferAsync(true);

            var emotes = DiscordRepository.Emotes;
            var user = await _mediator.Send(new GetUserQuery((long) Context.User.Id));
            var titles = await _mediator.Send(new GetUserTitlesQuery(user.Id));

            titles = titles
                .Where(x => x.Type != user.Title)
                .ToList();

            var maxPage = (int) Math.Ceiling(titles.Count / 10.0);
            maxPage = maxPage > 0 ? maxPage : 1; // just for better display

            var components = new ComponentBuilder()
                .WithButton(
                    Response.ComponentPaginatorBack.Parse(user.Language),
                    "user-titles-paginator:1",
                    disabled: true)
                .WithButton(
                    Response.ComponentPaginatorForward.Parse(user.Language),
                    "user-titles-paginator:2",
                    disabled: titles.Count <= 10);

            titles = titles
                .Take(10)
                .ToList();

            var embed = new EmbedBuilder()
                .WithUserColor(user.CommandColor)
                .WithAuthor(Response.UserTitlesAuthor.Parse(user.Language), Context.User.GetAvatarUrl())
                .WithDescription(
                    Response.UserTitlesDesc.Parse(user.Language,
                        Context.User.Mention.AsGameMention(user.Title, user.Language), emotes.GetEmote("Arrow")) +
                    $"\n{StringExtensions.EmptyChar}")
                .AddField(Response.UserTitleCurrentTitle.Parse(user.Language,
                        emotes.GetEmote("Arrow"), emotes.GetEmote(user.Title.EmoteName()),
                        user.Title.Localize(user.Language)),
                    StringExtensions.EmptyChar)
                .WithImageUrl(await _mediator.Send(new GetImageUrlQuery(Data.Enums.Image.UserTitles, user.Language)))
                .WithFooter(Response.PaginatorFooter.Parse(user.Language, 1, maxPage));

            var selectMenu = new SelectMenuBuilder()
                .WithPlaceholder(Response.ComponentUserTitleUpdate.Parse(user.Language))
                .WithCustomId("user-title-update");

            var counter = 0;
            foreach (var title in titles)
            {
                counter++;

                embed.AddField(
                    $"{emotes.GetEmote(title.Type.EmoteName())} {title.Type.Localize(user.Language)}",
                    Response.UserTitlesTitleDesc.Parse(user.Language,
                        title.CreatedAt.ToDiscordTimestamp(TimestampFormat.LongDate)),
                    true);

                selectMenu.AddOption(
                    title.Type.Localize(user.Language),
                    $"{title.Type.GetHashCode()}",
                    emote: Parse(emotes.GetEmote(title.Type.EmoteName())));

                if (counter == 2)
                {
                    counter = 0;
                    embed.AddEmptyField(true);
                }
            }

            if (selectMenu.Options.Any())
            {
                components.WithSelectMenu(selectMenu);
            }

            await _mediator.Send(new FollowUpEmbedCommand(Context.Interaction, embed, components.Build()));
        }
    }
}