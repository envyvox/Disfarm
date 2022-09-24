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
using Disfarm.Services.Discord.Interactions.Attributes;
using Disfarm.Services.Extensions;
using Disfarm.Services.Game.Farm.Queries;
using Disfarm.Services.Game.Localization;
using Disfarm.Services.Game.User.Queries;
using Disfarm.Services.Game.World.Queries;
using MediatR;
using static Discord.Emote;

namespace Disfarm.Services.Discord.Interactions.Commands.UserInfo
{
    [RequireLocation(Location.Neutral)]
    public class UserFarm : InteractionModuleBase<SocketInteractionContext>
    {
        private readonly IMediator _mediator;
        private readonly ILocalizationService _local;

        public UserFarm(
            IMediator mediator,
            ILocalizationService local)
        {
            _mediator = mediator;
            _local = local;
        }

        [SlashCommand("farm", "View and manage your farm")]
        public async Task Execute()
        {
            await DeferAsync(true);

            var emotes = DiscordRepository.Emotes;
            var user = await _mediator.Send(new GetUserQuery((long) Context.User.Id));
            var userFarms = await _mediator.Send(new GetUserFarmsQuery(user.Id));

            var embed = new EmbedBuilder()
                .WithUserColor(user.CommandColor)
                .WithAuthor(Response.UserFarmAuthor.Parse(user.Language), Context.User.GetAvatarUrl())
                .WithImageUrl(await _mediator.Send(new GetImageUrlQuery(Data.Enums.Image.Harvesting, user.Language)));

            var components = new ComponentBuilder();

            if (userFarms.Any())
            {
                embed.WithDescription(
                    Response.UserFarmDesc.Parse(user.Language,
                        Context.User.Mention.AsGameMention(user.Title, user.Language),
                        emotes.GetEmote(Building.Farm.ToString())) +
                    $"\n{StringExtensions.EmptyChar}");

                foreach (var userFarm in userFarms)
                {
                    string fieldName;
                    string fieldDesc;

                    switch (userFarm.State)
                    {
                        case FieldState.Empty:

                            fieldName = Response.UserFarmFieldEmptyTitle.Parse(user.Language);
                            fieldDesc = Response.UserFarmFieldEmptyDesc.Parse(user.Language);

                            break;
                        case FieldState.Planted:
                        {
                            var growthDays = userFarm.InReGrowth
                                ? userFarm.Seed.ReGrowthDays - userFarm.Progress
                                : userFarm.Seed.GrowthDays - userFarm.Progress;

                            fieldName = Response.UserFarmFieldPlantedTitle.Parse(user.Language,
                                emotes.GetEmote(userFarm.Seed.Name),
                                _local.Localize(LocalizationCategory.Seed, userFarm.Seed.Name, user.Language),
                                DateTimeOffset.UtcNow.AddDays(growthDays)
                                    .ToDiscordTimestamp(TimestampFormat.RelativeTime));

                            fieldDesc = Response.UserFarmFieldPlantedDesc.Parse(user.Language);

                            break;
                        }

                        case FieldState.Watered:
                        {
                            var growthDays = userFarm.InReGrowth
                                ? userFarm.Seed.ReGrowthDays - userFarm.Progress
                                : userFarm.Seed.GrowthDays - userFarm.Progress;

                            fieldName = Response.UserFarmFieldWateredTitle.Parse(user.Language,
                                emotes.GetEmote(userFarm.Seed.Name),
                                _local.Localize(LocalizationCategory.Seed, userFarm.Seed.Name, user.Language),
                                DateTimeOffset.UtcNow.AddDays(growthDays)
                                    .ToDiscordTimestamp(TimestampFormat.RelativeTime));

                            fieldDesc = Response.UserFarmFieldWateredDesc.Parse(user.Language);

                            break;
                        }

                        case FieldState.Completed:

                            fieldName = Response.UserFarmFieldCompletedTitle.Parse(user.Language,
                                emotes.GetEmote(userFarm.Seed.Crop.Name),
                                _local.Localize(LocalizationCategory.Crop, userFarm.Seed.Crop.Name, user.Language));

                            fieldDesc = userFarm.Seed.ReGrowthDays > 0
                                ? Response.UserFarmFieldCompletedReGrowthDesc.Parse(user.Language,
                                    DateTimeOffset.UtcNow.AddDays(userFarm.Seed.ReGrowthDays)
                                        .ToDiscordTimestamp(TimestampFormat.RelativeTime))
                                : Response.UserFarmFieldCompletedDesc.Parse(user.Language);

                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                    embed.AddField($"{emotes.GetEmote("List")} `#{userFarm.Number}` {fieldName}", fieldDesc);
                }

                components.WithRows(new[]
                {
                    new ActionRowBuilder()
                        .WithButton(
                            Response.ComponentUserFarmPlant.Parse(user.Language),
                            "user-farm-plant:1",
                            disabled: userFarms.Any(x => x.State is FieldState.Empty) is false)
                        .WithButton(
                            Response.ComponentUserFarmWater.Parse(user.Language),
                            "user-farm-water",
                            disabled: userFarms.Any(x => x.State is FieldState.Planted) is false)
                        .WithButton(
                            Response.ComponentUserFarmCollect.Parse(user.Language),
                            "user-farm-collect",
                            disabled: userFarms.Any(x => x.State is FieldState.Completed) is false)
                        .WithButton(
                            Response.ComponentUserFarmDig.Parse(user.Language),
                            "user-farm-dig",
                            ButtonStyle.Danger,
                            disabled: userFarms.All(x => x.State is FieldState.Empty)),
                    new ActionRowBuilder()
                        .WithButton(
                            Response.ComponentUserFarmQaHarvesting.Parse(user.Language),
                            "user-farm-qa:harvesting",
                            ButtonStyle.Secondary,
                            emote: Parse(emotes.GetEmote("DiscordHelp")))
                        .WithButton(
                            Response.ComponentUserFarmQaUpgrading.Parse(user.Language),
                            "user-farm-qa:upgrading",
                            ButtonStyle.Secondary,
                            emote: Parse(emotes.GetEmote("DiscordHelp")))
                });
            }
            else
            {
                var farmPrice = await _mediator.Send(new GetWorldPropertyValueQuery(WorldProperty.FarmPrice));

                embed.WithDescription(Response.UserFarmNeedToBuyDesc.Parse(user.Language,
                    Context.User.Mention.AsGameMention(user.Title, user.Language),
                    emotes.GetEmote(Building.Farm.ToString()), emotes.GetEmote(Currency.Token.ToString()), farmPrice,
                    _local.Localize(LocalizationCategory.Currency, Currency.Token.ToString(), user.Language, farmPrice),
                    emotes.GetEmote("Arrow")));

                components.WithButton(
                    Response.ComponentUserFarmBuy.Parse(user.Language),
                    "farm-buy",
                    emote: Parse(emotes.GetEmote("Farm")));
            }

            await _mediator.Send(new FollowUpEmbedCommand(Context.Interaction, embed, components.Build()));
        }
    }
}