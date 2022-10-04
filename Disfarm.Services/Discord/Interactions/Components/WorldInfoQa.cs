using System.Threading.Tasks;
using Discord;
using Discord.Interactions;
using Disfarm.Data.Enums;
using Disfarm.Services.Discord.Embed;
using Disfarm.Services.Discord.Emote.Extensions;
using Disfarm.Services.Discord.Extensions;
using Disfarm.Services.Discord.Image.Queries;
using Disfarm.Services.Extensions;
using Disfarm.Services.Game.User.Queries;
using MediatR;

namespace Disfarm.Services.Discord.Interactions.Components
{
    public class WorldInfoQa : InteractionModuleBase<SocketInteractionContext>
    {
        private readonly IMediator _mediator;

        public WorldInfoQa(IMediator mediator)
        {
            _mediator = mediator;
        }

        [ComponentInteraction("world-info-qa:*")]
        public async Task Execute(string selectedValue)
        {
            await DeferAsync();

            var emotes = DiscordRepository.Emotes;
            var user = await _mediator.Send(new GetUserQuery((long) Context.User.Id));

            var embed = new EmbedBuilder()
                .WithUserColor(user.CommandColor)
                .WithImageUrl(await _mediator.Send(new GetImageUrlQuery(Data.Enums.Image.WorldInfo, user.Language)));

            switch (selectedValue)
            {
                case "timesDay":
                {
                    embed
                        .WithAuthor(Response.WorldInfoQaTimesDayAuthor.Parse(user.Language),
                            Context.User.GetAvatarUrl())
                        .WithDescription(Response.WorldInfoQaTimesDayDesc.Parse(user.Language,
                            Context.User.Mention.AsGameMention(user.Title, user.Language),
                            emotes.GetEmote("Night"), emotes.GetEmote("Day")));

                    break;
                }
                case "weather":
                {
                    embed
                        .WithAuthor(Response.WorldInfoQaWeatherAuthor.Parse(user.Language),
                            Context.User.GetAvatarUrl())
                        .WithDescription(Response.WorldInfoQaWeatherDesc.Parse(user.Language,
                            Context.User.Mention.AsGameMention(user.Title, user.Language),
                            emotes.GetEmote("WeatherRain"), emotes.GetEmote("WeatherClear")));

                    break;
                }
                case "season":
                {
                    embed
                        .WithAuthor(Response.WorldInfoQaSeasonAuthor.Parse(user.Language), Context.User.GetAvatarUrl())
                        .WithDescription(Response.WorldInfoQaSeasonDesc.Parse(user.Language,
                            Context.User.Mention.AsGameMention(user.Title, user.Language)));

                    break;
                }
            }

            await Context.Interaction.FollowUpResponse(embed, ephemeral: true);
        }
    }
}