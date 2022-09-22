using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Interactions;
using Disfarm.Data.Enums;
using Disfarm.Services.Discord.Emote.Extensions;
using Disfarm.Services.Discord.Image.Queries;
using Disfarm.Services.Discord.Interactions.Attributes;
using Disfarm.Services.Extensions;
using Disfarm.Services.Game.Crop.Queries;
using Disfarm.Services.Game.Fish.Queries;
using Disfarm.Services.Game.Localization;
using Disfarm.Services.Game.User.Queries;
using MediatR;

namespace Disfarm.Services.Discord.Interactions.Components.Vendor
{
    [RequireLocation(Location.Neutral)]
    public class VendorPaginator : InteractionModuleBase<SocketInteractionContext>
    {
        private readonly IMediator _mediator;
        private readonly ILocalizationService _local;

        public VendorPaginator(
            IMediator mediator,
            ILocalizationService local)
        {
            _mediator = mediator;
            _local = local;
        }

        [ComponentInteraction("vendor-paginator:*,*")]
        public async Task Execute(string category, string pageString)
        {
            await DeferAsync(true);

            var page = int.Parse(pageString);
            int maxPage;

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

            var components = new ComponentBuilder();

            switch (category)
            {
                case "fish":
                {
                    var fishes = await _mediator.Send(new GetFishesQuery());
                    maxPage = (int) Math.Ceiling(fishes.Count / 10.0);
                    maxPage = maxPage > 0 ? maxPage : 1; // just for better display

                    fishes = fishes
                        .Skip(page > 1 ? (page - 1) * 10 : 0)
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

                    break;
                }

                case "crops":
                {
                    var crops = await _mediator.Send(new GetCropsQuery());
                    maxPage = (int) Math.Ceiling(crops.Count / 10.0);
                    maxPage = maxPage > 0 ? maxPage : 1; // just for better display

                    crops = crops
                        .Skip(page > 1 ? (page - 1) * 10 : 0)
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

                    break;
                }

                default:
                    throw new ArgumentOutOfRangeException();
            }

            embed.WithFooter(Response.PaginatorFooter.Parse(user.Language, page, maxPage));

            components
                .WithButton(
                    Response.ComponentPaginatorBack.Parse(user.Language),
                    $"vendor-paginator:{category},{page - 1}",
                    disabled: page <= 1)
                .WithButton(
                    Response.ComponentPaginatorForward.Parse(user.Language),
                    $"vendor-paginator:{category},{page + 1}",
                    disabled: page >= maxPage);

            switch (category)
            {
                case "fish":
                {
                    components.WithButton(
                        Response.ComponentVendorSellFish.Parse(user.Language),
                        $"vendor-sell:{category}",
                        ButtonStyle.Success);
                    break;
                }
                case "crops":
                {
                    components.WithButton(
                        Response.ComponentVendorSellCrops.Parse(user.Language),
                        $"vendor-sell:{category}",
                        ButtonStyle.Success);
                    break;
                }
            }

            await ModifyOriginalResponseAsync(x =>
            {
                x.Embed = embed.Build();
                x.Components = components.Build();
            });
        }
    }
}