using System;
using System.Threading.Tasks;
using Discord;
using Discord.Interactions;
using Disfarm.Data.Enums;
using Disfarm.Services.Discord.Emote.Extensions;
using Disfarm.Services.Discord.Extensions;
using Disfarm.Services.Discord.Image.Queries;
using Disfarm.Services.Extensions;
using Disfarm.Services.Game.Achievement.Commands;
using Disfarm.Services.Game.Cooldown.Commands;
using Disfarm.Services.Game.Cooldown.Queries;
using Disfarm.Services.Game.Currency.Commands;
using Disfarm.Services.Game.Currency.Queries;
using Disfarm.Services.Game.Localization;
using Disfarm.Services.Game.Statistic.Commands;
using Disfarm.Services.Game.User.Queries;
using Disfarm.Services.Game.World.Queries;
using MediatR;
using static Discord.Emote;
using static Disfarm.Services.Extensions.ExceptionExtensions;

namespace Disfarm.Services.Discord.Interactions.Commands
{
    [Group("casino", ".")]
    [RequireContext(ContextType.Guild)]
    public class Casino : InteractionModuleBase<SocketInteractionContext>
    {
        private readonly IMediator _mediator;
        private readonly ILocalizationService _local;
        private readonly Random _random = new();

        public Casino(
            IMediator mediator,
            ILocalizationService local)
        {
            _mediator = mediator;
            _local = local;
        }

        [SlashCommand("bet", "Make a bet in the casino")]
        public async Task Execute(
            [Summary("amount", "The amount you want to bet")] [MinValue(20)] [MaxValue(200)]
            uint amount)
        {
            await DeferAsync();

            var emotes = DiscordRepository.Emotes;
            var user = await _mediator.Send(new GetUserQuery((long) Context.User.Id));
            var userCooldown = await _mediator.Send(new GetUserCooldownQuery(user.Id, Cooldown.CasinoBet));

            if (userCooldown.Expiration > DateTimeOffset.UtcNow)
            {
                throw new GameUserExpectedException(Response.CasinoBetCooldown.Parse(user.Language,
                    userCooldown.Expiration.ToDiscordTimestamp(TimestampFormat.RelativeTime)));
            }

            var userCurrency = await _mediator.Send(new GetUserCurrencyQuery(user.Id, Currency.Token));

            if (userCurrency.Amount < amount)
            {
                throw new GameUserExpectedException(Response.CasinoBetNoCurrency.Parse(user.Language,
                    emotes.GetEmote(Currency.Token.ToString()),
                    _local.Localize(LocalizationCategory.Currency, Currency.Token.ToString(), user.Language, 5)));
            }

            double firstDrop = _random.Next(1, 101);
            double secondDrop = _random.Next(1, 101);
            var cubeDrop = Math.Floor((firstDrop + secondDrop) / 2);

            var cubeDropString = Response.CasinoBetDesc.Parse(user.Language,
                Context.User.Mention.AsGameMention(user.Title, user.Language), cubeDrop);

            switch (cubeDrop)
            {
                case >= 55 and < 90:

                    cubeDropString += Response.CasinoBetDescWon.Parse(user.Language,
                        emotes.GetEmote(Currency.Token.ToString()), amount * 2,
                        _local.Localize(LocalizationCategory.Currency, Currency.Token.ToString(), user.Language,
                            amount * 2));

                    await _mediator.Send(new AddCurrencyToUserCommand(
                        user.Id, Currency.Token, amount));

                    break;

                case >= 90 and < 100:

                    cubeDropString += Response.CasinoBetDescWon.Parse(user.Language,
                        emotes.GetEmote(Currency.Token.ToString()), amount * 4,
                        _local.Localize(LocalizationCategory.Currency, Currency.Token.ToString(), user.Language,
                            amount * 4));

                    await _mediator.Send(new AddCurrencyToUserCommand(
                        user.Id, Currency.Token, amount * 3));

                    break;

                case 100:

                    cubeDropString += Response.CasinoBetDescWon.Parse(user.Language,
                        emotes.GetEmote(Currency.Token.ToString()), amount * 10,
                        _local.Localize(LocalizationCategory.Currency, Currency.Token.ToString(), user.Language,
                            amount * 10));

                    await _mediator.Send(new AddCurrencyToUserCommand(
                        user.Id, Currency.Token, amount * 9));
                    await _mediator.Send(new AddStatisticToUserCommand(
                        user.Id, Statistic.CasinoJackpot));
                    await _mediator.Send(new CheckAchievementInUserCommand(user.Id, Achievement.FirstJackpot));

                    break;

                default:

                    cubeDropString += Response.CasinoBetDescLose.Parse(user.Language,
                        emotes.GetEmote(Currency.Token.ToString()), amount,
                        _local.Localize(LocalizationCategory.Currency, Currency.Token.ToString(), user.Language,
                            amount));

                    await _mediator.Send(new RemoveCurrencyFromUserCommand(
                        user.Id, Currency.Token, amount));

                    break;
            }

            var cooldownDurationInMinutes = await _mediator.Send(new GetWorldPropertyValueQuery(
                WorldProperty.CasinoBetCooldownDurationInMinutes));

            await _mediator.Send(new AddCooldownToUserCommand(
                user.Id, Cooldown.CasinoBet, TimeSpan.FromMinutes(cooldownDurationInMinutes)));
            await _mediator.Send(new AddStatisticToUserCommand(user.Id, Statistic.CasinoBet));
            await _mediator.Send(new CheckAchievementsInUserCommand(user.Id, new[]
            {
                Achievement.FirstBet,
                Achievement.Casino33Bet,
                Achievement.Casino333Bet,
                Achievement.Casino777Bet
            }));

            var embed = new EmbedBuilder()
                .WithUserColor(user.CommandColor)
                .WithAuthor(Response.CasinoBetAuthor.Parse(user.Language), Context.User.GetAvatarUrl())
                .WithDescription(cubeDropString)
                .WithImageUrl(await _mediator.Send(new GetImageUrlQuery(Data.Enums.Image.Casino, user.Language)));

            var components = new ComponentBuilder()
                .WithButton(
                    Response.ComponentHowCasinoBetWorks.Parse(user.Language),
                    "how-casino-bet-works",
                    ButtonStyle.Secondary,
                    Parse(emotes.GetEmote("DiscordHelp")))
                .Build();

            await Context.Interaction.FollowUpResponse(embed, components);
        }

        [ComponentInteraction("how-casino-bet-works", true)]
        public async Task Execute()
        {
            await DeferAsync();

            var emotes = DiscordRepository.Emotes;
            var user = await _mediator.Send(new GetUserQuery((long) Context.User.Id));

            var embed = new EmbedBuilder()
                .WithUserColor(user.CommandColor)
                .WithAuthor(Response.HowCasinoBetWorksAuthor.Parse(user.Language), Context.User.GetAvatarUrl())
                .WithDescription(Response.HowCasinoBetWorksDesc.Parse(user.Language,
                    Context.User.Mention.AsGameMention(user.Title, user.Language),
                    emotes.GetEmote("Arrow"), emotes.GetEmote(Currency.Token.ToString())))
                .WithImageUrl(await _mediator.Send(new GetImageUrlQuery(Data.Enums.Image.Casino, user.Language)));

            await Context.Interaction.FollowUpResponse(embed, ephemeral: true);
        }
    }
}