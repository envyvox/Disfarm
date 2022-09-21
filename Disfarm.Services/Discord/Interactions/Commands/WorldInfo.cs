using System;
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
using Disfarm.Services.Game.World.Queries;
using MediatR;
using static Discord.Emote;

namespace Disfarm.Services.Discord.Interactions.Commands
{
    public class WorldInfo : InteractionModuleBase<SocketInteractionContext>
    {
        private readonly IMediator _mediator;
        private readonly TimeZoneInfo _timeZoneInfo;

        public WorldInfo(
            IMediator mediator,
            TimeZoneInfo timeZoneInfo)
        {
            _mediator = mediator;
            _timeZoneInfo = timeZoneInfo;
        }

        [SlashCommand("world-info", "View information about the current state of the world")]
        public async Task Execute()
        {
            await DeferAsync(true);

            var emotes = DiscordRepository.Emotes;
            var timeNow = TimeZoneInfo.ConvertTime(DateTimeOffset.UtcNow, _timeZoneInfo);
            var newMonth = timeNow
                .AddDays(DateTime.DaysInMonth(timeNow.Year, timeNow.Month) - timeNow.Day + 1)
                .Subtract(TimeSpan.FromHours(timeNow.Hour))
                .Subtract(TimeSpan.FromMinutes(timeNow.Minute));

            var user = await _mediator.Send(new GetUserQuery((long) Context.User.Id));
            var (_, currentSeason, weatherToday, weatherTomorrow, _, _) =
                await _mediator.Send(new GetWorldStateQuery());
            var currentTimesDay = await _mediator.Send(new GetCurrentTimesDayQuery());

            var embed = new EmbedBuilder()
                .WithUserColor(user.CommandColor)
                .WithAuthor(Response.WorldStateAuthor.Parse(user.Language), Context.User.GetAvatarUrl())
                .AddField(StringExtensions.EmptyChar, Response.WorldStateTimesDayDesc.Parse(user.Language,
                    timeNow.ToDiscordTimestamp(TimestampFormat.ShortTime),
                    emotes.GetEmote(currentTimesDay.ToString()),
                    currentTimesDay.Localize(user.Language)))
                .AddField(StringExtensions.EmptyChar,
                    Response.WorldStateWeatherTodayDesc.Parse(user.Language,
                        emotes.GetEmote(weatherToday.EmoteName()), weatherToday.Localize(user.Language)))
                .AddField(StringExtensions.EmptyChar,
                    Response.WorldStateWeatherTomorrowDesc.Parse(user.Language,
                        emotes.GetEmote(weatherTomorrow.EmoteName()), weatherTomorrow.Localize(user.Language)))
                .AddField(StringExtensions.EmptyChar,
                    Response.WorldStateSeasonDesc.Parse(user.Language,
                        emotes.GetEmote(currentSeason.EmoteName()), currentSeason.Localize(user.Language).ToLower(),
                        emotes.GetEmote(currentSeason.NextSeason().EmoteName()),
                        currentSeason.NextSeason().Localize(user.Language, true).ToLower(),
                        newMonth.ToDiscordTimestamp(TimestampFormat.RelativeTime)))
                .WithImageUrl(await _mediator.Send(new GetImageUrlQuery(Data.Enums.Image.WorldInfo, user.Language)));

            var components = new ComponentBuilder()
                .WithButton(
                    Response.ComponentTimesDayQa.Parse(user.Language),
                    "world-info-qa:timesDay",
                    ButtonStyle.Secondary,
                    Parse(emotes.GetEmote("DiscordHelp")))
                .WithButton(
                    Response.ComponentWeatherQa.Parse(user.Language),
                    "world-info-qa:weather",
                    ButtonStyle.Secondary,
                    Parse(emotes.GetEmote("DiscordHelp")))
                .WithButton(
                    Response.ComponentSeasonQa.Parse(user.Language),
                    "world-info-qa:season",
                    ButtonStyle.Secondary,
                    Parse(emotes.GetEmote("DiscordHelp")));

            await _mediator.Send(new FollowUpEmbedCommand(Context.Interaction, embed, components.Build()));
        }
    }
}