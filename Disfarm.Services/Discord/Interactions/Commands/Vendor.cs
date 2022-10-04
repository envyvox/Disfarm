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

namespace Disfarm.Services.Discord.Interactions.Commands
{
    [RequireContext(ContextType.Guild)]
    [RequireLocation(Location.Neutral)]
    [Group("vendor", "Vendor")]
    public class Vendor : InteractionModuleBase<SocketInteractionContext>
    {
        private readonly IMediator _mediator;
        private readonly ILocalizationService _local;

        public Vendor(IMediator mediator, ILocalizationService local)
        {
            _mediator = mediator;
            _local = local;
        }

        [SlashCommand("view", "View what products the vendor is ready to buy (also has bulk sell)")]
        public async Task Execute(
            [Summary("category", "The category of the products you want to view or bulk sell")]
            [Choice("Fish", "fish")]
            [Choice("Crops", "crops")]
            string category)
        {
            await DeferAsync(true);

            var emotes = DiscordRepository.Emotes;
            var user = await _mediator.Send(new GetUserQuery((long) Context.User.Id));

            var embed = new EmbedBuilder()
                .WithUserColor(user.CommandColor)
                .WithAuthor(Response.VendorAuthor.Parse(user.Language), Context.User.GetAvatarUrl())
                .WithDescription(
                    Response.VendorDesc.Parse(user.Language,
                        Context.User.Mention.AsGameMention(user.Title, user.Language)) +
                    $"\n{StringExtensions.EmptyChar}")
                .WithImageUrl(await _mediator.Send(new GetImageUrlQuery(Data.Enums.Image.Vendor, user.Language)));

            var components = new ComponentBuilder()
                .WithButton(
                    Response.ComponentPaginatorBack.Parse(user.Language),
                    $"vendor-paginator:{category},1",
                    disabled: true)
                .WithButton(
                    Response.ComponentPaginatorForward.Parse(user.Language),
                    $"vendor-paginator:{category},2");

            switch (category)
            {
                case "fish":
                {
                    var fishes = await _mediator.Send(new GetFishesQuery());
                    var maxPage = (int) Math.Ceiling(fishes.Count / 10.0);
                    maxPage = maxPage > 0 ? maxPage : 1; // just for better display

                    fishes = fishes
                        .Take(10)
                        .ToList();

                    var counter = 0;
                    foreach (var fish in fishes)
                    {
                        counter++;

                        embed.AddField(
                            $"{emotes.GetEmote(fish.Name)} {_local.Localize(LocalizationCategory.Fish, fish.Name, user.Language)}",
                            Response.VendorItemPrice.Parse(user.Language,
                                emotes.GetEmote(Currency.Token.ToString()), fish.Price,
                                _local.Localize(LocalizationCategory.Currency, Currency.Token.ToString(), user.Language,
                                    fish.Price)),
                            true);

                        if (counter == 2)
                        {
                            counter = 0;
                            embed.AddEmptyField(true);
                        }
                    }

                    embed.WithFooter(Response.PaginatorFooter.Parse(user.Language, 1, maxPage));

                    components.WithButton(
                        Response.ComponentVendorSellFish.Parse(user.Language),
                        $"vendor-bulk-sell:{category}",
                        ButtonStyle.Danger);

                    break;
                }

                case "crops":
                {
                    var crops = await _mediator.Send(new GetCropsQuery());
                    var maxPage = (int) Math.Ceiling(crops.Count / 10.0);
                    maxPage = maxPage > 0 ? maxPage : 1; // just for better display

                    crops = crops
                        .Take(10)
                        .ToList();

                    var counter = 0;
                    foreach (var crop in crops)
                    {
                        counter++;

                        embed.AddField(
                            $"{emotes.GetEmote(crop.Name)} {_local.Localize(LocalizationCategory.Crop, crop.Name, user.Language)}",
                            Response.VendorItemPrice.Parse(user.Language,
                                emotes.GetEmote(Currency.Token.ToString()), crop.Price,
                                _local.Localize(LocalizationCategory.Currency, Currency.Token.ToString(), user.Language,
                                    crop.Price)),
                            true);

                        if (counter == 2)
                        {
                            counter = 0;
                            embed.AddEmptyField(true);
                        }
                    }

                    embed.WithFooter(Response.PaginatorFooter.Parse(user.Language, 1, maxPage));

                    components.WithButton(
                        Response.ComponentVendorSellCrops.Parse(user.Language),
                        $"vendor-bulk-sell:{category}",
                        ButtonStyle.Danger);

                    break;
                }
            }

            await Context.Interaction.FollowUpResponse(embed, components.Build());
        }

        [SlashCommand("sell", "Sell the specified product to a vendor")]
        public async Task Execute(
            [Summary("category", "Category of product you want to sell")]
            [Choice("fish", "fish")]
            [Choice("crops", "crops")]
            string category,
            [Summary("name", "Name of the product (use autocomplete options!)")]
            [Autocomplete(typeof(VendorAutocompleteHandler))]
            string name,
            [Summary("amount", "Amount you want to sell")] [MinValue(1)]
            uint amount)
        {
            await DeferAsync();

            var emotes = DiscordRepository.Emotes;
            var user = await _mediator.Send(new GetUserQuery((long) Context.User.Id));

            var embed = new EmbedBuilder()
                .WithUserColor(user.CommandColor)
                .WithAuthor(Response.VendorAuthor.Parse(user.Language))
                .WithImageUrl(await _mediator.Send(new GetImageUrlQuery(Data.Enums.Image.Vendor, user.Language)));

            switch (category)
            {
                case "fish":
                {
                    var fish = await _mediator.Send(new GetFishByNameQuery(name));
                    var userFish = await _mediator.Send(new GetUserFishQuery(user.Id, fish.Id));

                    if (userFish.Amount < amount)
                    {
                        throw new GameUserExpectedException(Response.VendorSellDontHaveThatMuch.Parse(user.Language,
                            emotes.GetEmote(fish.Name),
                            _local.Localize(LocalizationCategory.Fish, fish.Name, user.Language, 2)));
                    }

                    await _mediator.Send(new RemoveFishFromUserCommand(user.Id, userFish.Fish.Id, userFish.Amount));
                    await _mediator.Send(new AddCurrencyToUserCommand(user.Id, Currency.Token, fish.Price * amount));
                    await _mediator.Send(new AddStatisticToUserCommand(user.Id, Statistic.VendorSell, userFish.Amount));

                    embed.WithDescription(Response.VendorSellDesc.Parse(user.Language,
                        Context.User.Mention.AsGameMention(user.Title, user.Language),
                        emotes.GetEmote(fish.Name), amount,
                        _local.Localize(LocalizationCategory.Fish, fish.Name, user.Language, amount),
                        emotes.GetEmote(Currency.Token.ToString()), fish.Price * amount,
                        _local.Localize(LocalizationCategory.Currency, Currency.Token.ToString(), user.Language,
                            fish.Price * amount)));

                    break;
                }
                case "crops":
                {
                    var crop = await _mediator.Send(new GetCropByNameQuery(name));
                    var userCrop = await _mediator.Send(new GetUserCropQuery(user.Id, crop.Id));

                    if (userCrop.Amount < amount)
                    {
                        throw new GameUserExpectedException(Response.VendorSellDontHaveThatMuch.Parse(user.Language,
                            emotes.GetEmote(crop.Name),
                            _local.Localize(LocalizationCategory.Crop, crop.Name, user.Language, 2)));
                    }

                    await _mediator.Send(new RemoveCropFromUserCommand(user.Id, userCrop.Crop.Id, userCrop.Amount));
                    await _mediator.Send(new AddCurrencyToUserCommand(user.Id, Currency.Token, crop.Price * amount));
                    await _mediator.Send(new AddStatisticToUserCommand(user.Id, Statistic.VendorSell, userCrop.Amount));

                    embed.WithDescription(Response.VendorSellDesc.Parse(user.Language,
                        Context.User.Mention.AsGameMention(user.Title, user.Language),
                        emotes.GetEmote(crop.Name), amount,
                        _local.Localize(LocalizationCategory.Crop, crop.Name, user.Language, amount),
                        emotes.GetEmote(Currency.Token.ToString()), crop.Price * amount,
                        _local.Localize(LocalizationCategory.Currency, Currency.Token.ToString(), user.Language,
                            crop.Price * amount)));

                    break;
                }
            }

            await Context.Interaction.FollowUpResponse(embed);
        }
    }
}