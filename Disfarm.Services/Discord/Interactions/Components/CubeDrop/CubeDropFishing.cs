using System;
using System.Threading.Tasks;
using Discord;
using Discord.Interactions;
using Disfarm.Data.Enums;
using Disfarm.Services.Discord.Emote.Extensions;
using Disfarm.Services.Discord.Extensions;
using Disfarm.Services.Discord.Image.Queries;
using Disfarm.Services.Discord.Interactions.Attributes;
using Disfarm.Services.Extensions;
using Disfarm.Services.Game.Calculation;
using Disfarm.Services.Game.Transit.Commands;
using Disfarm.Services.Game.User.Commands;
using Disfarm.Services.Game.User.Queries;
using Disfarm.Services.Game.World.Queries;
using Disfarm.Services.Hangfire.BackgroundJobs.CompleteFishing;
using Hangfire;
using MediatR;
using static Discord.Emote;

namespace Disfarm.Services.Discord.Interactions.Components.CubeDrop
{
    [RequireLocation(Location.Neutral)]
    public class CubeDropFishing : InteractionModuleBase<SocketInteractionContext>
    {
        private readonly IMediator _mediator;
        private readonly TimeZoneInfo _timeZoneInfo;

        public CubeDropFishing(
            IMediator mediator,
            TimeZoneInfo timeZoneInfo)
        {
            _mediator = mediator;
            _timeZoneInfo = timeZoneInfo;
        }

        [RequireComponentOwner]
        [ComponentInteraction("cube-drop-fishing:*")]
        public async Task Execute(ulong _)
        {
            await DeferAsync();

            var user = await _mediator.Send(new GetUserQuery((long) Context.User.Id));
            var timeNow = TimeZoneInfo.ConvertTime(DateTimeOffset.UtcNow, _timeZoneInfo);
            var emotes = DiscordRepository.Emotes;

            var drop1 = user.CubeType.DropCube();
            var drop2 = user.CubeType.DropCube();
            var drop3 = user.CubeType.DropCube();
            var drop1CubeEmote = emotes.GetEmote(user.CubeType.EmoteName(drop1));
            var drop2CubeEmote = emotes.GetEmote(user.CubeType.EmoteName(drop2));
            var drop3CubeEmote = emotes.GetEmote(user.CubeType.EmoteName(drop3));
            var cubeDrop = drop1 + drop2 + drop3;

            var fishingTime = await _mediator.Send(new GetWorldPropertyValueQuery(
                WorldProperty.FishingDefaultDurationInMinutes));
            var duration = await _mediator.Send(new GetActionTimeQuery(
                TimeSpan.FromMinutes(fishingTime), cubeDrop));

            await _mediator.Send(new UpdateUserCommand(user with {Location = Location.Fishing}));
            await _mediator.Send(new CreateUserMovementCommand(user.Id, Location.Fishing, Location.Neutral, duration));

            BackgroundJob.Schedule<ICompleteFishingJob>(
                x => x.Execute((long) Context.Guild.Id, user.Id, cubeDrop),
                duration);

            var embed = new EmbedBuilder()
                .WithUserColor(user.CommandColor)
                .WithAuthor(Location.Fishing.Localize(user.Language), Context.User.GetAvatarUrl())
                .WithDescription(
                    Response.FishingDesc.Parse(user.Language,
                        Context.User.Mention.AsGameMention(user.Title, user.Language),
                        Location.Fishing.Localize(user.Language)) +
                    Response.CubeDrops.Parse(user.Language,
                        drop1CubeEmote, drop2CubeEmote, drop3CubeEmote, cubeDrop) +
                    $"\n{StringExtensions.EmptyChar}")
                .AddField(Response.FishingExpectedRewardTitle.Parse(user.Language),
                    Response.FishingExpectedRewardDesc.Parse(user.Language,
                        emotes.GetEmote("Xp"), emotes.GetEmote("OctopusBW")))
                .AddField(Response.FishingWillEndTitle.Parse(user.Language),
                    timeNow.Add(duration).ToDiscordTimestamp(TimestampFormat.RelativeTime))
                .WithImageUrl(await _mediator.Send(new GetImageUrlQuery(Data.Enums.Image.Fishing, user.Language)));

            var components = new ComponentBuilder()
                .WithButton(
                    Response.ComponentCubeDropHowWorks.Parse(user.Language),
                    "cube-drop-how-works",
                    ButtonStyle.Secondary,
                    Parse(emotes.GetEmote("DiscordHelp")));

            await Context.Interaction.ModifyOriginalResponseAsync(x =>
            {
                x.Embed = embed.Build();
                x.Components = components.Build();
            });
        }
    }
}