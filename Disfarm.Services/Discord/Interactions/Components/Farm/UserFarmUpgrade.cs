using System;
using System.Threading.Tasks;
using Discord;
using Discord.Interactions;
using Disfarm.Data.Enums;
using Disfarm.Services.Discord.Emote.Extensions;
using Disfarm.Services.Discord.Extensions;
using Disfarm.Services.Discord.Image.Queries;
using Disfarm.Services.Discord.Interactions.Attributes;
using Disfarm.Services.Extensions;
using Disfarm.Services.Game.Building.Commands;
using Disfarm.Services.Game.Currency.Commands;
using Disfarm.Services.Game.Currency.Queries;
using Disfarm.Services.Game.Farm.Commands;
using Disfarm.Services.Game.Localization;
using Disfarm.Services.Game.User.Queries;
using Disfarm.Services.Game.World.Queries;
using MediatR;
using static Disfarm.Services.Extensions.ExceptionExtensions;

namespace Disfarm.Services.Discord.Interactions.Components.Farm
{
    [RequireLocation(Location.Neutral)]
    public class UserFarmUpgrade : InteractionModuleBase<SocketInteractionContext>
    {
        private readonly IMediator _mediator;
        private readonly ILocalizationService _local;

        public UserFarmUpgrade(
            IMediator mediator,
            ILocalizationService local)
        {
            _mediator = mediator;
            _local = local;
        }

        [ComponentInteraction("user-farm-upgrade:*")]
        public async Task Execute(string buildingHashcode)
        {
            await DeferAsync();

            var building = (Building)int.Parse(buildingHashcode);
            var emotes = DiscordRepository.Emotes;
            var user = await _mediator.Send(new GetUserQuery((long)Context.User.Id));
            var userCurrency = await _mediator.Send(new GetUserCurrencyQuery(user.Id, Currency.Token));
            var buildingPrice = await _mediator.Send(new GetWorldPropertyValueQuery(
                (WorldProperty)Enum.Parse(typeof(WorldProperty), building + "Price")));

            if (userCurrency.Amount < buildingPrice)
            {
                throw new GameUserExpectedException(Response.UserFarmUpgradeNoCurrency.Parse(user.Language,
                    emotes.GetEmote(Currency.Token.ToString()),
                    _local.Localize(LocalizationCategory.Currency, Currency.Token.ToString(), user.Language, 5),
                    emotes.GetEmote(building.ToString())));
            }

            await _mediator.Send(new RemoveCurrencyFromUserCommand(user.Id, Currency.Token, buildingPrice));
            await _mediator.Send(new CreateUserBuildingCommand(user.Id, building));

            if (building.NewCells() is not null)
            {
                await _mediator.Send(new CreateUserFarmsCommand(user.Id, building.NewCells()));
            }

            var embed = new EmbedBuilder()
                .WithUserColor(user.CommandColor)
                .WithAuthor(Response.UserFarmUpgradeAuthor.Parse(user.Language), Context.User.GetAvatarUrl())
                .WithDescription(Response.UserFarmUpgradeDesc.Parse(user.Language,
                    Context.User.Mention.AsGameMention(user.Title, user.Language),
                    emotes.GetEmote(building.EmoteName()), emotes.GetEmote(Currency.Token.ToString()), buildingPrice,
                    _local.Localize(LocalizationCategory.Currency, Currency.Token.ToString(), user.Language,
                        buildingPrice)))
                .WithImageUrl(await _mediator.Send(new GetImageUrlQuery(Data.Enums.Image.Harvesting, user.Language)));

            await Context.Interaction.FollowUpResponse(embed);
            await Context.Interaction.ClearOriginalResponse(user.Language);
        }
    }
}