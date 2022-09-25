using System.Threading.Tasks;
using Discord;
using Disfarm.Data.Enums;
using Disfarm.Services.Discord.Embed;
using Disfarm.Services.Discord.Emote.Extensions;
using Disfarm.Services.Discord.Guild.Queries;
using Disfarm.Services.Discord.Image.Queries;
using Disfarm.Services.Extensions;
using Disfarm.Services.Game.Calculation;
using Disfarm.Services.Game.Collection.Commands;
using Disfarm.Services.Game.Fish.Commands;
using Disfarm.Services.Game.Fish.Queries;
using Disfarm.Services.Game.Localization;
using Disfarm.Services.Game.Statistic.Commands;
using Disfarm.Services.Game.Transit.Commands;
using Disfarm.Services.Game.User.Commands;
using Disfarm.Services.Game.User.Queries;
using Disfarm.Services.Game.World.Queries;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Disfarm.Services.Hangfire.BackgroundJobs.CompleteFishing
{
    public class CompleteFishingJob : ICompleteFishingJob
    {
        private readonly IMediator _mediator;
        private readonly ILogger<CompleteFishingJob> _logger;
        private readonly ILocalizationService _local;

        public CompleteFishingJob(
            IMediator mediator,
            ILogger<CompleteFishingJob> logger,
            ILocalizationService local)
        {
            _mediator = mediator;
            _logger = logger;
            _local = local;
        }

        public async Task Execute(ulong guildId, long userId, uint cubeDrop)
        {
            _logger.LogInformation(
                "Complete fishing job executed for user {UserId}",
                userId);

            var emotes = DiscordRepository.Emotes;
            var user = await _mediator.Send(new GetUserQuery(userId));
            var socketUser = await _mediator.Send(new GetSocketGuildUserQuery(guildId, (ulong) user.Id));
            var timesDay = await _mediator.Send(new GetCurrentTimesDayQuery());
            var state = await _mediator.Send(new GetWorldStateQuery());
            var rarity = await _mediator.Send(new GetRandomFishRarityQuery(cubeDrop));
            var fish = await _mediator.Send(new GetRandomFishWithParamsQuery(rarity, state.WeatherToday, timesDay,
                state.CurrentSeason));
            var success = await _mediator.Send(new CheckFishingSuccessQuery(fish.Rarity, cubeDrop));
            var fishingXp = await _mediator.Send(new GetWorldPropertyValueQuery(
                WorldProperty.XpFishing));

            await _mediator.Send(new UpdateUserCommand(user with {Location = Location.Neutral}));
            await _mediator.Send(new AddXpToUserCommand(socketUser.Guild.Id, user.Id, fishingXp));
            await _mediator.Send(new DeleteUserMovementCommand(user.Id));

            var embed = new EmbedBuilder()
                .WithAuthor(Location.Fishing.Localize(user.Language), socketUser.GetAvatarUrl())
                .WithImageUrl(await _mediator.Send(new GetImageUrlQuery(Data.Enums.Image.Fishing, user.Language)));

            if (success)
            {
                await _mediator.Send(new AddFishToUserCommand(userId, fish.Id, 1));
                await _mediator.Send(new AddCollectionToUserCommand(userId, CollectionCategory.Fish, fish.Id));
                await _mediator.Send(new AddStatisticToUserCommand(userId, Statistic.CatchFish));
                // await _mediator.Send(new CheckAchievementsInUserCommand(user.Id, new[]
                // {
                //     Achievement.FirstFish,
                //     Achievement.Catch50Fish,
                //     Achievement.Catch100Fish,
                //     Achievement.Catch300Fish
                // }));

                // switch (rarity)
                // {
                //     case FishRarity.Common:
                //     case FishRarity.Rare:
                //         // ignored
                //         break;
                //     case FishRarity.Epic:
                //         await _mediator.Send(new CheckAchievementInUserCommand(userId, Achievement.CatchEpicFish));
                //         break;
                //     case FishRarity.Mythical:
                //         await _mediator.Send(new CheckAchievementInUserCommand(userId, Achievement.CatchMythicalFish));
                //         break;
                //     case FishRarity.Legendary:
                //         await _mediator.Send(new CheckAchievementInUserCommand(userId, Achievement.CatchLegendaryFish));
                //         break;
                //     default:
                //         throw new ArgumentOutOfRangeException();
                // }

                embed
                    .WithDescription(
                        Response.CompleteFishingSuccessDesc.Parse(user.Language,
                            socketUser.Mention.AsGameMention(user.Title, user.Language)) +
                        $"\n{StringExtensions.EmptyChar}")
                    .AddField(Response.CompleteFishingRewardTitle.Parse(user.Language),
                        Response.CompleteFishingRewardSuccessDesc.Parse(user.Language,
                            emotes.GetEmote("Xp"), fishingXp, emotes.GetEmote(fish.Name),
                            _local.Localize(LocalizationCategory.Fish, fish.Name, user.Language)));
            }
            else
            {
                embed
                    .WithDescription(
                        Response.CompleteFishingFailDesc.Parse(user.Language,
                            socketUser.Mention.AsGameMention(user.Title, user.Language), emotes.GetEmote(fish.Name),
                            _local.Localize(LocalizationCategory.Fish, fish.Name, user.Language)) +
                        $"\n{StringExtensions.EmptyChar}")
                    .AddField(Response.CompleteFishingRewardTitle.Parse(user.Language),
                        Response.CompleteFishingRewardFailDesc.Parse(user.Language,
                            emotes.GetEmote("Xp"), fishingXp));
            }

            await _mediator.Send(new SendEmbedToUserCommand(socketUser.Guild.Id, socketUser.Id, embed));
        }
    }
}