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
using Disfarm.Services.Game.Currency.Commands;
using Disfarm.Services.Game.Currency.Queries;
using Disfarm.Services.Game.Localization;
using Disfarm.Services.Game.Seed.Commands;
using Disfarm.Services.Game.Seed.Queries;
using Disfarm.Services.Game.User.Queries;
using MediatR;
using static Disfarm.Services.Extensions.ExceptionExtensions;

namespace Disfarm.Services.Discord.Interactions.Components.Shop
{
    [RequireLocation(Location.Neutral)]
    public class ShopSeedBuy : InteractionModuleBase<SocketInteractionContext>
    {
        private readonly IMediator _mediator;
        private readonly ILocalizationService _local;

        public ShopSeedBuy(
            IMediator mediator,
            ILocalizationService local)
        {
            _mediator = mediator;
            _local = local;
        }

        [ComponentInteraction("shop-seed-buy")]
        public async Task Execute(string[] selectedValues)
        {
            await DeferAsync();

            var seedId = Guid.Parse(selectedValues.First());

            var emotes = DiscordRepository.Emotes;
            var user = await _mediator.Send(new GetUserQuery((long) Context.User.Id));
            var seed = await _mediator.Send(new GetSeedQuery(seedId));
            var userCurrency = await _mediator.Send(new GetUserCurrencyQuery(user.Id, Currency.Token));

            if (userCurrency.Amount < seed.Price * 5)
            {
                throw new GameUserExpectedException(Response.ShopSeedBuyNoCurrency.Parse(user.Language,
                    emotes.GetEmote(Currency.Token.ToString()),
                    _local.Localize(LocalizationCategory.Currency, Currency.Token.ToString(), user.Language, 5),
                    emotes.GetEmote(seed.Name),
                    _local.Localize(LocalizationCategory.Seed, seed.Name, user.Language, 5)));
            }

            await _mediator.Send(new RemoveCurrencyFromUserCommand(user.Id, Currency.Token, seed.Price * 5));
            await _mediator.Send(new AddSeedToUserCommand(user.Id, seed.Id, 5));

            var embed = new EmbedBuilder()
                .WithUserColor(user.CommandColor)
                .WithAuthor(Response.ShopSeedBuyAuthor.Parse(user.Language), Context.User.GetAvatarUrl())
                .WithDescription(Response.ShopSeedBuyDesc.Parse(user.Language,
                    Context.User.Mention.AsGameMention(user.Title, user.Language), emotes.GetEmote(seed.Name),
                    _local.Localize(LocalizationCategory.Seed, seed.Name, user.Language, 5),
                    emotes.GetEmote(Currency.Token.ToString()), seed.Price * 5,
                    _local.Localize(LocalizationCategory.Currency, Currency.Token.ToString(), user.Language,
                        seed.Price * 5)))
                .WithImageUrl(await _mediator.Send(new GetImageUrlQuery(Data.Enums.Image.ShopSeed, user.Language)));

            await ModifyOriginalResponseAsync(x =>
            {
                x.Embed = embed.Build();
                x.Components = new ComponentBuilder().Build();
            });
        }
    }
}