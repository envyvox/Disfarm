using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Interactions;
using Disfarm.Data.Enums;
using Disfarm.Services.Discord.Embed;
using Disfarm.Services.Discord.Emote.Extensions;
using Disfarm.Services.Discord.Emote.Models;
using Disfarm.Services.Discord.Extensions;
using Disfarm.Services.Discord.Image.Queries;
using Disfarm.Services.Extensions;
using Disfarm.Services.Game.Container.Models;
using Disfarm.Services.Game.Container.Queries;
using Disfarm.Services.Game.Crop.Models;
using Disfarm.Services.Game.Crop.Queries;
using Disfarm.Services.Game.Currency.Models;
using Disfarm.Services.Game.Currency.Queries;
using Disfarm.Services.Game.Fish.Models;
using Disfarm.Services.Game.Fish.Queries;
using Disfarm.Services.Game.Localization;
using Disfarm.Services.Game.Seed.Models;
using Disfarm.Services.Game.Seed.Queries;
using Disfarm.Services.Game.User.Queries;
using MediatR;
using static Discord.Emote;

namespace Disfarm.Services.Discord.Interactions.Commands.UserInfo
{
    [RequireContext(ContextType.Guild)]
    public class UserInventory : InteractionModuleBase<SocketInteractionContext>
    {
        private readonly IMediator _mediator;
        private readonly ILocalizationService _local;
        private Dictionary<string, EmoteDto> _emotes;

        public UserInventory(
            IMediator mediator,
            ILocalizationService local)
        {
            _mediator = mediator;
            _local = local;
        }

        [SlashCommand("inventory", "View your inventory. All received items go here.")]
        public async Task Execute(
            [Summary("category", "Inventory category you want to see")]
            [Choice("Fish", "fish")]
            [Choice("Seeds", "seeds")]
            [Choice("Crops", "crops")]
            string category = null)
        {
            await DeferAsync(true);

            _emotes = DiscordRepository.Emotes;
            var user = await _mediator.Send(new GetUserQuery((long) Context.User.Id));

            var embed = new EmbedBuilder()
                .WithUserColor(user.CommandColor)
                .WithAuthor(Response.UserInventoryAuthor.Parse(user.Language), Context.User.GetAvatarUrl())
                .WithImageUrl(
                    await _mediator.Send(new GetImageUrlQuery(Data.Enums.Image.UserInventory, user.Language)));

            var components = new ComponentBuilder();

            var desc = string.Empty;
            if (category is null)
            {
                desc = Response.UserInventoryNoCategoryDesc.Parse(user.Language);
                var userCurrencies = await _mediator.Send(new GetUserCurrenciesQuery(user.Id));
                var userContainers = await _mediator.Send(new GetUserContainersQuery(user.Id));
                var userFishes = await _mediator.Send(new GetUserFishesQuery(user.Id));
                var userSeeds = await _mediator.Send(new GetUserSeedsQuery(user.Id));
                var userCrops = await _mediator.Send(new GetUserCropsQuery(user.Id));

                embed
                    .AddField(Response.UserInventoryCurrencyTitle.Parse(user.Language),
                        DisplayUserCurrencies(userCurrencies, user.Language))
                    .AddField(Response.UserInventoryContainersTitle.Parse(user.Language),
                        DisplayUserContainers(userContainers, user.Language));

                if (userFishes.Any())
                {
                    embed.AddField(Response.UserInventoryFishesTitle.Parse(user.Language),
                        DisplayUserFishes(userFishes, user.Language));
                }

                if (userSeeds.Any())
                {
                    embed.AddField(Response.UserInventorySeedsTitle.Parse(user.Language),
                        DisplayUserSeeds(userSeeds, user.Language));
                }

                if (userCrops.Any())
                {
                    embed.AddField(Response.UserInventoryCropsTitle.Parse(user.Language),
                        DisplayUserCrops(userCrops, user.Language));
                }

                components
                    .WithButton(
                        Response.ComponentContainerOpenTokens.Parse(user.Language),
                        $"container-open:{Container.Token.GetHashCode()}",
                        emote: Parse(_emotes.GetEmote(Container.Token.EmoteName())),
                        disabled: (userContainers.ContainsKey(Container.Token) &&
                                   userContainers[Container.Token].Amount > 0) is false)
                    .WithButton(
                        Response.ComponentContainerOpenSupplies.Parse(user.Language),
                        $"container-open:{Container.Supply.GetHashCode()}",
                        emote: Parse(_emotes.GetEmote(Container.Supply.EmoteName())),
                        // todo включить когда будет функционал
                        disabled: true);
            }
            else
            {
                switch (category)
                {
                    case "fish":
                    {
                        desc = Response.UserInventoryFishesCategoryDesc.Parse(user.Language);
                        var userFishes = await _mediator.Send(new GetUserFishesQuery(user.Id));

                        foreach (var rarity in Enum.GetValues(typeof(FishRarity)).Cast<FishRarity>())
                        {
                            embed.AddField(rarity.Localize(user.Language),
                                DisplayUserFishes(userFishes.Where(x => x.Fish.Rarity == rarity), user.Language));
                        }

                        break;
                    }
                    case "seeds":

                        desc = Response.UserInventorySeedsCategoryDesc.Parse(user.Language);
                        var userSeeds = await _mediator.Send(new GetUserSeedsQuery(user.Id));

                        foreach (var season in Enum
                                     .GetValues(typeof(Season))
                                     .Cast<Season>()
                                     .Where(x => x != Season.Any))
                        {
                            embed.AddField(season.Localize(user.Language),
                                DisplayUserSeeds(userSeeds.Where(x => x.Seed.Season == season), user.Language));
                        }

                        break;
                    case "crops":

                        desc = Response.UserInventoryCropsCategoryDesc.Parse(user.Language);
                        var userCrops = await _mediator.Send(new GetUserCropsQuery(user.Id));

                        foreach (var season in Enum
                                     .GetValues(typeof(Season))
                                     .Cast<Season>()
                                     .Where(x => x != Season.Any))
                        {
                            embed.AddField(season.Localize(user.Language),
                                DisplayUserCrops(userCrops.Where(x => x.Crop.Seed.Season == season), user.Language));
                        }

                        break;
                }
            }

            embed.WithDescription(
                $"{Context.User.Mention.AsGameMention(user.Title, user.Language)}, " + desc +
                $"\n{StringExtensions.EmptyChar}");

            await Context.Interaction.FollowUpResponse(embed, components.Build());
        }

        private string DisplayUserCurrencies(IReadOnlyDictionary<Currency, UserCurrencyDto> userCurrencies,
            Language language)
        {
            var str = Enum
                .GetValues(typeof(Currency))
                .Cast<Currency>()
                .Aggregate(string.Empty, (s, v) =>
                    s +
                    $"{_emotes.GetEmote(v.ToString())} {(userCurrencies.ContainsKey(v) ? userCurrencies[v].Amount : 0)} " +
                    $"{_local.Localize(LocalizationCategory.Currency, v.ToString(), language, userCurrencies.ContainsKey(v) ? userCurrencies[v].Amount : 0)}, ");

            return str.RemoveFromEnd(2);
        }

        private string DisplayUserContainers(IReadOnlyDictionary<Container, UserContainerDto> userContainers,
            Language language)
        {
            var str = Enum
                .GetValues(typeof(Container))
                .Cast<Container>()
                .Aggregate(string.Empty, (s, v) =>
                    s +
                    $"{_emotes.GetEmote(v.EmoteName())} {(userContainers.ContainsKey(v) ? userContainers[v].Amount : 0)} " +
                    $"{_local.Localize(LocalizationCategory.Container, v.ToString(), language, userContainers.ContainsKey(v) ? userContainers[v].Amount : 0)}, ");

            return str.RemoveFromEnd(2);
        }

        private string DisplayUserFishes(IEnumerable<UserFishDto> userFishes, Language language)
        {
            var str = userFishes.Aggregate(string.Empty, (s, v) =>
                s +
                $"{_emotes.GetEmote(v.Fish.Name)} {v.Amount} {_local.Localize(LocalizationCategory.Fish, v.Fish.Name, language, v.Amount)}, ");

            return str.Length > 0
                ? str.Length > 1024
                    ? Response.UserInventoryTooMuchFishes.Parse(language)
                    : str.RemoveFromEnd(2)
                : Response.UserInventoryCategoryEmpty.Parse(language);
        }

        private string DisplayUserSeeds(IEnumerable<UserSeedDto> userSeeds, Language language)
        {
            var str = userSeeds.Aggregate(string.Empty, (s, v) =>
                s +
                $"{_emotes.GetEmote(v.Seed.Name)} {v.Amount} {_local.Localize(LocalizationCategory.Seed, v.Seed.Name, language, v.Amount)}, ");

            return str.Length > 0
                ? str.Length > 1024
                    ? Response.UserInventoryTooMuchSeeds.Parse(language)
                    : str.RemoveFromEnd(2)
                : Response.UserInventoryCategoryEmpty.Parse(language);
        }

        private string DisplayUserCrops(IEnumerable<UserCropDto> userCrops, Language language)
        {
            var str = userCrops.Aggregate(string.Empty, (s, v) =>
                s +
                $"{_emotes.GetEmote(v.Crop.Name)} {v.Amount} {_local.Localize(LocalizationCategory.Crop, v.Crop.Name, language, v.Amount)}, ");

            return str.Length > 0
                ? str.Length > 1024
                    ? Response.UserInventoryTooMuchCrops.Parse(language)
                    : str.RemoveFromEnd(2)
                : Response.UserInventoryCategoryEmpty.Parse(language);
        }
    }
}