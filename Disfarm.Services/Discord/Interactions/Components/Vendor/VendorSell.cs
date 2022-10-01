using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Interactions;
using Disfarm.Data.Enums;
using Disfarm.Services.Discord.Embed;
using Disfarm.Services.Discord.Emote.Extensions;
using Disfarm.Services.Discord.Image.Queries;
using Disfarm.Services.Discord.Interactions.Attributes;
using Disfarm.Services.Extensions;
using Disfarm.Services.Game.Achievement.Commands;
using Disfarm.Services.Game.Crop.Commands;
using Disfarm.Services.Game.Crop.Queries;
using Disfarm.Services.Game.Currency.Commands;
using Disfarm.Services.Game.Fish.Commands;
using Disfarm.Services.Game.Fish.Queries;
using Disfarm.Services.Game.Localization;
using Disfarm.Services.Game.Statistic.Commands;
using Disfarm.Services.Game.User.Queries;
using MediatR;
using static Disfarm.Services.Extensions.ExceptionExtensions;

namespace Disfarm.Services.Discord.Interactions.Components.Vendor
{
    [RequireLocation(Location.Neutral)]
    public class VendorSell : InteractionModuleBase<SocketInteractionContext>
    {
        private readonly IMediator _mediator;
        private readonly ILocalizationService _local;

        public VendorSell(
            IMediator mediator,
            ILocalizationService local)
        {
            _mediator = mediator;
            _local = local;
        }

        [ComponentInteraction("vendor-sell:*")]
        public async Task Execute(string category)
        {
            await DeferAsync();

            var emotes = DiscordRepository.Emotes;
            var user = await _mediator.Send(new GetUserQuery((long) Context.User.Id));

            var embed = new EmbedBuilder()
                .WithUserColor(user.CommandColor)
                .WithAuthor(Response.VendorAuthor.Parse(user.Language), Context.User.GetAvatarUrl())
                .WithImageUrl(await _mediator.Send(new GetImageUrlQuery(Data.Enums.Image.Vendor, user.Language)));

            uint totalCurrencyAmount = 0;
            var soldItems = string.Empty;

            switch (category)
            {
                case "fish":
                {
                    var userFishes = await _mediator.Send(new GetUserFishesQuery(user.Id));

                    if (userFishes.Any() is false)
                    {
                        throw new GameUserExpectedException(
                            Response.VendorSellFishNothing.Parse(user.Language));
                    }

                    foreach (var userFish in userFishes)
                    {
                        await _mediator.Send(new RemoveFishFromUserCommand(
                            user.Id, userFish.Fish.Id, userFish.Amount));
                        await _mediator.Send(new AddStatisticToUserCommand(
                            user.Id, Statistic.VendorSell, userFish.Amount));

                        var currencyAmount = userFish.Fish.Price * userFish.Amount;
                        totalCurrencyAmount += currencyAmount;

                        soldItems +=
                            $"{emotes.GetEmote(userFish.Fish.Name)} {userFish.Amount} " +
                            $"{_local.Localize(LocalizationCategory.Fish, userFish.Fish.Name, user.Language, userFish.Amount)} " +
                            $"- {emotes.GetEmote(Currency.Token.ToString())} {currencyAmount} " +
                            $"{_local.Localize(LocalizationCategory.Currency, Currency.Token.ToString(), user.Language, currencyAmount)}\n";
                    }

                    break;
                }

                case "crops":
                {
                    var userCrops = await _mediator.Send(new GetUserCropsQuery(user.Id));

                    if (userCrops.Any() is false)
                    {
                        throw new GameUserExpectedException(
                            Response.VendorSellCropsNothing.Parse(user.Language));
                    }

                    foreach (var userCrop in userCrops)
                    {
                        await _mediator.Send(new RemoveCropFromUserCommand(
                            user.Id, userCrop.Crop.Id, userCrop.Amount));
                        await _mediator.Send(new AddStatisticToUserCommand(
                            user.Id, Statistic.VendorSell, userCrop.Amount));

                        var currencyAmount = userCrop.Crop.Price * userCrop.Amount;
                        totalCurrencyAmount += currencyAmount;

                        soldItems +=
                            $"{emotes.GetEmote(userCrop.Crop.Name)} {userCrop.Amount} " +
                            $"{_local.Localize(LocalizationCategory.Crop, userCrop.Crop.Name, user.Language, userCrop.Amount)} " +
                            $"- {emotes.GetEmote(Currency.Token.ToString())} {currencyAmount} " +
                            $"{_local.Localize(LocalizationCategory.Currency, Currency.Token.ToString(), user.Language, currencyAmount)}\n";
                    }

                    break;
                }
            }

            await _mediator.Send(new AddCurrencyToUserCommand(user.Id, Currency.Token, totalCurrencyAmount));
            await _mediator.Send(new CheckAchievementsInUserCommand(Context.Guild.Id, Context.Channel.Id, user.Id, new[]
            {
                Achievement.FirstVendorDeal,
                Achievement.Vendor100Sell,
                Achievement.Vendor777Sell,
                Achievement.Vendor1500Sell,
                Achievement.Vendor3500Sell
            }));

            var descString =
                soldItems +
                Response.VendorSellResultMoney.Parse(user.Language,
                    emotes.GetEmote(Currency.Token.ToString()), totalCurrencyAmount,
                    _local.Localize(LocalizationCategory.Currency, Currency.Token.ToString(), user.Language,
                        totalCurrencyAmount));

            embed
                .WithDescription(
                    Response.VendorSellDesc.Parse(user.Language,
                        Context.User.Mention.AsGameMention(user.Title, user.Language)) +
                    $"\n{StringExtensions.EmptyChar}")
                .AddField(Response.VendorSellResultTitle.Parse(user.Language),
                    descString.Length > 1024
                        ? Response.VendorSellResultTooLong.Parse(user.Language) +
                          Response.VendorSellResultMoney.Parse(user.Language,
                              emotes.GetEmote(Currency.Token.ToString()), totalCurrencyAmount,
                              _local.Localize(LocalizationCategory.Currency, Currency.Token.ToString(), user.Language,
                                  totalCurrencyAmount))
                        : descString);

            await ModifyOriginalResponseAsync(x =>
            {
                x.Embed = embed.Build();
                x.Components = new ComponentBuilder().Build();
            });
        }
    }
}