using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Interactions;
using Disfarm.Data.Enums;
using Disfarm.Services.Discord.Emote.Extensions;
using Disfarm.Services.Discord.Emote.Models;
using Disfarm.Services.Discord.Extensions;
using Disfarm.Services.Discord.Image.Queries;
using Disfarm.Services.Discord.Interactions.Attributes;
using Disfarm.Services.Extensions;
using Disfarm.Services.Game.Building.Queries;
using Disfarm.Services.Game.Localization;
using Disfarm.Services.Game.User.Queries;
using Disfarm.Services.Game.World.Queries;
using MediatR;
using static Discord.Emote;

namespace Disfarm.Services.Discord.Interactions.Components.Farm
{
    [RequireLocation(Location.Neutral)]
    public class UserFarmQa : InteractionModuleBase<SocketInteractionContext>
    {
        private readonly IMediator _mediator;
        private readonly ILocalizationService _local;
        private Dictionary<string, EmoteDto> _emotes;

        public UserFarmQa(
            IMediator mediator,
            ILocalizationService local)
        {
            _mediator = mediator;
            _local = local;
        }

        [ComponentInteraction("user-farm-qa:*")]
        public async Task Execute(string selectedQuestion)
        {
            await DeferAsync();

            _emotes = DiscordRepository.Emotes;
            var user = await _mediator.Send(new GetUserQuery((long)Context.User.Id));

            var embed = new EmbedBuilder()
                .WithUserColor(user.CommandColor)
                .WithImageUrl(await _mediator.Send(new GetImageUrlQuery(Data.Enums.Image.Harvesting, user.Language)));

            var components = new ComponentBuilder();

            switch (selectedQuestion)
            {
                case "harvesting":
                {
                    embed
                        .WithAuthor(Response.UserFarmQaHarvestingAuthor.Parse(user.Language),
                            Context.User.GetAvatarUrl())
                        .WithDescription(Response.UserFarmQaHarvestingDesc.Parse(user.Language,
                            Context.User.Mention.AsGameMention(user.Title, user.Language),
                            _emotes.GetEmote("Arrow"), _emotes.GetEmote(Building.Farm.ToString())));

                    break;
                }
                case "upgrading":
                {
                    embed
                        .WithAuthor(Response.UserFarmQaUpgradingAuthor.Parse(user.Language),
                            Context.User.GetAvatarUrl())
                        .WithDescription(Response.UserFarmQaUpgradingDesc.Parse(user.Language,
                            Context.User.Mention.AsGameMention(user.Title, user.Language),
                            _emotes.GetEmote(Building.Farm.ToString()), _emotes.GetEmote("Arrow")));

                    var userBuildings =
                        await _mediator.Send(new GetUserBuildingsQuery((long)Context.User.Id, BuildingCategory.Farm));
                    var currentUpgrade = BuildingHelper.CurrentUpgrade(userBuildings);
                    var nextUpgrade = currentUpgrade.NextUpgrade();

                    foreach (var building in Enum
                                 .GetValues(typeof(Building))
                                 .Cast<Building>()
                                 .Where(x => x.Category() is BuildingCategory.Farm))
                    {
                        var hasUpgrade = currentUpgrade >= building;
                        var price = await _mediator.Send(new GetWorldPropertyValueQuery(
                            (WorldProperty)Enum.Parse(typeof(WorldProperty), building + "Price")));

                        embed.AddField(StringExtensions.EmptyChar,
                            $"{_emotes.GetEmote(hasUpgrade ? "Checkmark" : "List")} " +
                            $"{DisplayUpgradeInfo(building, user.Language, price)}");
                    }

                    components.WithButton(
                        Response.ComponentFarmUpgrade.Parse(user.Language),
                        @$"user-farm-upgrade:{nextUpgrade.GetHashCode()}",
                        emote: Parse(_emotes.GetEmote(nextUpgrade.EmoteName())),
                        disabled: nextUpgrade is Building.Undefined);

                    break;
                }
            }

            await Context.Interaction.FollowUpResponse(embed, components.Build(), ephemeral: true);
        }

        private string DisplayUpgradeInfo(Building building, Language language, uint price)
        {
            if (building is Building.Farm)
            {
                return language switch {
                    Language.English => 
                        $"Purchase {_emotes.GetEmote(building.EmoteName())} farm for " +
                        $"{_emotes.GetEmote(Currency.Token.ToString())} {price} " +
                        $"{_local.Localize(LocalizationCategory.Currency, Currency.Token.ToString(), language, price)} " +
                        "that unlocks **3 starting slots**",
                    Language.Russian =>
                        $"Приобретение {_emotes.GetEmote(building.EmoteName())} фермы за " +
                        $"{_emotes.GetEmote(Currency.Token.ToString())} {price} " +
                        $"{_local.Localize(LocalizationCategory.Currency, Currency.Token.ToString(), language, price)} " +
                        "открывающей **3 стартовых ячейки**",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                };
            }
            var newCells = building.NewCells();
            var speedBonusPercent = building.SpeedBonusPercent();

            if (newCells is not null)
            {
                return language switch
                {
                    Language.English =>
                        $"Upgrade for {_emotes.GetEmote(Currency.Token.ToString())} {price} " +
                        $"{_local.Localize(LocalizationCategory.Currency, Currency.Token.ToString(), language, price)} " +
                        $"that unlocks {_emotes.GetEmote(building.EmoteName())} **{newCells.Count()} extra slot**",
                    Language.Russian =>
                        $"Улучшение за {_emotes.GetEmote(Currency.Token.ToString())} {price} " +
                        $"{_local.Localize(LocalizationCategory.Currency, Currency.Token.ToString(), language, price)} " +
                        $"открывающее {_emotes.GetEmote(building.EmoteName())} **{newCells.Count()} дополнительную ячейку**",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                };
            }

            if (speedBonusPercent is not null)
            {
                return language switch
                {
                    Language.English =>
                        $"Upgrade for {_emotes.GetEmote(Currency.Token.ToString())} {price} " +
                        $"{_local.Localize(LocalizationCategory.Currency, Currency.Token.ToString(), language, price)} " +
                        $"that speeds up {_emotes.GetEmote(building.EmoteName())} **yield growth by {speedBonusPercent}%**",
                    Language.Russian =>
                        $"Улучшение за {_emotes.GetEmote(Currency.Token.ToString())} {price} " +
                        $"{_local.Localize(LocalizationCategory.Currency, Currency.Token.ToString(), language, price)} " +
                        $"ускоряющее {_emotes.GetEmote(building.EmoteName())} **рост урожая на {speedBonusPercent}%**",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                };
            }

            return string.Empty;
        }
    }
}