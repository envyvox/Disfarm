using System;
using System.Linq;
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
using Disfarm.Services.Game.Farm.Queries;
using Disfarm.Services.Game.Transit.Commands;
using Disfarm.Services.Game.User.Commands;
using Disfarm.Services.Game.User.Queries;
using Disfarm.Services.Game.World.Queries;
using Disfarm.Services.Hangfire.BackgroundJobs.CompleteFarmWatering;
using Hangfire;
using MediatR;
using static Discord.Emote;
using static Disfarm.Services.Extensions.ExceptionExtensions;

namespace Disfarm.Services.Discord.Interactions.Components.Farm
{
    [RequireLocation(Location.Neutral)]
    public class UserFarmWater : InteractionModuleBase<SocketInteractionContext>
    {
        private readonly IMediator _mediator;

        public UserFarmWater(IMediator mediator)
        {
            _mediator = mediator;
        }

        [ComponentInteraction("user-farm-water")]
        public async Task Execute()
        {
            await DeferAsync();

            var emotes = DiscordRepository.Emotes;
            var user = await _mediator.Send(new GetUserQuery((long) Context.User.Id));
            var userFarms = await _mediator.Send(new GetUserFarmsQuery(user.Id));
            var cellsToWater = userFarms.Count(x => x.State == FieldState.Planted);

            if (cellsToWater < 1)
            {
                throw new GameUserExpectedException(Response.UserFarmWaterNoPlatedCells.Parse(user.Language,
                    emotes.GetEmote(Building.Farm.ToString())));
            }

            var embed = new EmbedBuilder()
                .WithUserColor(user.CommandColor)
                .WithAuthor(Response.UserFarmWaterAuthor.Parse(user.Language), Context.User.GetAvatarUrl())
                .WithDescription(
                    Response.UserFarmWaterDesc.Parse(user.Language,
                        Context.User.Mention.AsGameMention(user.Title, user.Language),
                        emotes.GetEmote(Building.Farm.ToString())) +
                    Response.CubeDropPressButton.Parse(user.Language) +
                    $"\n{StringExtensions.EmptyChar}")
                .AddField(Response.WillEndTitle.Parse(user.Language),
                    Response.CubeDropWaiting.Parse(user.Language))
                .WithImageUrl(await _mediator.Send(new GetImageUrlQuery(Data.Enums.Image.Harvesting, user.Language)));

            var components = new ComponentBuilder()
                .WithButton(Response.ComponentCubeDrop.Parse(user.Language), "user-farm-water-cube-drop");

            await ModifyOriginalResponseAsync(x =>
            {
                x.Embed = embed.Build();
                x.Components = components.Build();
            });
        }

        [ComponentInteraction("user-farm-water-cube-drop")]
        public async Task ExecuteCubeDrop()
        {
            await DeferAsync();

            var emotes = DiscordRepository.Emotes;
            var user = await _mediator.Send(new GetUserQuery((long) Context.User.Id));
            var userFarms = await _mediator.Send(new GetUserFarmsQuery(user.Id));
            var farmsToWater = (uint) userFarms.Count(x => x.State == FieldState.Planted);

            var drop1 = user.CubeType.DropCube();
            var drop2 = user.CubeType.DropCube();
            var drop3 = user.CubeType.DropCube();
            var drop1CubeEmote = emotes.GetEmote(user.CubeType.EmoteName(drop1));
            var drop2CubeEmote = emotes.GetEmote(user.CubeType.EmoteName(drop2));
            var drop3CubeEmote = emotes.GetEmote(user.CubeType.EmoteName(drop3));
            var cubeDrop = drop1 + drop2 + drop3;

            var wateringTime = await _mediator.Send(new GetWorldPropertyValueQuery(
                WorldProperty.FarmWateringDefaultDurationInMinutes));
            var duration = await _mediator.Send(new GetActionTimeQuery(
                TimeSpan.FromMinutes(wateringTime * farmsToWater), cubeDrop));

            await _mediator.Send(new UpdateUserCommand(user with {Location = Location.FarmWatering}));
            await _mediator.Send(new CreateUserMovementCommand(
                user.Id, Location.FarmWatering, Location.Neutral, duration));

            BackgroundJob.Schedule<ICompleteFarmWateringJob>(
                x => x.Execute((long) Context.Guild.Id, user.Id, farmsToWater),
                duration);

            var embed = new EmbedBuilder()
                .WithUserColor(user.CommandColor)
                .WithAuthor(Response.UserFarmWaterAuthor.Parse(user.Language), Context.User.GetAvatarUrl())
                .WithDescription(
                    Response.UserFarmWaterDesc.Parse(user.Language,
                        Context.User.Mention.AsGameMention(user.Title, user.Language),
                        emotes.GetEmote(Building.Farm.ToString())) +
                    Response.CubeDrops.Parse(user.Language,
                        drop1CubeEmote, drop2CubeEmote, drop3CubeEmote, cubeDrop) +
                    $"\n{StringExtensions.EmptyChar}")
                .AddField(Response.WillEndTitle.Parse(user.Language),
                    DateTimeOffset.UtcNow.Add(duration).ToDiscordTimestamp(TimestampFormat.RelativeTime))
                .WithImageUrl(await _mediator.Send(new GetImageUrlQuery(Data.Enums.Image.Harvesting, user.Language)));

            var components = new ComponentBuilder()
                .WithButton(
                    Response.ComponentCubeDropHowWorks.Parse(user.Language),
                    "cube-drop-how-works",
                    ButtonStyle.Secondary,
                    Parse(emotes.GetEmote("DiscordHelp")));

            await ModifyOriginalResponseAsync(x =>
            {
                x.Embed = embed.Build();
                x.Components = components.Build();
            });
        }
    }
}