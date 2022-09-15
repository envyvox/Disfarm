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
using Disfarm.Services.Game.Title.Queries;
using Disfarm.Services.Game.User.Queries;
using MediatR;
using static Discord.Emote;

namespace Disfarm.Services.Discord.Interactions.Components.UserTitles
{
    public class UserTitlesPaginator : InteractionModuleBase<SocketInteractionContext>
    {
        private readonly IMediator _mediator;

        public UserTitlesPaginator(IMediator mediator)
        {
            _mediator = mediator;
        }

        [ComponentInteraction("user-titles-paginator:*")]
        public async Task Execute(string pageString)
        {
            await DeferAsync(true);

            var page = int.Parse(pageString);

            var emotes = DiscordRepository.Emotes;
            var user = await _mediator.Send(new GetUserQuery((long) Context.User.Id));
            var titles = await _mediator.Send(new GetUserTitlesQuery(user.Id));

            titles = titles
                .Where(x => x.Type != user.Title)
                .ToList();

            var maxPage = (int) Math.Ceiling(titles.Count / 10.0);
            maxPage = maxPage > 0 ? maxPage : 1; // just for better display

            titles = titles
                .Skip(page > 1 ? (page - 1) * 10 : 0)
                .Take(10)
                .ToList();

            var components = new ComponentBuilder()
                .WithButton(
                    Response.ComponentPaginatorBack.Parse(user.Language),
                    $"user-titles-paginator:{page - 1}",
                    disabled: page <= 1)
                .WithButton(
                    Response.ComponentPaginatorForward.Parse(user.Language),
                    $"user-titles-paginator:{page + 1}",
                    disabled: page >= maxPage);

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
                .WithFooter(Response.PaginatorFooter.Parse(user.Language, page, maxPage));

            var selectMenu = new SelectMenuBuilder()
                .WithPlaceholder(Response.ComponentUserTitleUpdate.Parse(user.Language))
                .WithCustomId("user-titles-update");

            var counter = 0;
            foreach (var title in titles)
            {
                counter++;

                embed.AddField(
                    $"{emotes.GetEmote(title.Type.EmoteName())} {title.Type.Localize(user.Language)}",
                    Response.UserTitlesTitleDesc.Parse(user.Language,
                        title.CreatedAt.ConvertToDiscordTimestamp(TimestampFormat.LongDate)),
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

            await ModifyOriginalResponseAsync(x =>
            {
                x.Embed = embed.Build();
                x.Components = components.Build();
            });
        }
    }
}