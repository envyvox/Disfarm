using System;
using System.Threading.Tasks;
using Discord;
using Discord.Interactions;
using Disfarm.Data.Enums;
using Disfarm.Services.Discord.Emote.Extensions;
using Disfarm.Services.Discord.Extensions;
using Disfarm.Services.Discord.Image.Queries;
using Disfarm.Services.Extensions;
using Disfarm.Services.Game.Container.Commands;
using Disfarm.Services.Game.Currency.Commands;
using Disfarm.Services.Game.DailyReward.Commands;
using Disfarm.Services.Game.DailyReward.Queries;
using Disfarm.Services.Game.Fish.Commands;
using Disfarm.Services.Game.Fish.Queries;
using Disfarm.Services.Game.Localization;
using Disfarm.Services.Game.Seed.Commands;
using Disfarm.Services.Game.Seed.Queries;
using Disfarm.Services.Game.User.Queries;
using Disfarm.Services.Game.World.Queries;
using MediatR;

namespace Disfarm.Services.Discord.Interactions.Components
{
    public class DailyReward : InteractionModuleBase<SocketInteractionContext>
    {
        private readonly IMediator _mediator;
        private readonly ILocalizationService _local;
        private readonly TimeZoneInfo _timeZoneInfo;

        public DailyReward(
            IMediator mediator,
            ILocalizationService local,
            TimeZoneInfo timeZoneInfo)
        {
            _mediator = mediator;
            _local = local;
            _timeZoneInfo = timeZoneInfo;
        }

        [ComponentInteraction("receive-daily-reward")]
        public async Task ExecuteReceive()
        {
            await DeferAsync();

            var timeNow = TimeZoneInfo.ConvertTime(DateTimeOffset.UtcNow, _timeZoneInfo);
            var emotes = DiscordRepository.Emotes;
            var user = await _mediator.Send(new GetUserQuery((long) Context.User.Id));

            await _mediator.Send(new CreateUserDailyRewardCommand(user.Id, timeNow.DayOfWeek));

            string rewardString;
            switch (timeNow.DayOfWeek)
            {
                case DayOfWeek.Monday:
                {
                    var amountWithoutPremium = await _mediator.Send(new GetWorldPropertyValueQuery(
                        WorldProperty.DailyRewardMondayAmountWithoutPremium));
                    var amountPremium = await _mediator.Send(new GetWorldPropertyValueQuery(
                        WorldProperty.DailyRewardMondayAmountPremium));
                    var amount = user.IsPremium ? amountPremium : amountWithoutPremium;

                    await _mediator.Send(new AddCurrencyToUserCommand(user.Id, Currency.Token, amount));

                    rewardString =
                        $"{emotes.GetEmote(Currency.Token.ToString())} {amount} " +
                        $"{_local.Localize(LocalizationCategory.Currency, Currency.Token.ToString(), user.Language, amount)}";

                    break;
                }

                case DayOfWeek.Tuesday:
                {
                    var amountWithoutPremium = await _mediator.Send(new GetWorldPropertyValueQuery(
                        WorldProperty.DailyRewardTuesdayAmountWithoutPremium));
                    var amountPremium = await _mediator.Send(new GetWorldPropertyValueQuery(
                        WorldProperty.DailyRewardTuesdayAmountPremium));
                    var amount = user.IsPremium ? amountPremium : amountWithoutPremium;

                    await _mediator.Send(new AddCurrencyToUserCommand(user.Id, Currency.Chip, amount));

                    rewardString =
                        $"{emotes.GetEmote(Currency.Chip.ToString())} {amount} " +
                        $"{_local.Localize(LocalizationCategory.Currency, Currency.Chip.ToString(), user.Language, amount)}";

                    break;
                }

                case DayOfWeek.Wednesday:
                {
                    var amountWithoutPremium = await _mediator.Send(new GetWorldPropertyValueQuery(
                        WorldProperty.DailyRewardWednesdayAmountWithoutPremium));
                    var amountPremium = await _mediator.Send(new GetWorldPropertyValueQuery(
                        WorldProperty.DailyRewardWednesdayAmountPremium));
                    var amount = user.IsPremium ? amountPremium : amountWithoutPremium;

                    await _mediator.Send(new AddCurrencyToUserCommand(user.Id, Currency.Token, amount));

                    rewardString =
                        $"{emotes.GetEmote(Currency.Token.ToString())} {amount} " +
                        $"{_local.Localize(LocalizationCategory.Currency, Currency.Token.ToString(), user.Language, amount)}";

                    break;
                }

                case DayOfWeek.Thursday:
                {
                    var amountWithoutPremium = await _mediator.Send(new GetWorldPropertyValueQuery(
                        WorldProperty.DailyRewardThursdayAmountWithoutPremium));
                    var amountPremium = await _mediator.Send(new GetWorldPropertyValueQuery(
                        WorldProperty.DailyRewardThursdayAmountPremium));
                    var amount = user.IsPremium ? amountPremium : amountWithoutPremium;

                    await _mediator.Send(new AddContainerToUserCommand(user.Id, Container.Supply, amount));

                    rewardString =
                        $"{emotes.GetEmote(Container.Supply.EmoteName())} {amount} " +
                        $"{_local.Localize(LocalizationCategory.Container, Container.Supply.ToString(), user.Language, amount)}";

                    break;
                }
                case DayOfWeek.Friday:
                {
                    var amountWithoutPremium = await _mediator.Send(new GetWorldPropertyValueQuery(
                        WorldProperty.DailyRewardFridayAmountWithoutPremium));
                    var amountPremium = await _mediator.Send(new GetWorldPropertyValueQuery(
                        WorldProperty.DailyRewardFridayAmountPremium));
                    var amount = user.IsPremium ? amountPremium : amountWithoutPremium;
                    var randomFish = await _mediator.Send(new GetRandomFishWithRarityQuery(FishRarity.Epic));

                    await _mediator.Send(new AddFishToUserCommand(user.Id, randomFish.Id, amount));

                    rewardString =
                        $"{emotes.GetEmote(randomFish.Name)} {amount} " +
                        $"{_local.Localize(LocalizationCategory.Fish, randomFish.Name, user.Language, amount)}";

                    break;
                }
                case DayOfWeek.Saturday:
                {
                    var amountWithoutPremium = await _mediator.Send(new GetWorldPropertyValueQuery(
                        WorldProperty.DailyRewardSaturdayAmountWithoutPremium));
                    var amountPremium = await _mediator.Send(new GetWorldPropertyValueQuery(
                        WorldProperty.DailyRewardSaturdayAmountPremium));
                    var amount = user.IsPremium ? amountPremium : amountWithoutPremium;
                    var randomSeed = await _mediator.Send(new GetRandomSeedQuery());

                    await _mediator.Send(new AddSeedToUserCommand(user.Id, randomSeed.Id, amount));

                    rewardString =
                        $"{emotes.GetEmote(randomSeed.Name)} {amount} " +
                        $"{_local.Localize(LocalizationCategory.Seed, randomSeed.Name, user.Language, amount)}";

                    break;
                }
                case DayOfWeek.Sunday:
                {
                    var amountWithoutPremium = await _mediator.Send(new GetWorldPropertyValueQuery(
                        WorldProperty.DailyRewardSundayAmountWithoutPremium));
                    var amountPremium = await _mediator.Send(new GetWorldPropertyValueQuery(
                        WorldProperty.DailyRewardSundayAmountPremium));
                    var amount = user.IsPremium ? amountPremium : amountWithoutPremium;

                    await _mediator.Send(new AddCurrencyToUserCommand(user.Id, Currency.Chip, amount));

                    rewardString =
                        $"{emotes.GetEmote(Currency.Chip.ToString())} {amount} " +
                        $"{_local.Localize(LocalizationCategory.Currency, Currency.Chip.ToString(), user.Language, amount)}";

                    break;
                }

                default:
                    throw new ArgumentOutOfRangeException();
            }

            var userDailyRewards = await _mediator.Send(new GetUserDailyRewardsQuery(user.Id));

            if (userDailyRewards.Count is 7)
            {
                var amountWithoutPremium = await _mediator.Send(new GetWorldPropertyValueQuery(
                    WorldProperty.DailyRewardBonusAmountWithoutPremium));
                var amountPremium = await _mediator.Send(new GetWorldPropertyValueQuery(
                    WorldProperty.DailyRewardBonusAmountPremium));
                var amount = user.IsPremium ? amountPremium : amountWithoutPremium;

                await _mediator.Send(new AddCurrencyToUserCommand(user.Id, Currency.Token, amount));

                rewardString += Response.DailyRewardReceivedBonus.Parse(user.Language,
                    emotes.GetEmote(Currency.Token.ToString()), amount,
                    _local.Localize(LocalizationCategory.Currency, Currency.Token.ToString(), user.Language, amount));
            }

            var embed = new EmbedBuilder()
                .WithUserColor(user.CommandColor)
                .WithAuthor(Response.DailyRewardAuthor.Parse(user.Language), Context.User.GetAvatarUrl())
                .WithDescription(Response.DailyRewardReceivedDesc.Parse(user.Language,
                    Context.User.Mention.AsGameMention(user.Title, user.Language), rewardString))
                .WithImageUrl(await _mediator.Send(new GetImageUrlQuery(Data.Enums.Image.DailyReward, user.Language)));

            await Context.Interaction.FollowUpResponse(embed);
            await Context.Interaction.ClearOriginalResponse(user.Language);
        }

        [ComponentInteraction("show-daily-rewards")]
        public async Task ExecuteShow()
        {
            await DeferAsync();

            var user = await _mediator.Send(new GetUserQuery((long) Context.User.Id));

            var embed = new EmbedBuilder()
                .WithUserColor(user.CommandColor)
                .WithAuthor(Response.DailyRewardAuthor.Parse(user.Language), Context.User.GetAvatarUrl())
                .WithImageUrl(await _mediator.Send(new GetImageUrlQuery(user.IsPremium
                    ? Data.Enums.Image.DailyRewardInfoPremium
                    : Data.Enums.Image.DailyRewardInfo, user.Language)));

            await Context.Interaction.FollowUpResponse(embed, ephemeral: true);
        }
    }
}